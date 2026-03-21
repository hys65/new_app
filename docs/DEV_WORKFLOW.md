# DEV WORKFLOW

## Purpose

This document defines the working method for continuing development on **Power Prank 3D**.

The project has already moved beyond early prototype guessing.
Core runtime architecture, enemy switching, level selection, progression flow, and result UI flow are now validated.

So future work must follow a stricter workflow:
- inspect first
- design second
- change minimally
- preserve validated boundaries
- prefer content/data expansion over architecture churn

---

## Current Development Phase

The project is currently in:

**validated runtime architecture + content expansion phase**

This means:
- the project is no longer proving whether the core structure works
- the project is now extending content and polish on top of a working runtime stack

Already validated:
- throw / hit / breakdown loop
- enemy reactions
- enemy defense visual layer
- enemy AI timing
- enemy switching
- level enemy selection
- encounter config application
- multi-level progression
- runtime next / retry flow
- player-facing result UI
- localized result panel support

Because of this, future work must not casually rewrite foundations that are already working.

---

## Required Work Sequence

Every future implementation or debugging task must follow this order:

### 1. Read docs first
Required reading order:
1. `docs/AI_CONTEXT.md`
2. `docs/PROJECT_STATE.md`
3. `docs/architecture.md`
4. `docs/enemy_system.md`
5. `docs/gameplay_systems.md`
6. `docs/AI_RULES.md`
7. `docs/DEV_WORKFLOW.md`
8. `docs/development_tasks.md`
9. `docs/SESSION_LOG.md`

### 2. Inspect real repository code second
Always inspect:
- `unity-client/Assets/Scripts/`

Do not trust summaries alone.
Do not assume prior chat context is accurate if code says otherwise.

### 3. Confirm current ownership boundaries
Before proposing any change, confirm:
- who owns gameplay state
- who owns preset application
- who owns scene-level switching
- who owns encounter application
- who owns progression
- who owns result presentation

### 4. Design change around current architecture
Prefer:
- extending validated systems
- adding content assets
- adding small read-only helper APIs
- improving inspector hookup
- polishing presentation

Avoid:
- rewriting stable orchestration layers
- moving logic across ownership boundaries
- inventing new manager layers unless absolutely necessary

### 5. Data / scene setup before script rewrites
When possible:
- create or bind data assets first
- configure hierarchy / inspector first
- only then change scripts if real code support is missing

This project is intentionally inspector-friendly.
Do not jump to scripts first if the actual problem is scene wiring or asset setup.

### 6. Validate runtime result
After every meaningful change, validate:
- does startup still work
- does result flow still work
- does active enemy still switch correctly
- does current level still restart correctly
- does next level still progress correctly
- does localization still resolve correctly
- do placeholder UI texts stay hidden until intended

### 7. Only then update docs
Docs must be updated after:
- runtime validation
- scene validation
- content validation

Do not document speculative systems as completed.

---

## Source of Truth Rule

When docs, prior chat summaries, and repository code disagree:

**Repository code and actual scene wiring win.**

Use this order of trust:
1. repository scripts
2. current scene inspector state
3. validated runtime result
4. docs
5. prior conversation summary

Reason:
- docs may lag
- conversation memory may reflect local-only work not yet committed
- repository is the only stable source that new sessions can inspect

---

## Preferred Change Style

### Prefer
- exact change points
- small extensions
- full replacement files when safer than fragmented patches
- clear hierarchy instructions
- precise inspector hookup steps
- data-driven additions
- read-only helper APIs when UI needs runtime info

### Avoid
- speculative redesign
- vague “you should refactor everything” advice
- hidden assumptions about missing fields or controllers
- bypassing already validated layers
- introducing convenience shortcuts that duplicate existing architecture

---

## Architecture Preservation Rules

Future work must preserve these rules.

### Preset Rule
- `EnemyPresetApplicator` must remain the only preset injection layer

Do not:
- inject preset data directly into multiple enemy runtime controllers
- create alternative preset application paths
- bypass the applicator for startup convenience

