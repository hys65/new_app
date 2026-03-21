# AI CONTEXT

## Project Identity

Project name:
**Power Prank 3D**

Project type:
- Unity 6.3 LTS small-scale prototype
- single-scene gameplay prototype evolving into multi-level content flow
- third-person fixed-camera prank-throwing game

Core fantasy:
- throw prank items at expressive enemy characters
- build breakdown pressure
- trigger defense and reaction states
- create readable and funny character-specific behavior

---

## Current Stage

The project is no longer just validating combat feel.

It has already validated:
- core throw / hit / breakdown gameplay loop
- enemy reaction behavior
- enemy defense presentation
- archetype-driven enemy behavior
- AI-driven defense timing
- same-scene enemy switching
- roster-driven level enemy selection
- encounter-driven target/time/enemy authoring
- progression-driven multi-level runtime flow
- player-facing result UI with Retry / Next flow
- localized result panel support

The current project stage is:

**content-capable runtime architecture with prototype-ready result presentation**

---

## Current Completed Systems

### Gameplay
- ThrowController
- Projectile hit flow
- GameplayManager breakdown loop
- Hit popup feedback
- HUD current/target/time/item display
- combo display
- round finish handling

### Enemy Runtime
- EnemyReactionLayerController
- EnemyDefenseController
- EnemyDefenseVisualLayerController
- EnemyDefenseStateWindowController
- EnemyAiLayerController
- EnemyVisualProxyController

### Enemy Data
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- EnemyPresetData

### Enemy Switching / Selection
- EnemyPresetApplicator
- EnemyRuntimePresetController
- EnemySwitchingManager
- EnemyRosterData
- LevelEnemySelectionData
- LevelEnemySelectionController

### Encounter / Progression
- LevelEncounterConfigData
- LevelEncounterController
- LevelProgressionData
- LevelProgressionController

### UI / Result Flow
- HudController
- localized Retry / Next labels
- localized result subtitle / level info / goal summary / final-level notice
- result panel hierarchy cleanup
- result panel hidden-at-start and shown-on-finish behavior

---

## Current Content Status

Implemented enemy archetypes:
1. Meeting Tyrant
2. Narcissist Manager

Implemented encounter content:
- `level_01_encounter_config`
- `level_02_encounter_config`
- `level_03_encounter_config`

Implemented selection content:
- `level_enemy_selection_meeting_tyrant`
- `level_enemy_selection_narcissist_manager`

Implemented progression content:
- `main_level_progression_data`

Current prototype result panel localization keys now include:
- `ui_retry`
- `ui_next_level`
- `result_ready_for_next`
- `result_all_levels_complete`
- `result_try_again`
- `ui_level`
- `ui_goal_progress`
- `ui_final_level_cleared`

---

## Current Runtime Model

Current runtime model is:

- multiple enemy roots may exist in the scene
- only one enemy is active at a time
- enemy switching is scene orchestration, not AI ownership
- AI remains per-enemy
- one scene may host multiple encounter configs
- progression decides which encounter is active
- player result UI decides whether to retry or advance
- HUD presents result choices
- progression executes level changes

This is not:
- a wave spawn system
- a full multi-enemy combat system
- a runtime-loaded roster content pipeline
- a procedural encounter generator

It is:
- a reusable single-scene multi-level prototype architecture

---

## Mandatory Architecture Rules

### Preset Application
- `EnemyPresetApplicator` must remain the only preset injection layer
- do not inject preset data directly into multiple enemy runtime controllers
- do not create alternate preset setup shortcuts

### Defense Timing
- `EnemyDefenseStateWindowProfile.autoCycle` must remain FALSE
- AI controls defense timing
- defense window system does not self-run combat behavior

### Scene Startup
- do not allow competing startup preset paths in the same scene
- `LevelEnemyController` is legacy
- `LevelEnemyController` must not coexist with `LevelEnemySelectionController` in the current switching-oriented scene

### Encounter / Progression Boundaries
- `LevelEncounterController` owns single-encounter application only
- `LevelProgressionController` owns multi-level flow only
- do not merge these responsibilities

### HUD / Result Flow Boundaries
- `HudController` presents result UI
- `HudController` may localize and refresh result texts
- `HudController` must not own level progression logic
- `LevelProgressionController` must execute Retry / Next
- `GameplayManager` owns round state
- `GameplayManager` must not become the result-flow executor

---

## Result Panel Polish 1.0 Context

Result Panel Polish 1.0 is complete.

Purpose:
- replace debug-looking result presentation with a cleaner prototype UI
- preserve existing Retry / Next flow ownership
- localize result labels and support texts
- keep result panel hidden before round finish

Implemented result hierarchy:
- `ResultPanel`
- `Dimmer`
- `SafeArea`
- `ResultCard`
- `Header`
  - `ResultTitleText`
  - `ResultSubtitleText`
- `Body`
  - `LevelInfoText`
  - `GoalSummaryText`
  - `FinalLevelNoticeText`
- `Actions`
  - `RetryButton`
    - `RetryButtonText`
  - `NextLevelButton`
    - `NextLevelButtonText`

Validation results:
- startup scene does not show result panel
- victory shows localized title and subtitle
- level info and goal progress text display correctly
- Retry and Next labels localize correctly
- Retry and Next remain functional
- localization CSV now contains required keys
- no placeholder TMP text remains after inspector cleanup

Current note:
- Result Panel Polish 1.0 is functionally complete
- visual styling is prototype-acceptable, not final production polish

---

## Known Validated Behaviors

### Enemy Switching
Validated:
- same enemy object runtime preset switching
- switching between multiple scene enemy roots
- one active enemy at runtime
- clean startup active slot selection

### Level Selection / Encounter Application
Validated:
- enemy selection bound through encounter config
- target breakdown refresh per level
- timer refresh per level
- startup level index application
- runtime encounter reapplication during progression

### Runtime Level Flow
Validated:
- next level progression
- current level restart
- enemy reselection on progression
- drag/input contamination fix after round end
- delayed progression transition to avoid same-frame carry-over

### Result Flow
Validated:
- result panel appears after round finish
- Retry restarts current level
- Next advances when another level exists
- final level hides next button and supports final notice
- result panel text resolves through localization CSV

---

## Current Recommended Next Step

Preferred next milestone:
**Level Goal Variety 1.0**

Reason:
- core loop is validated
- multi-level runtime flow is validated
- result presentation is now good enough for prototype use
- the best leverage is now richer level goals, not further architecture churn

Secondary follow-up directions:
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Result Panel Polish 1.1
- HUD suppression while result panel is active

---

## Working Expectations For Future AI Sessions

Any future AI working on this project must:

1. Read documentation first
2. Inspect actual scripts second
3. Respect the current data-driven architecture
4. Avoid speculative rewrites
5. Preserve validated ownership boundaries
6. Prefer exact modifications over broad redesign
7. Keep scene instructions precise and inspector-friendly
8. Distinguish clearly between:
   - validated repo state
   - local scene wiring mistakes
   - optional polish improvements

If a mismatch appears between prior conversation summary and current repository code, the repository must be inspected and treated as source of truth.
