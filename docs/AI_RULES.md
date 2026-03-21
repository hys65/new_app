# AI RULES

## Purpose

This document defines mandatory behavioral rules for any AI continuing development on **Power Prank 3D**.

The project already has a validated runtime architecture.
AI must help extend it, not destabilize it.

---

## Core Mandatory Rules

### 1. Inspect before proposing
Before suggesting architecture, code changes, scene fixes, or debugging steps, AI must:
1. read project docs in the required order
2. inspect actual repository scripts
3. compare docs against code when necessary

Do not guess code structure.
Do not infer missing systems without checking.

### 2. Repository reality beats summary
If docs, prior summaries, screenshots, or user memory conflict with repository code:

**repository code and validated scene wiring are the source of truth**

AI must explicitly call out the mismatch instead of silently assuming both are correct.

### 3. Preserve validated architecture
This project already has a working spine.

AI must preserve:
- data-driven enemy behavior
- single preset injection layer
- scene-level enemy switching
- content-driven level enemy selection
- encounter-driven gameplay setup
- progression-driven multi-level flow
- HUD-presented result panel with delegated Retry / Next flow

Do not destabilize working layers for cosmetic convenience.

---

## Hard Architecture Boundaries

### Preset Injection Boundary
`EnemyPresetApplicator` must remain the only preset injection layer.

AI must not propose:
- direct preset injection into multiple runtime controllers
- shortcut preset application paths
- parallel startup preset assignment logic that bypasses the applicator

### Defense Timing Boundary
`EnemyDefenseStateWindowProfile.autoCycle` must remain FALSE.

AI must preserve:
- AI controls defense timing
- defense window controller is not autonomous combat logic

AI must not propose:
- enabling auto-cycle to simulate AI
- moving defense timing ownership away from AI

### Enemy Switching Boundary
`EnemySwitchingManager` owns scene-level active enemy switching.

AI must not collapse:
- enemy switching
- AI behavior
- gameplay state ownership

into one system.

### Selection Boundary
`LevelEnemySelectionController` owns level-driven enemy startup selection.

AI must not reintroduce:
- legacy competing startup setup paths
- manual scene-only enemy startup overrides that bypass the validated selection flow

### Encounter Boundary
`LevelEncounterController` owns single-encounter application only.

AI must not turn it into:
- a progression controller
- a UI controller
- a preset injection shortcut layer

### Progression Boundary
`LevelProgressionController` owns multi-level flow only.

AI may allow it to expose read-only state needed by UI.

AI must not turn it into:
- a HUD controller
- a localization owner
- a scene presentation system

### HUD Boundary
`HudController` presents UI and delegates button actions.

AI may extend:
- localized text refresh
- presentation-only state
- read-only display info
- result panel hierarchy support

AI must not:
- move progression ownership into HUD
- let HUD directly execute full progression logic beyond delegation
- turn HUD into a gameplay-state owner

### Gameplay Boundary
`GameplayManager` owns round state and gameplay runtime values.

AI must not:
- make GameplayManager own Retry / Next execution flow
- make GameplayManager the UI presentation owner
- merge level orchestration into gameplay state management

---

## Legacy Handling Rules

### LevelEnemyController
`LevelEnemyController` is legacy.

AI must treat it as:
- non-primary
- not for use in the validated switching-oriented scene
- incompatible with `LevelEnemySelectionController` in the same active setup path

AI must explicitly prevent the user from wiring both in the same scene.

---

## UI / Result Panel Rules

Result Panel Polish 1.0 is completed and validated.

Current validated structure:
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

AI may propose:
- layout cleanup
- typography improvements
- dimmer/card/button styling upgrades
- hiding/reducing normal HUD during result display
- additional localized support text
- read-only progression/gameplay info shown on result panel

AI must not propose:
- rebuilding result flow around a new orchestration manager
- moving Retry / Next execution into GameplayManager
- splitting result flow into unnecessary additional controllers unless the existing validated structure fails in a proven way

---

## Localization Rules

Localization currently uses:
- `LocalizationManager`
- CSV-driven lookup from `Assets/Localization/localization_table.csv`

AI must:
- verify localization keys before assuming UI bugs are logic bugs
- check missing CSV keys before redesigning scripts
- remember that unresolved keys may appear as raw key strings in runtime UI

AI must not:
- assume TMP rendering issues are always font problems
- assume localization bugs require architecture changes

---

## Scene / Inspector Rules

This project is heavily inspector-driven.

AI must always consider scene hookup issues before recommending script rewrites.

When a problem may be caused by scene setup, AI should inspect or request:
- hierarchy structure
- inspector references
- missing field hookups
- hidden placeholder text
- duplicated controllers
- layout component conflicts

AI must prefer:
- exact object names
- exact hierarchy paths
- exact inspector drag targets
- exact component parameter settings

over vague scene advice.

---

## Debugging Rules

When debugging, AI must follow this order:

1. compile errors
2. missing references / inspector hookup
3. hierarchy/layout issues
4. localization data issues
5. runtime ownership violations
6. only then architecture redesign if truly needed

AI must not jump straight to refactor when the real issue is:
- a missing TMP reference
- a raw localization key
- a bad layout group setting
- a duplicated scene component
- a legacy controller still hooked up

---

## Change Strategy Rules

### Prefer
- minimal safe changes
- full replacement files when safer than partial patches
- read-only helper properties instead of ownership rewrites
- data additions before code additions
- asset authoring before architecture churn
- exact instructions over abstract recommendations

### Avoid
- large rewrites without proof they are needed
- speculative abstractions
- manager proliferation
- hidden assumptions about unrevealed code
- “clean architecture” proposals that ignore validated runtime behavior

---

## Documentation Rules

AI must update docs only after runtime validation.

AI must not mark features complete based on:
- intention
- partial code
- untested scene setup
- speculative future design

When a milestone is validated, AI should update the relevant docs:
- `docs/PROJECT_STATE.md`
- `docs/architecture.md`
- `docs/gameplay_systems.md`
- `docs/development_tasks.md`
- `docs/SESSION_LOG.md`

When milestone framing changes significantly, also update:
- `AI_START.md`
- `docs/AI_CONTEXT.md`
- `docs/DEV_WORKFLOW.md`
- `docs/AI_RULES.md`

---

## Current Strategic Guidance

The project is currently strongest when moving toward:
- Level Goal Variety 1.0
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
- Result Panel Polish 1.1

AI should prefer these directions over foundational rewrites.

---

## Final Rule

Power Prank 3D already has validated systems.

AI must behave like a continuity engineer:
- inspect first
- preserve what works
- extend carefully
- keep responsibilities clean
- avoid inventing new chaos