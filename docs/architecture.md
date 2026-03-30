# architecture.md

## Project Runtime Model

**Power Prank 3D** is a single-scene, data-authored, multi-level boss-content prototype.

The project is not structured as:

- a scene-per-level campaign
- a manual inspector-only boss tuning workflow
- a procedural combat sandbox

The project is structured as:

- one runtime scene
- reusable enemy roots
- reusable boss presets
- reusable level encounter data
- progression-driven encounter switching

---

## High-Level Runtime Ownership

### `GameplayManager`

Owns:

- round state
- countdown timer
- breakdown accumulation
- selected throw item
- high-level win/lose state

Does not own:

- multi-level progression
- boss selection flow
- result-panel button logic
- boss preset content authoring

### `LevelEncounterController`

Owns:

- applying one encounter configuration into runtime systems
- encounter-level goal setup
- encounter-level round duration / target setup
- encounter-level enemy selection application

Does not own:

- whole progression sequencing
- retry/next UI ownership
- enemy system architecture itself

### `LevelProgressionController`

Owns:

- ordered level list
- current level index
- retry current level
- advance to next level
- integration with victory-choice flow

Does not own:

- direct boss-rule authoring
- direct HUD rendering responsibilities
- low-level combat scoring

### `HudController`

Owns:

- HUD presentation
- goal text readability
- result panel presentation
- forwarding Retry / Next user intent to progression flow

Does not own:

- progression state machine logic
- encounter rule authority
- round-state authority

---

## Enemy Content Architecture

Enemy behavior is authored through layered data.

### Core behavior layers

- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`

These are combined by:

- `EnemyPresetData`

And applied through:

- `EnemyPresetApplicator`

This is the authoritative content path.

Not through:

- scene-only manual component edits
- pre-Play inspector values that are overwritten at runtime
- duplicate legacy asset families

This rule exists because runtime preset application overwrites defense-related references.

Canonical authoring chain:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

---

## Enemy Runtime Flow

The active enemy at runtime is produced through this chain:

Data
→ `EnemyPresetData`
→ `EnemyPresetApplicator`
→ `EnemyRuntimePresetController`
→ `EnemySwitchingManager`
→ `LevelEnemySelectionController`
→ active enemy slot

Important runtime meaning:

- enemy roots can coexist in the scene
- only one routed enemy should be active for the encounter
- preset application defines the real runtime defense behavior
- scene-only manual defense edits are not authoritative once runtime preset routing is active

---

## Encounter Runtime Flow

Encounter flow is produced through this chain:

`LevelEncounterConfigData`
→ `LevelEncounterController`
→ `GameplayManager`
→ `HudController`

Encounter configuration is responsible for:

- which enemy selection is applied
- which goal type is active
- which target count / breakdown is active
- which round duration is active

This means a level is not defined only by which enemy appears.

A level is the combination of:

- enemy content
- goal rule
- timer pressure

---

## Progression Runtime Flow

Progression flow is produced through this chain:

`LevelProgressionData`
→ `LevelProgressionController`
→ `LevelEncounterController`
→ encounter runtime application

The progression controller is the owner of:

- current level order
- retry same level
- advance to next level
- post-victory next-step routing

This is important because:

- encounter logic should not start owning campaign flow
- HUD should not become the progression state machine
- `GameplayManager` should not be overloaded with level-sequencing responsibilities

---

## Current Content Architecture

### Teaching block

Levels 01–03:

- low confusion
- direct rule teaching
- basic onboarding for goal types

### Boss-reference block

Levels 04–11:

- each level has a distinct demand
- each level acts as a validated reference implementation
- these levels should not be casually redesigned once validated

### Expansion / polish block

Current work after Level 11 closure should focus on:

- combat readability
- boss presentation
- presentation-side clarity
- later final release balancing

Do not prematurely convert this phase into final release balancing while presentation is still moving.

---

## Boss Ladder Identity Map

### Level 04

Meeting Tyrant briefcase boss

Core demand:

- understand explicit hard defense and break logic

### Level 05

Narcissist Manager sunglasses boss

Core demand:

- respect face-guard behavior and invalid paint route while guarded

### Level 06

Meeting Tyrant weak-window boss

Core demand:

- exploit short openings correctly

### Level 07

Narcissist Manager precision paint boss

Core demand:

- satisfy a specific-item accuracy rule

### Level 08

Zero-Mistake Boss

Core demand:

- maintain a clean unblocked streak

### Level 09

Narcissist Manager Face Guard Boss

Core demand:

- choose the reliable scoring route through hit-zone judgment

### Level 10

Adaptive Shutdown Boss

Core demand:

- avoid becoming rhythm-readable

### Level 11

Head Hunter Boss

Core demand:

- wait for a later scoring opportunity and convert with head-focused precision

Important architecture meaning:

The boss ladder expanded through new content authoring demands, not through architecture forks.

---

## Legacy Rule

`LevelEnemyController` is legacy.

Do not build new content flow around it if `LevelEnemySelectionController` and progression are active in the same scene.

If both coexist, the newer selection / progression path is the intended production path.
