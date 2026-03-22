# GAMEPLAY SYSTEMS

## Gameplay Loop

Power Prank 3D currently runs on a single-scene, multi-level gameplay loop.

Core loop:
1. Start encounter
2. Throw prank item
3. Register hit
4. Apply breakdown pressure
5. Trigger enemy reactions / defense behavior
6. Advance goal progress
7. Reach encounter goal or run out of time
8. Show result panel
9. Player chooses Retry or Next
10. Restart current level or advance to next level

---

## Core Gameplay Systems

### GameplayManager
Owns core runtime gameplay state.

Responsibilities:
- current breakdown value
- target breakdown value
- round duration / remaining time
- current selected item
- round running / round finished state
- active enemy reaction layer reference
- round finish event dispatch
- breakdown-based win condition enable / disable
- force-finish support for non-breakdown goals

Important boundary:
- GameplayManager owns gameplay state
- it does not own level progression
- it does not own result button execution logic
- it does not decide encounter goal definitions

### ThrowController
Owns throw input and projectile launch flow.

Responsibilities:
- drag / aim input
- projectile launch request
- preview / drag reset on round transitions

Important runtime rule:
- transient drag state must reset when rounds finish or levels restart

### ProjectileBehavior
Owns projectile movement and hit delivery.

Responsibilities:
- movement
- collision
- hit payload application
- impact VFX / SFX
- stain spawn
- report resolved hit data into LevelGoalController

Current hit payload report:
- `isHeadHit`
- `itemId`
- `gainedScore`

### LevelGoalController
Owns current encounter goal runtime progress.

Responsibilities:
- apply encounter primary goal
- reset goal progress when encounter starts
- consume resolved hit events
- evaluate current goal completion
- finish round for non-breakdown goals
- provide readable goal summary text for UI

Important boundary:
- LevelGoalController does not own level progression
- it does not select encounters
- it does not own raw gameplay state like timer or breakdown
- it consumes runtime hit results and converts them into encounter objective progress

### HitPopupSpawner
Owns floating hit feedback text.

Responsibilities:
- score popup spawning
- visual gameplay feedback for successful hits

### HudController
Owns player-facing HUD and result presentation.

Responsibilities:
- current breakdown text
- target breakdown text
- timer text
- selected item text
- combo display
- result panel visibility
- result title / subtitle text
- level info text
- goal progress text
- final-level notice text
- Retry / Next button text refresh
- button click delegation into LevelProgressionController

Important boundary:
- HudController presents UI only
- it must not own multi-level progression logic
- it must not directly apply encounter data
- it must not replace GameplayManager as round-state owner

---

## Level Goal Variety 1.0

Level Goal Variety 1.0 is now implemented and runtime-validated.

Each encounter may define one primary goal through:
- `LevelEncounterConfigData.primaryGoal`

### Supported goal types

#### 1. BreakdownTarget
Legacy breakdown-based win condition.

Behavior:
- round wins when breakdown reaches target breakdown value
- goal summary displays breakdown progress

#### 2. HeadHitCount
Precision goal.

Behavior:
- only hits resolved through collider Tag = `Head` advance progress
- body hits do not count
- round wins when head-hit target is reached

#### 3. SpecificItemHitCount
Item-restricted goal.

Behavior:
- only hits whose `itemId` matches `primaryGoal.requiredItemId` advance progress
- round wins when target count is reached

---

## Runtime Goal Flow

Encounter application flow:
LevelProgressionController  
→ LevelEncounterController  
→ GameplayManager target/time refresh  
→ GameplayManager breakdown-win-condition refresh  
→ LevelEnemySelectionController  
→ EnemySwitchingManager  
→ LevelGoalController.ApplyGoal(...)

Hit resolution flow:
ProjectileBehavior  
→ resolve collision / defense / score  
→ build `CombatHitInfo`  
→ LevelGoalController.NotifyHitResolved(...)

