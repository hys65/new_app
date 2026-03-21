# GAMEPLAY SYSTEMS

## Gameplay Loop

Power Prank 3D currently runs on a single-scene, multi-level gameplay loop.

Core loop:
1. Start encounter
2. Throw prank item
3. Register hit
4. Apply breakdown pressure
5. Trigger enemy reactions / defense behavior
6. Reach target or run out of time
7. Show result panel
8. Player chooses Retry or Next
9. Restart current level or advance to next level

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

Important boundary:
- GameplayManager owns gameplay state
- it does not own level progression
- it does not own result button execution logic

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

## Current Multi-Level Runtime Flow

Encounter application flow:
LevelProgressionController  
→ LevelEncounterController  
→ GameplayManager target/time refresh  
→ LevelEnemySelectionController  
→ EnemySwitchingManager  
→ active enemy slot selected

Result flow:
GameplayManager.OnRoundFinished  
→ HudController.ShowResultPanel  
→ Retry / Next button click  
→ LevelProgressionController.RestartCurrentLevel / AdvanceToNextLevel

---

## Current Gameplay Boundaries

Do:
- keep gameplay state in GameplayManager
- keep level flow in LevelProgressionController
- keep encounter application in LevelEncounterController
- keep enemy startup selection in LevelEnemySelectionController
- keep result presentation in HudController

Do not:
- move level progression into HUD
- move result UI ownership into GameplayManager
- bypass LevelEncounterController for runtime level application
- bypass EnemyPresetApplicator for preset injection
- enable defense window auto-cycle