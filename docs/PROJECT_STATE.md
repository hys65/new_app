# PROJECT STATE

## Project
**Power Prank 3D**  
Unity 6.3 LTS small-scale prototype game.

Core fantasy:
- Throw prank items at enemy characters
- Build breakdown pressure
- Trigger defensive behaviors
- Create readable, funny, character-specific enemy reactions
- Progress through level-driven encounter goals

---

## Current Milestone Status

### Completed Milestones
- Core throw / hit / breakdown gameplay loop ✅
- Enemy Reaction Layer 1.0 ✅
- Enemy Defense Visual Layer 1.0 ✅
- Enemy Archetype System ✅
- Enemy AI Layer 1.0 ✅
- Enemy Switching System 1.0 ✅
- Enemy Roster / Level Enemy Selection 1.0 ✅
- Level Content / Encounter Configuration 1.0 ✅
- Level Progression / Multi-Level Content 1.0 ✅
- Runtime Level Advance 1.0 ✅
- Victory Choice Flow 1.0 ✅
- Result Panel Polish 1.0 ✅
- Level Goal Variety 1.0 ✅
- Enemy Hitbox Structure Repair ✅
- Enemy Stain Attachment Repair ✅

---

## Current Enemy System Status

Enemy behavior is fully data-driven.

Implemented behavior definition stack:
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- Combined through EnemyPresetData
- Applied through EnemyPresetApplicator

Implemented playable archetypes:

### 1. Meeting Tyrant
- Early defense trigger
- Strong guard
- Short recover
- Stable and hard to break

### 2. Narcissist Manager
- Late defense trigger
- Long telegraph
- Short guard
- Long recover
- High head weakness
- Easy to break

Both archetypes are confirmed working and visibly different in runtime.

---

## Enemy AI Layer 1.0 Status

Completed.

Current rules:
- AI controls defense timing
- EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
- Defense window system does not self-run
- AI decides when to enter defense
- Behavior differences are driven by data, not hardcoded enemy branches

Confirmed result:
- Meeting Tyrant and Narcissist Manager produce different defensive timing and feel using the same runtime controller stack

---

## Enemy Switching System 1.0 Status

Completed and validated.

Implemented runtime switching architecture:
- EnemyRuntimePresetController
- EnemySwitchingManager
- GameplayManager runtime active reaction target switching

Validated capabilities:
1. Runtime preset switching on the same enemy object
2. Switching between multiple real enemy objects in scene
3. Single active enemy model
4. Clean integration with EnemyPresetApplicator
5. Inspector-friendly slot-based setup
6. No breakage to existing hit / defense / AI flow

System behavior:
- Scene may contain multiple enemy roots
- Only one enemy is active at a time
- Active enemy is selected by EnemySwitchingManager
- Preset application still flows through EnemyPresetApplicator
- GameplayManager is updated to use the active enemy reaction layer

---

## Enemy Roster / Level Enemy Selection 1.0 Status

Completed and validated.

Implemented content selection architecture:
- EnemyRosterData
- LevelEnemySelectionData
- LevelEnemySelectionController
- Extended EnemySwitchingManager startup configuration flow

Validated capabilities:
1. Reusable enemy roster asset definition
2. Level-specific enemy selection asset
3. Level-driven startup enemy selection
4. Startup slot assignment through level content
5. Clean reuse of EnemySwitchingManager without bypassing preset injection rules
6. Removal of old competing startup setup path from the current scene

Validated startup selection result:
- `startupSelectionIndex = 0` starts with Meeting Tyrant
- `startupSelectionIndex = 1` starts with Narcissist Manager
- In both cases only one enemy remains active during play
- Preset application occurs once for the selected startup enemy
- Runtime result is clean after removing legacy `LevelEnemyController` scene hookup

---

## Level Content / Encounter Configuration 1.0 Status

Completed and validated.

Implemented:
- LevelEncounterConfigData
- LevelEncounterController

Capabilities:
- bind LevelEnemySelectionData into encounter config
- define target breakdown per level
- define round duration / time limit per level
- define encounter primary goal per level
- apply encounter config into runtime systems
- reduce manual scene-only gameplay setup