Victory flow:
- `BreakdownTarget` → GameplayManager wins from breakdown reaching target
- `HeadHitCount` → LevelGoalController calls GameplayManager.ForceFinishRound(true)
- `SpecificItemHitCount` → LevelGoalController calls GameplayManager.ForceFinishRound(true)

Result flow:
GameplayManager.OnRoundFinished  
→ HudController.ShowResultPanel  
→ Retry / Next button click  
→ LevelProgressionController.RestartCurrentLevel / AdvanceToNextLevel

---

## Enemy Gameplay Hitbox Structure

A repaired hitbox structure is now required.

### Valid structure
- `EnemyVisual` → visual-only
- `Torso` → body collider
- `HeadCollider` → dedicated head collider, Tag = `Head`
- `EnemyHitReaction` → shared gameplay parent (`DefenseBodyPivot`)

### Invalid structure
- using `EnemyVisual` collider as the main gameplay collider
- attaching `EnemyHitReaction` to a node that head/body hitboxes cannot resolve through `GetComponentInParent(...)`

### Why this matters
This prevents:
- visual-shell collider stealing hits before gameplay hitboxes
- head-hit goals never progressing
- body / head reactions resolving inconsistently

---

## Head Hit Detection Notes

Current runtime head-hit detection is collider-based.

Head hit is resolved by:
- projectile collision target tag
- `collision.collider.CompareTag("Head")`

Current validated setup:
- `HeadCollider` positioned and sized to cover upper-head gameplay space
- `HeadCollider` enlarged relative to early setup to avoid top-of-head miss cases

---

## Enemy Stain Attachment Behavior

Enemy hit stains are now handled differently from ground stains.

### Enemy hit stain behavior
- spawn on enemy hit
- parent to enemy hit target hierarchy
- rigidbody gravity / motion disabled on spawn
- remain attached after impact

### Ground stain behavior
- spawn on ground hit
- parent to world `Stains` root
- remain world-rooted

This prevents enemy hit stains from falling out of the scene after collision.

---

## Result Panel Polish 1.0

Result Panel Polish 1.0 upgrades the functional result screen into a readable prototype UI.

Implemented scene hierarchy:
- HUDCanvas
  - HudPanel
  - ResultPanel
    - Dimmer
    - SafeArea
      - ResultCard
        - Header
          - ResultTitleText
          - ResultSubtitleText
        - Body
          - LevelInfoText
          - GoalSummaryText
          - FinalLevelNoticeText
        - Actions
          - RetryButton
            - RetryButtonText
          - NextLevelButton
            - NextLevelButtonText

Implemented runtime presentation:
- startup scene keeps result panel hidden
- round finish shows localized title
- subtitle changes based on win/final-state condition
- level info text is shown on result panel
- goal progress text is shown on result panel
- final-level notice is supported
- Retry and Next use localized labels
- next button visibility depends on LevelProgressionController.HasNextLevel()

Localization keys required by result panel:
- `ui_retry`
- `ui_next_level`
- `result_victory`
- `result_failed`
- `result_ready_for_next`
- `result_all_levels_complete`
- `result_try_again`
- `ui_level`
- `ui_goal_progress`
- `ui_final_level_cleared`

Validation result:
- no placeholder `New Text` remains after proper inspector hookup
- no raw localization keys remain after CSV update
- result panel only appears after round finish
- button flow remains functional

Known current quality note:
- panel styling is prototype-level
- future polish may improve dimmer strength, button style, card styling, and HUD suppression during result display

---

## Current Gameplay Boundaries

Do:
- keep gameplay state in GameplayManager
- keep level flow in LevelProgressionController
- keep encounter application in LevelEncounterController
- keep goal runtime progress in LevelGoalController
- keep enemy startup selection in LevelEnemySelectionController
- keep result presentation in HudController
- keep gameplay colliders separate from visual shell colliders

Do not:
- move level progression into HUD
- move result UI ownership into GameplayManager
- bypass LevelEncounterController for runtime level application
- bypass EnemyPresetApplicator for preset injection
- enable defense window auto-cycle
- use EnemyVisual as the main gameplay hitbox