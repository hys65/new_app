# AI START

You are continuing development of the Unity project **Power Prank 3D**.

Repository:
https://github.com/hys65/new_app

Before doing any design, code, debugging, balancing, or recommendations, you MUST read the project documentation and inspect actual scripts in the repository.

---

## Required Reading Order

Read these files in order:

1. `docs/AI_CONTEXT.md`
2. `docs/PROJECT_STATE.md`
3. `docs/architecture.md`
4. `docs/enemy_system.md`
5. `docs/gameplay_systems.md`
6. `docs/AI_RULES.md`
7. `docs/DEV_WORKFLOW.md`
8. `docs/development_tasks.md`
9. `docs/SESSION_LOG.md`

Also inspect actual scripts in:

- `unity-client/Assets/Scripts/`

Do not rely only on docs.
Do not assume docs are fully up to date unless repository scripts and runtime content support them.

If docs and repository state differ, trust the repository and explain the drift clearly.

---

## Current Project Status Summary

The following milestones are completed and runtime-validated:

- Core throw / hit / breakdown gameplay loop
- Enemy Reaction Layer 1.0
- Enemy Defense Visual Layer 1.0
- Enemy Archetype System
- Enemy AI Layer 1.0
- Enemy Switching System 1.0
- Enemy Roster / Level Enemy Selection 1.0
- Level Content / Encounter Configuration 1.0
- Level Progression / Multi-Level Content 1.0
- Runtime Level Advance 1.0
- Victory Choice Flow 1.0
- Result Panel Polish 1.0
- Level Goal Variety 1.0
- Enemy gameplay hitbox structure repair
- Enemy stain attachment repair
- Goal HUD Readability 1.0
- Boss Preset Override Debugging Pass

The project is no longer at the single-enemy prototype stage.
It already supports reusable multi-level runtime content in one scene.

---

## Current Goal System Status

Implemented and validated goal types:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`
- `UnblockedHitStreak`

Validated teaching block:

- Level 01 -> `BreakdownTarget`
- Level 02 -> `HeadHitCount`
- Level 03 -> `SpecificItemHitCount(item_egg)`

Validated boss-reference block:

- Level 04 -> Meeting Tyrant briefcase boss
- Level 05 -> Narcissist Manager sunglasses boss
- Level 06 -> Meeting Tyrant weak-window boss
- Level 07 -> Narcissist Manager precision paint boss
- Level 08 -> Zero-Mistake Boss
- Level 09 -> Narcissist Manager Face Guard Boss

---

## Current Enemy Architecture

Enemy behavior is fully data-driven.

Behavior definition stack:

- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`

Combined through:

- `EnemyPresetData`

Applied only through:

- `EnemyPresetApplicator`

This is a hard rule.

Do not bypass `EnemyPresetApplicator`.
Do not create parallel preset injection paths.

---

## Current Runtime Flow

Validated runtime stack:

Data
→ `EnemyPresetData`
→ `EnemyPresetApplicator`
→ `EnemyRuntimePresetController`
→ `EnemySwitchingManager`
→ `LevelEnemySelectionController` / `LevelEnemySelectionData`
→ `LevelEncounterController` / `LevelEncounterConfigData`
→ `LevelProgressionController` / `LevelProgressionData`
→ `HudController`

Gameplay sync:

- `EnemySwitchingManager` updates the active enemy reaction layer into `GameplayManager`
- `LevelEncounterController` applies target / time / enemy content into runtime systems
- `LevelProgressionController` owns level flow
- `HudController` presents result UI and delegates Retry / Next into `LevelProgressionController`

---

## Current Content Boundaries

Teaching levels:

- Levels 01–03 remain the tutorial block

Validated boss-reference levels:

- Level 04 = Meeting Tyrant briefcase boss
- Level 05 = Narcissist Manager sunglasses boss
- Level 06 = Meeting Tyrant weak-window boss
- Level 07 = Narcissist Manager precision paint boss
- Level 08 = Zero-Mistake Boss
- Level 09 = Narcissist Manager Face Guard Boss

Important rule:

Post-tutorial content must not regress into fake repetition.
Level 10 and beyond should continue boss-identity-driven expansion.

---

## Level 09 Finalized Direction

Level 09 is implemented as:

**Narcissist Manager – Face Guard Boss**

Core rule:

- head hits are long-term low-value
- body hits are the primary reliable scoring route
- the level is built around hit-zone judgment, not item restriction
- current runtime version is accepted as playable and can be closed

Final Level 09 configuration:

- Goal Type = `BreakdownTarget`
- Target Breakdown = `180`
- Round Duration Seconds = `34`

Important production note:

- enemy-head stain visuals remain imperfect on the current sphere-head setup
- this is accepted for now
- do not reopen stain polish unless it becomes a blocker

---

## Important Current Rules

These rules are mandatory:

1. Do not guess code structure
2. Do not rewrite systems before inspecting repository code
3. Keep behavior data-driven
4. `EnemyPresetApplicator` must remain the only preset injection layer
5. Do not rely on scene-only defense pattern edits when runtime preset application exists
6. Boss content must be authored through:
   - pattern
   - defense state window profile
   - preset
   - roster entry
   - level enemy selection
   - runtime slot routing
7. `LevelEncounterController` owns single-encounter application only
8. `LevelProgressionController` owns multi-level flow only
9. `HudController` may present result UI, but must not own progression logic
10. `GameplayManager` owns round state, not result flow execution
11. `LevelEnemyController` is legacy and must not coexist with `LevelEnemySelectionController` in the same switching-oriented scene

---

## Current Development Position

The project has already moved beyond:

- single enemy prototype validation
- scene-only encounter setup
- forced auto-advance after victory
- Level 08 as a future design target
- Level 09 as a future design target

The project is currently at:

- reusable multi-level runtime flow
- validated four-goal system
- validated six-boss-reference ladder
- content authoring expansion stage

---

## Next Recommended Milestone

**Combat Pacing / Per-Item Throw Cooldown Pass**

High-level direction:

- add a cooldown to each weapon independently
- prevent unrealistically high spam throw rate
- make future boss balancing trustworthy
- do not patch this as a single global cooldown unless code inspection proves that is the intended architecture

This should be treated as a high-priority gameplay pacing task before large-scale future boss balancing.

---

## Required Working Style

When answering:

1. Read docs first
2. Inspect real scripts second
3. Respect current architecture
4. Prefer extending cleanly over rewriting
5. Give exact change points
6. If code changes are needed, provide full direct replacement files when safer than partial patches
7. If scene setup is needed, give exact hierarchy and inspector instructions
8. If repository state and prior summary conflict, trust the repository and explain the mismatch clearly
