# DEVELOPMENT TASKS

## Current Position

Enemy AI Layer 1.0 is complete.  
Enemy Switching System 1.0 is complete.

The project is no longer blocked by core enemy architecture.

Current priority is to move from:

- system validation

to:

- scalable enemy content flow

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

### Enemy Switching System 1.0 completed scope

- Added EnemyRuntimePresetController
- Added EnemySwitchingManager
- Added GameplayManager runtime active reaction target switching
- Validated preset switching on same enemy object
- Validated switching between two real enemy objects
- Validated single active enemy workflow
- Validated inspector slot setup flow

---

## Next Recommended Tasks

### 1. Enemy Roster / Level Enemy Selection 1.0
Goal:
Move enemy selection from manual scene test setup into reusable level content flow.

Suggested scope:
- define reusable enemy roster data
- bind enemy selection per level
- choose startup enemy from level config
- reduce manual test-scene-only setup

Expected result:
- levels can declare which enemy or preset should appear
- startup active enemy becomes content-driven
- future enemy expansion becomes cleaner

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

Expected result:
- faster validation of enemy data tuning
- less inspector-only testing overhead

---

### 4. Level Integration Cleanup
Goal:
Reduce overlap between older setup path and new switching path.

Suggested scope:
- review LevelEnemyController usage
- keep legacy compatibility where needed
- avoid running LevelEnemyController and EnemySwitchingManager as competing setup paths in the same scene

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

---

## Recommended Immediate Next Milestone

**Enemy Roster / Level Enemy Selection 1.0**