Validated encounter content:
- `level_01_encounter_config`
- `level_02_encounter_config`
- `level_03_encounter_config`

Validated runtime result:
- target breakdown changes correctly per encounter
- timer changes correctly per encounter
- enemy selection is applied through encounter config
- encounter application no longer relies on manual scene tweaking

---

## Level Progression / Multi-Level Content 1.0 Status

Completed and validated.

Implemented:
- LevelProgressionData
- LevelProgressionController

Capabilities:
1. Ordered multi-level encounter list
2. Startup level index
3. Single-scene multi-level runtime setup
4. Runtime application of Level 01 / Level 02 / Level 03 encounter configs
5. Runtime next-level progression
6. Runtime current-level restart

Validated runtime result:
- `startupLevelIndex = 0` starts Level 01
- `startupLevelIndex = 1` starts Level 02
- `startupLevelIndex = 2` starts Level 03
- target/time/enemy selection all refresh correctly when changing levels
- runtime progression works without reloading the scene

---

## Runtime Level Advance 1.0 Status

Completed and validated.

Capabilities:
- manual advance to next level
- automatic runtime re-application of encounter config
- runtime restart of current level
- runtime enemy reselection
- runtime timer reset
- runtime target breakdown reset

Transition stability fixes:
- ThrowController resets drag / input transient state on round end
- runtime level transition uses delayed progression to avoid same-frame end/start contamination
- runtime enemy slot switches immediately during level application

Validated result:
- advancing from Level 01 to Level 02 works
- advancing from Level 02 to Level 03 works
- gameplay remains playable after level transition
- no stuck drag state or central-screen projectile lock remains

---

## Victory Choice Flow 1.0 Status

Completed and validated.

Implemented through:
- HudController
- LevelProgressionController integration

Capabilities:
- victory result panel shown on win
- retry current level
- next level button shown only when next level exists
- final level hides next-level option
- auto-advance on win disabled for player-facing flow

Validated result:
- Level 01 victory stops and presents choice
- player can click Retry to replay current level
- player can click Next to enter Level 02
- current runtime flow now waits for player decision instead of forcing auto-advance

---

## Result Panel Polish 1.0 Status

Completed and validated.

Implemented through:
- HudController result panel expansion
- LocalizationManager CSV update
- Result panel hierarchy cleanup in scene

Implemented result UI structure:
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
  - NextLevelButton

Implemented presentation behavior:
- cleaner centered result card
- Retry / Next button row
- localized title / subtitle / button labels
- localized level label and goal progress label
- final-level-only notice support
- result panel remains hidden before round end
- result panel appears only after round finish

Validated runtime result:
- startup scene keeps result panel hidden
- win result shows proper Victory title
- subtitle displays localized state text
- level info and goal progress text display correctly
- Retry and Next labels localize correctly
- no raw localization keys remain on the result panel after CSV update

---

## Level Goal Variety 1.0 Status

Completed and runtime-validated.

Implemented:
- `LevelGoalType`
- `LevelGoalDefinition`
- `CombatHitInfo`
- `LevelGoalController`
- `LevelEncounterConfigData.primaryGoal`
- `GameplayManager.ConfigureBreakdownWinCondition(...)`
- `GameplayManager.ForceFinishRound(true)`
- `ProjectileBehavior` hit reporting into `LevelGoalController`

Validated supported goal types:
1. `BreakdownTarget`
2. `HeadHitCount`
3. `SpecificItemHitCount`

Validated runtime behavior:
- Level 1 can still run as standard breakdown-based encounter
- Head-hit-only goals now resolve correctly and finish the round on completion
- Item-specific hit goals now resolve correctly and finish the round on completion
- Result panel displays runtime goal summary correctly
- Non-breakdown goal victory no longer depends on reaching target breakdown

Validated content examples:
- `level_01_encounter_config` → `BreakdownTarget`
- `level_02_encounter_config` → `HeadHitCount`
- `level_03_encounter_config` → `SpecificItemHitCount(item_egg)`

---

## Enemy Hitbox Structure Status

