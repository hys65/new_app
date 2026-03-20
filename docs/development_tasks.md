# DEVELOPMENT TASKS

## Current Position

Enemy AI Layer 1.0 is complete.  
Enemy Switching System 1.0 is complete.  
Enemy Roster / Level Enemy Selection 1.0 is complete.  
Level Content / Encounter Configuration 1.0 is complete.  
Level Progression / Multi-Level Content 1.0 is complete.  
Runtime Level Advance 1.0 is complete.  
Victory Choice Flow 1.0 is complete.

The project is no longer blocked by core enemy or level-flow architecture.

Current priority has moved from:
- system validation

to:
- playable content scaling
- UI polish
- reusable level authoring
- progression-ready content expansion

---

## Completed

- Core throw / hit / breakdown gameplay loop ✅
- Enemy Reaction Layer 1.0 ✅
- Enemy Defense Visual Layer 1.0 ✅
- Enemy Archetype System ✅
- Enemy AI Layer 1.0 ✅
- Enemy Preset integration ✅
- Meeting Tyrant preset ✅
- Narcissist Manager preset ✅
- Enemy Switching System 1.0 ✅
- Enemy Roster / Level Enemy Selection 1.0 ✅
- Level Content / Encounter Configuration 1.0 ✅
- Level Progression / Multi-Level Content 1.0 ✅
- Runtime Level Advance 1.0 ✅
- Victory Choice Flow 1.0 ✅

### Enemy Switching System 1.0 completed scope
- Added EnemyRuntimePresetController
- Added EnemySwitchingManager
- Added GameplayManager runtime active reaction target switching
- Validated preset switching on same enemy object
- Validated switching between two real enemy objects
- Validated single active enemy workflow
- Validated inspector slot setup flow

### Enemy Roster / Level Enemy Selection 1.0 completed scope
- Added EnemyRosterData
- Added LevelEnemySelectionData
- Added LevelEnemySelectionController
- Extended EnemySwitchingManager startup configuration support
- Validated reusable roster asset workflow
- Validated level-driven startup enemy selection
- Validated `startupSelectionIndex = 0`
- Validated `startupSelectionIndex = 1`
- Removed legacy `LevelEnemyController` scene hookup from current switching scene
- Confirmed clean single startup preset application after removing old competing path

### Level Content / Encounter Configuration 1.0 completed scope
- Added LevelEncounterConfigData
- Added LevelEncounterController
- Bound LevelEnemySelectionData into encounter config
- Added per-level target breakdown values
- Added per-level time limit values
- Reduced manual scene-only gameplay setup
- Validated Level 01 / Level 02 / Level 03 encounter configs

### Level Progression / Multi-Level Content 1.0 completed scope
- Added LevelProgressionData
- Added LevelProgressionController
- Added startup level index flow
- Added ordered multi-level encounter asset flow
- Validated single-scene multi-level startup application
- Validated runtime application of Level 01 / Level 02 / Level 03

### Runtime Level Advance 1.0 completed scope
- Added runtime next-level progression
- Added runtime current-level restart
- Added runtime enemy reselection during level change
- Added runtime target / timer refresh during level change
- Fixed transition timing contamination with delayed progression
- Fixed drag carry-over by resetting ThrowController transient state

### Victory Choice Flow 1.0 completed scope
- Disabled mandatory auto-advance for player-facing progression
- Added result panel Retry / Next behavior
- Added next-level availability check
- Retry now restarts current level through progression flow
- Next now advances level through progression flow
- Final level hides next-level option

---

## Next Recommended Tasks

### 1. Result Panel Polish 1.0
Goal:
Upgrade the currently functional result panel into a cleaner, test-ready gameplay UI flow.

Suggested scope:
- improve result panel hierarchy
- clean button placement
- add proper localized labels for Retry / Next / Victory / Failed
- improve spacing and readability
- ensure final-level behavior is clean and readable
- remove temporary debug-looking UI presentation

Expected result:
- clearer result panel
- stronger player-facing progression choice
- no regression in Retry / Next logic

---

### 2. Level Goal Variety 1.0
Goal:
Move beyond pure breakdown-target-only level design.

Suggested scope:
- head-hit goals
- block-count goals
- item-specific goals
- mixed encounter goals
- more authored gameplay variety per level

Expected result:
- levels feel more distinct
- progression becomes more than target/time tuning

---

### 3. Content Expansion toward 12 Levels
Goal:
Scale the validated level pipeline into actual game content.

Suggested scope:
- author more LevelEnemySelectionData assets
- author more LevelEncounterConfigData assets
- build ordered full progression list
- tune enemy / target / timer curve across levels
- reuse current single-scene runtime architecture

Expected result:
- progression becomes real game content, not only system validation

---

### 4. Enemy Visual Identity Upgrade
Goal:
Improve readability and personality of enemy presentation.

Suggested scope:
- replace whitebox proxy visuals
- preserve current enemy root / collider / controller structure
- differentiate Meeting Tyrant and Narcissist Manager more clearly
- keep all behavior data-driven

Expected result:
- stronger character readability
- same validated enemy system, better presentation

---

## Important Constraints

Must remain true:

1. Do not break data-driven enemy setup
2. EnemyPresetApplicator must remain the only preset injection layer
3. EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
4. AI controls defense timing, not the window system
5. Prefer data tuning over unnecessary script rewrites
6. Do not collapse scene orchestration logic into AI logic
7. Do not allow legacy LevelEnemyController to compete with switching / encounter / progression startup flow
8. HudController should present result flow, not own level orchestration
9. LevelProgressionController should own next / restart flow, not HUD