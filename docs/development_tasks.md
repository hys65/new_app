# DEVELOPMENT TASKS

## Current Position

Enemy AI Layer 1.0 is complete.  
Enemy Switching System 1.0 is complete.  
Enemy Roster / Level Enemy Selection 1.0 is complete.

The project is no longer blocked by core enemy architecture.

Current priority is to move from:
- system validation

to:
- scalable content flow
- reusable level authoring

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

---

## Next Recommended Tasks

### 1. Level Content / Encounter Configuration 1.0
Goal:
Move from enemy-only startup selection into full level encounter content authoring.

Suggested scope:
- define per-level gameplay target values
- define per-level time limit
- bind startup enemy selection together with level gameplay settings
- prepare cleaner reusable level config assets

Expected result:
- a level defines both enemy and gameplay goals
- scene setup becomes less inspector-manual
- future level expansion becomes cleaner

---

### 2. Enemy Content Expansion 1.0
Goal:
Scale from two test enemies to multiple playable enemies.

Suggested scope:
- add more enemy presets
- add more archetype variations
- expand defense timing diversity
- expand weakness profiles
- increase visual personality separation

Expected result:
- enemy switching becomes meaningful game content instead of only a debug feature

---

### 3. Runtime Debug Switching UI
Goal:
Improve testing efficiency.

Suggested scope:
- previous enemy button
- next enemy button
- current enemy label
- current preset label
- startup enemy label

Expected result:
- faster validation of enemy data tuning
- less inspector-only testing overhead

---

### 4. Legacy Scene Cleanup
Goal:
Prevent old setup paths from re-entering validated switching scenes.

Suggested scope:
- review LevelEnemyController usage
- keep legacy compatibility only where explicitly needed
- avoid running LevelEnemyController and EnemySwitchingManager as competing setup paths in the same scene
- document startup ownership rules clearly

Expected result:
- cleaner scene setup
- fewer configuration mistakes
- easier onboarding in future sessions

---

## Important Constraints

Must remain true:

1. Do not break data-driven enemy setup
2. EnemyPresetApplicator must remain the only preset injection layer
3. EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
4. AI controls defense timing, not the window system
5. Prefer data tuning over script rewrites
6. Do not collapse scene orchestration logic into AI logic
7. Do not allow legacy LevelEnemyController to compete with level-driven switching flow in the same scene

---

## Recommended Immediate Next Milestone

**Level Content / Encounter Configuration 1.0**