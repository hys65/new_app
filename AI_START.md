# AI START

You are continuing development of the Unity project **Power Prank 3D**.

Repository:
https://github.com/hys65/new_app

Before doing any design, code, debugging, or recommendations, you MUST read the project documentation and inspect actual scripts in the repository.

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

The following milestones are completed and validated:

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
- Goal-aware HUD Readability 1.0
- Boss Preset Override Debugging Pass
- Level 04 Briefcase Boss Foundation
- Level 05 Sunglasses Boss Foundation
- Level 06 Weak-Window Boss Foundation

The project is no longer at the single-enemy prototype stage.
It already supports reusable multi-level runtime content in one scene.

---

## Current Goal System Status

Implemented and validated goal types:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`

Validated teaching block:

- Level 01 -> `BreakdownTarget`
- Level 02 -> `HeadHitCount`
- Level 03 -> `SpecificItemHitCount(item_egg)`

Validated boss-reference block:

- Level 04 -> `BreakdownTarget` + Meeting Tyrant briefcase boss rule
- Level 05 -> `SpecificItemHitCount(item_paint_ball)` + Narcissist Manager sunglasses boss rule
- Level 06 -> `HeadHitCount` + Meeting Tyrant weak-window boss rule

These three goal types are sufficient for the current repository content.

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

Important rule:

Post-tutorial content must not regress into fake repetition.
Level 07 and beyond should continue boss-identity-driven expansion.

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
- Level 06 as a future design target

The project is currently at:

- reusable multi-level runtime flow
- validated three-goal system
- validated three-boss-reference block
- content authoring expansion stage

---

## Next Recommended Milestone

**Level 07 Boss Identity design and implementation**

High-level direction:

- preserve Levels 04–06 as validated reference implementations
- do not redesign finished boss levels unless repository inspection proves a real regression
- continue expanding boss identity through minimal clean extensions
- do not introduce unnecessary architecture churn

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
