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
- Goal-aware HUD readability update
- Level 04 Briefcase Boss foundation
- Level 05 Sunglasses Boss foundation

The project is no longer a single-enemy prototype.
It already supports same-scene multi-level runtime flow and goal-driven encounters.

---

## Current Goal System Status

The following goal types are implemented and validated:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`

Validated example progression:

- Level 1 -> BreakdownTarget
- Level 2 -> HeadHitCount
- Level 3 -> SpecificItemHitCount(item_egg)
- Level 4 -> BreakdownTarget + Briefcase Boss rule
- Level 5 -> SpecificItemHitCount(item_paint_ball) + Sunglasses Boss rule

Levels 4-9 content assets exist in progression, but only Levels 1-5 should currently be treated as meaningfully validated content structure.

---

## Current Enemy Runtime Architecture

Enemy behavior remains data-driven.

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

## Important Runtime Fact

For boss-pattern work, changing `EnemyDefenseController.defensePattern` directly on the scene object is NOT sufficient if runtime preset application is active.

Runtime preset application will overwrite scene-level defense pattern values through:

- `EnemyPresetApplicator.ApplyPreset(...)`

If a boss defense pattern must survive runtime startup, it must be assigned through the actual `EnemyPresetData` used by the runtime-selected roster entry.

---

## Current Level Design Direction

Levels 1-3 are tutorial levels.

From Level 4 onward, level design direction is:

- each level should represent a distinct boss identity
- each boss should have a distinct defense style
- each boss should force different item usage or timing
- avoid fake content expansion through repeated goal rotation only

Do not continue building the project as simple repeated:

- BreakdownTarget
- HeadHitCount
- SpecificItemHitCount

loops with only numeric escalation.

Boss identity now has higher priority than pure goal rotation.

---

## Current Boss Progress

### Level 04
**Briefcase Boss / Hammer Break rule** is completed and validated.

Runtime authoring path:

- `meeting_tyrant_briefcase_boss_defense_pattern`
- `enemy_preset_meeting_tyrant_briefcase_boss`
- `meeting_tyrant_briefcase_boss` roster entry
- `level_enemy_selection_level_04`

Validated rule:

- timed defense activation
- briefcase-style guard state
- sponge hammer breaks defense
- non-hammer items are blocked

### Level 05
**Sunglasses Boss / Foam Break / Paint Finish rule** is completed and validated.

Runtime authoring path:

- `narcissist_manager_sunglasses_boss_defense_pattern`
- `enemy_preset_narcissist_manager_sunglasses_boss`
- `narcissist_manager_sunglasses_boss` roster entry
- `level_enemy_selection_level_05`

Validated rule:

- timed sunglasses defense activation
- paint is ineffective while face guard is active
- foam breaks defense
- paint becomes valid after defense break
- level goal is built around `SpecificItemHitCount(item_paint_ball)`

This is now the second true boss-identity encounter.

---

## Mandatory Current Boundaries

1. Do not guess code structure
2. Inspect real scripts before changing docs or architecture
3. Keep enemy runtime data-driven
4. `EnemyPresetApplicator` must remain the only preset injection layer
5. `LevelEnemyController` remains legacy and must not re-enter the main scene flow
6. `LevelEncounterController` owns encounter application only
7. `LevelProgressionController` owns multi-level flow only
8. `HudController` may display goal/status/result UI but must not own progression logic
9. `GameplayManager` owns round state, not level progression flow
10. Do not regress repaired hitbox structure
11. Do not reintroduce `EnemyVisual` as gameplay collision root
12. Do not attach boss-rule authoring only at scene-object level if preset application will overwrite it

---

## Current Development Position

The project has already moved beyond:

- single-enemy combat validation
- result-panel-only polish
- simple goal variety validation
- first boss-defense proof only

The project is now at:

- tutorial-to-boss structure planning
- multi-boss identity implementation stage
- preset-driven boss configuration stage

---

## Current Recommended Next Milestone

**Level 06 Boss Identity design and implementation**

Suggested direction:

- continue distinct boss identity work after Level 05
- preserve Level 04 and Level 05 as boss-foundation levels
- avoid reverting to generic repeated content
- prefer minimal clean extensions over architecture churn

Secondary follow-up:

- update Levels 4-12 into tutorial + unique boss structure
- add minimal boss-specific goal types only when required
- avoid architecture churn

---

## Required Working Style

When answering:

1. Read docs first
2. Inspect real scripts second
3. Respect current architecture
4. Prefer exact extensions over speculative rewrites
5. If code changes are needed, provide full direct replacement files when safer
6. If scene/data changes are needed, specify exact asset and inspector changes
7. If runtime config is overwritten, trace the preset / roster / selection chain before changing code
8. Distinguish clearly between:
   - scene-level values
   - preset-level values
   - runtime-selected roster entry values
