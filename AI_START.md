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
Do not assume docs are fully up to date unless repository scripts and scene wiring support them.

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

The current project is no longer at the “single enemy prototype only” stage.

It already supports:
- multiple enemy roots in one scene
- only one active enemy at a time
- roster-driven level enemy selection
- encounter config driven target/time/enemy setup
- progression-driven multi-level flow in one scene
- player-facing result panel with Retry / Next flow
- localized result panel labels and supporting text

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

## Implemented Enemy Archetypes

### Meeting Tyrant
- Early defense trigger
- Strong guard
- Short recover
- Stable and hard to break

### Narcissist Manager
- Late defense trigger
- Long telegraph
- Short guard
- Long recover
- High head weakness
- Easy to break

Both are validated and intentionally different.

---

## Current Runtime Flow

Current validated stack:

Data  
→ `EnemyPresetData`  
→ `EnemyPresetApplicator`  
→ `EnemyRuntimePresetController`  
→ `EnemySwitchingManager`  
→ `LevelEnemySelectionController` / `LevelEnemySelectionData`  
→ `LevelEncounterController` / `LevelEncounterConfigData`  
→ `LevelProgressionController` / `LevelProgressionData`  
→ `HudController` result presentation

Gameplay sync:
- `EnemySwitchingManager` updates active enemy reaction layer into `GameplayManager`
- `LevelEncounterController` applies target/time/enemy content into runtime systems
- `LevelProgressionController` owns level flow
- `HudController` presents result UI and delegates Retry / Next into `LevelProgressionController`

---

## Current Result Panel Status

Result Panel Polish 1.0 is completed.

Validated structure:

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

Validated behavior:
- result panel hidden at startup
- result panel shown after round finish
- localized Victory / Failed / Retry / Next / subtitle / goal text
- Retry restarts current level
- Next advances only if another level exists
- final-level notice is supported
- no raw localization keys remain after CSV update
- no TMP placeholder `New Text` remains after proper binding

---

## Important Current Boundaries

These rules are mandatory:

1. Do not guess code structure
2. Do not rewrite systems before inspecting repository code
3. Keep all behavior data-driven
4. `EnemyPresetApplicator` must remain the only preset injection layer
5. `EnemyDefenseStateWindowProfile.autoCycle` must remain FALSE
6. AI controls defense timing, not the defense window system
7. Do not collapse scene orchestration into enemy AI
8. Do not let competing startup preset setup paths coexist in the same scene
9. `LevelEncounterController` owns single-encounter application only
10. `LevelProgressionController` owns multi-level flow only
11. `HudController` may present result UI, but must not own level progression logic
12. `GameplayManager` owns round state, not result flow execution
13. `LevelEnemyController` is legacy and must not coexist with `LevelEnemySelectionController` in the same switching-oriented scene

---

## Current Development Position

The project has already moved beyond:
- single enemy prototype validation
- defense timing validation
- scene-only manual encounter setup
- forced auto-advance after victory

The project is currently at:
- reusable multi-level runtime flow
- player-facing result UI
- content authoring expansion stage

---

## Next Recommended Milestone

**Level Goal Variety 1.0**

Suggested direction:
- expand beyond pure breakdown-only goals
- add level-specific goal variety
- keep goals data-driven
- extend encounter / progression content without breaking existing result flow

Secondary options:
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Result Panel Polish 1.1

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
8. Do not propose speculative architecture that ignores the repository state

If repository state and prior summary conflict, trust the repository and explain the mismatch clearly.
