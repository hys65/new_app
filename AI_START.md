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
- Combat Pacing / Per-Item Throw Cooldown Pass
- Repository asset-structure cleanup
- Level 10 boss identity closure
- Level 11 boss identity closure

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
- Level 10 -> Adaptive Shutdown Boss
- Level 11 -> Head Hunter Boss

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
- Level 10 = Adaptive Shutdown Boss
- Level 11 = Head Hunter Boss

Important rule:

Post-tutorial content must not regress into fake repetition.

---

## Current Level 11 Closure Meaning

Level 11 is now accepted as a distinct boss identity.

Identity summary:

- the player is pushed toward late-window precision
- the player must wait for a later scoring opportunity rather than simply spam safe rhythm
- the encounter pressure is not primarily about item restriction
- the encounter pressure is not primarily about body-vs-head judgment in the Level 09 sense
- the encounter pressure is not primarily about anti-predictability in the Level 10 sense

Level 11 establishes:

- a late-window head-focused boss demand
- a new boss identity achieved through current systems
- no new goal type was required
- no new architecture branch was required

---

## Current Production Direction

Do not reopen Level 10 or Level 11 casually if they already feel correct in runtime.

Current next milestone:

- combat readability / boss presentation pass
- strengthen telegraph / active / weak / block / break / success readability using the current systems
- continue content cleanup and presentation polish before final release balancing

Important production rule:

Do not perform final 04–11 release balancing yet.
Final full-ladder balancing should happen later, after more presentation / art-side readability work is in place.
