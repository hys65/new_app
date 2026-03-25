# AI CONTEXT

## Project Identity

Project name: **Power Prank 3D**

Project type:

- Unity 6.3 LTS small-scale prototype
- single-scene gameplay prototype evolving into multi-level content flow
- third-person fixed-camera prank-throwing game

Core fantasy:

- throw prank items at expressive enemy bosses
- build breakdown pressure
- trigger defense and reaction states
- read boss identity
- use the right item or timing
- clear goal-driven encounters

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
- encounter-driven target / time / enemy authoring
- progression-driven multi-level runtime flow
- player-facing result UI with Retry / Next flow
- goal-aware HUD text
- three distinct goal types
- three distinct boss-reference levels

The current project stage is:

**content-capable runtime architecture with validated boss-reference content**

---

## Current Completed Systems

### Gameplay

- `ThrowController`
- `ProjectileBehavior`
- `GameplayManager`
- hit popup feedback
- stain placement
- HUD current / target / timer / selected item display
- combo display
- round finish handling

### Enemy Runtime

- `EnemyReactionLayerController`
- `EnemyDefenseController`
- `EnemyDefenseVisualLayerController`
- `EnemyDefenseStateWindowController`
- `EnemyAiLayerController`
- `EnemyVisualProxyController`

### Enemy Data

- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`
- `EnemyPresetData`

### Enemy Switching / Selection

- `EnemyPresetApplicator`
- `EnemyRuntimePresetController`
- `EnemySwitchingManager`
- `EnemyRosterData`
- `LevelEnemySelectionData`
- `LevelEnemySelectionController`

### Encounter / Progression

- `LevelEncounterConfigData`
- `LevelEncounterController`
- `LevelProgressionData`
- `LevelProgressionController`

### UI / Result Flow

- `HudController`
- localized Retry / Next labels
- localized result subtitle / level info / goal summary / final-level notice
- result panel hidden-at-start and shown-on-finish behavior

---

## Current Content Status

Implemented enemy archetypes:

1. Meeting Tyrant
2. Narcissist Manager

Validated teaching levels:

- Level 01 -> `BreakdownTarget`
- Level 02 -> `HeadHitCount`
- Level 03 -> `SpecificItemHitCount(item_egg)`

Validated boss-reference levels:

- Level 04 -> Meeting Tyrant briefcase boss
- Level 05 -> Narcissist Manager sunglasses boss
- Level 06 -> Meeting Tyrant weak-window boss

Current production note:

- Levels 01–03 are the teaching block
- Levels 04–06 are the current validated boss-reference block
- Levels 07–09 still exist in progression and should continue boss-first expansion rather than fake repetition

---

## Current Runtime Model

The current runtime model is:

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

### Runtime Truth Rule

- do not treat scene inspector references as final truth when runtime preset application is active
- runtime preset application can overwrite defense pattern and defense state window references
- if runtime behavior looks wrong, inspect:
  1. level enemy selection
  2. roster entry
  3. recommended slot routing
  4. runtime preset controller
  5. preset applicator
  6. active runtime enemy state during Play

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
- localization CSV contains required keys
- no placeholder TMP text remains after inspector cleanup

Current note:

- functionally complete
- prototype-acceptable visually
- not final production polish

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

### Goal-aware HUD

Validated:

- BreakdownTarget shows breakdown-oriented HUD text
- HeadHitCount shows head-hit progress
- SpecificItemHitCount shows item-specific progress text

### Boss Content

Validated:

- Level 04 briefcase boss = hammer break rule
- Level 05 sunglasses boss = foam break / paint finish rule
- Level 06 weak-window boss = mostly defended / briefly vulnerable timing rule

---

## Current Recommended Next Step

Preferred next milestone:

**Level 07 Boss Identity design and implementation**

Reason:

- core loop is validated
- multi-level runtime flow is validated
- result presentation is good enough for prototype use
- three-goal system is already sufficient
- the best leverage is now richer boss content, not architecture churn

Secondary follow-up directions:

- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Result Panel Polish 1.1
- HUD suppression while result panel is active

---

## Working Expectations For Future AI Sessions

Any future AI working on this project must:

1. inspect repository code, not just old summaries
2. preserve Levels 04–06 as reference implementations unless real regression is proven
3. keep behavior data-driven
4. use the preset-authoritative runtime path for boss authoring
5. explain doc drift clearly if docs and runtime content diverge
6. avoid fake repetition in post-tutorial content