Repaired and validated.

Previous issue:
- `EnemyVisual` collider intercepted projectile hits before dedicated head hitboxes
- disabling `EnemyVisual` collider revealed missing gameplay body collider coverage
- `EnemyHitReaction` was attached to the visual node instead of the shared gameplay parent

Validated working structure:
- `EnemyVisual` is visual-only
- `Torso` owns body collider
- `HeadCollider` owns head hit detection and uses Tag = `Head`
- `EnemyHitReaction` is attached to the shared gameplay parent (`DefenseBodyPivot`)

Validated result:
- body hits register correctly
- head hits register correctly
- head-hit goals now advance correctly
- upper-head coverage was improved by enlarging / repositioning `HeadCollider`
- visual shell no longer steals collision from gameplay hitboxes

---

## Enemy Stain Attachment Status

Repaired and validated.

Previous issue:
- enemy-hit stains were instantiated without being properly attached to the enemy target hierarchy
- stain rigidbody / gravity caused spawned decals to fall out of the screen

Implemented fix:
- enemy-hit stains now parent to the enemy hit target hierarchy
- stain rigidbody gravity / motion is disabled on spawn
- ground stains remain world-rooted under `Stains`

Validated result:
- enemy hit stains remain attached after impact
- enemy hit stains no longer fall away
- ground stain behavior remains valid

---

## Current Scene Validation Result

Validated scene setup:
- EnemyRoot_MeetingTyrant
- EnemyRoot_NarcissistManager
- EnemySwitchingManager with two enemy slots
- LevelEnemySelectionController with LevelEnemySelectionData
- LevelEncounterController
- LevelProgressionController
- LevelGoalController
- multi-level progression asset
- HUD result panel with Retry / Next flow

Validated runtime result:
- before play, both enemy objects exist in scene
- during play, only the active enemy remains enabled
- level-driven enemy startup works
- encounter-driven target/time works
- progression-driven level switching works
- result panel works
- three validated goal types work
- repaired hitbox structure works
- repaired stain attachment works

---

## Current Architecture Position

The project has moved from:

**multi-level runtime flow + result panel validation**

to:

**runtime-validated goal-driven encounter flow with repaired enemy gameplay hitboxes**

The current stack is now:

Data  
→ EnemyPresetData  
→ EnemyPresetApplicator  
→ EnemyRuntimePresetController  
→ EnemySwitchingManager  
→ LevelEnemySelectionController / LevelEnemySelectionData  
→ LevelEncounterController / LevelEncounterConfigData  
→ LevelGoalController  
→ LevelProgressionController / LevelProgressionData  
→ HudController result presentation

Gameplay sync:
- EnemySwitchingManager → GameplayManager.SetActiveEnemyReactionLayer(...)
- LevelEncounterController → GameplayManager encounter values
- LevelEncounterController → LevelGoalController goal setup
- ProjectileBehavior → LevelGoalController hit reporting
- HudController → Retry / Next button delegation into LevelProgressionController

---

## Current Runtime Model

Current model is:
- multiple enemy roots may exist in scene
- only one enemy is active at a time
- one scene may host multiple level encounter configs
- progression decides which encounter is active
- encounter decides which primary goal is active
- HUD result flow decides whether player retries or advances

Important boundary:
- this is NOT a full multi-enemy combat system
- this is NOT a wave spawning system
- this is NOT a roster-driven runtime loading system
- this is a reusable level / encounter / progression / goal system built on top of the validated switching layer

---

## Legacy Status

`LevelEnemyController` is treated as a legacy setup path.

Current rule:
- it must not be used in switching-oriented scenes
- it must not coexist with `LevelEnemySelectionController` in the same scene

Also obsolete:
- using `EnemyVisual` collider as the main gameplay hitbox is no longer valid
- gameplay hit detection must use dedicated body / head colliders

---

## Current Development Position

The project is now ready to move from:

**goal system validation + hitbox repair**

into:

**content expansion + enemy visual identity upgrade + more encounter variety**

Next recommended milestone:
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Additional Goal Types after current baseline stabilization
- Optional Result Panel Polish 1.1