### Defense Timing Rule
- `EnemyDefenseStateWindowProfile.autoCycle` must remain FALSE
- AI controls defense timing
- defense window controller must not become autonomous combat logic

### Scene Startup Rule
- do not allow competing startup preset paths in the same scene
- `LevelEnemyController` is legacy
- `LevelEnemyController` must not coexist with `LevelEnemySelectionController` in the current switching-oriented scene

### Encounter Boundary Rule
- `LevelEncounterController` owns single-encounter application only
- it must not become the progression controller

### Progression Boundary Rule
- `LevelProgressionController` owns multi-level flow only
- it must not become HUD logic
- it must not directly own UI presentation

### HUD Boundary Rule
- `HudController` presents UI
- `HudController` may refresh localized texts
- `HudController` may delegate Retry / Next
- `HudController` must not own level flow execution

### Gameplay Boundary Rule
- `GameplayManager` owns round state
- `GameplayManager` must not become the result button executor
- `GameplayManager` must not absorb level progression orchestration

---

## UI / Result Flow Workflow

Since Result Panel Polish 1.0 is now validated, any future result-panel work must follow this rule:

### Allowed changes
- update hierarchy layout
- improve dimmer/card/button styling
- improve typography
- hide or reduce normal HUD while result panel is active
- add localized support text
- add read-only displayed info sourced from progression/gameplay state

### Not allowed
- move Retry / Next execution into HUD internals beyond delegation
- move result ownership into GameplayManager
- create a separate orchestration manager for result flow unless the existing structure clearly fails

Current validated result structure:
- ResultPanel
- Dimmer
- SafeArea
- ResultCard
- Header
  - ResultTitleText
  - ResultSubtitleText
- Body
  - LevelInfoText
  - GoalSummaryText
  - FinalLevelNoticeText
- Actions
  - RetryButton
  - NextLevelButton

---

## Content Expansion Workflow

The project is now ready for more content.
Content expansion should follow this order:

### 1. Reuse validated runtime systems
Use:
- existing enemy roster
- existing encounter config structure
- existing progression structure

### 2. Add new content assets first
Examples:
- new `LevelEncounterConfigData`
- new `LevelEnemySelectionData`
- new progression entries
- new localization keys if needed

### 3. Tune data before touching code
Prefer:
- changing target breakdown
- changing time limit
- changing enemy archetype usage
- changing progression order
- changing result text content

before:
- changing runtime manager code

### 4. Validate in-scene progression
When adding levels:
- test startup index
- test Retry
- test Next
- test final-level behavior
- test result text correctness

---

## Debugging Workflow

When something breaks, debug in this order:

### 1. Compiler errors
Fix all hard compile errors first.

### 2. Scene hookup
Check:
- inspector references
- hierarchy object names
- hidden placeholder TMP text
- missing buttons / text field hookups

### 3. Localization source
Check:
- `Assets/Localization/localization_table.csv`
- missing keys
- raw key fallback showing in UI

### 4. Runtime ownership
Check whether the bug is actually a boundary violation:
- gameplay bug
- progression bug
- HUD presentation bug
- scene switching bug
- localization data bug

### 5. Only then consider architecture changes
Do not redesign because of a missing reference or CSV key.

---

## Documentation Workflow

Update docs after each completed and runtime-validated milestone.

At minimum update:
- `docs/PROJECT_STATE.md`
- `docs/architecture.md`
- `docs/gameplay_systems.md`
- `docs/development_tasks.md`
- `docs/SESSION_LOG.md`

Update these when milestone framing changes:
- `AI_START.md`
- `docs/AI_CONTEXT.md`

Do not mark a milestone complete unless:
- runtime behavior is confirmed
- scene wiring is confirmed
- docs reflect the actual validated state

---

## Current Recommended Next Milestones

Preferred next milestone:
- **Level Goal Variety 1.0**

Strong follow-up candidates:
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Result Panel Polish 1.1
- HUD suppression while result panel is active

---

## Final Rule

Future development must optimize for:

**clarity, reuse, validation, and clean extension**

not for:
- clever rewrites
- architecture churn
- speculative abstraction
- manager proliferation

This project already has a working spine.
Protect it.