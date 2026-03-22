# ARCHITECTURE

## Architectural Summary

`Power Prank 3D` is a single-scene, data-driven Unity prototype.

Its current architecture is built around:

- one active enemy at runtime
- multiple enemy roots existing in one scene
- level-driven encounter configuration
- progression-driven multi-level flow
- preset-driven enemy behavior injection
- HUD-driven player-facing feedback

---

## High-Level Runtime Flow

### Enemy data layer
- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`

### Enemy preset layer
- `EnemyPresetData`
- `EnemyPresetApplicator`
- `EnemyRuntimePresetController`

### Enemy switching / level selection layer
- `EnemyRosterData`
- `LevelEnemySelectionData`
- `LevelEnemySelectionController`
- `EnemySwitchingManager`

### Encounter layer
- `LevelEncounterConfigData`
- `LevelEncounterController`
- `LevelGoalController`
- `GameplayManager`

### Progression / presentation layer
- `LevelProgressionData`
- `LevelProgressionController`
- `HudController`

---

## Ownership Boundaries

### `EnemyPresetApplicator`
Owns:
- writing preset data into runtime enemy components

Must:
- remain the only preset injection layer

Must not:
- be bypassed by direct runtime scene-field assignment hacks

---

### `EnemySwitchingManager`
Owns:
- choosing which scene enemy root is active

Must not:
- own encounter rules
- own progression logic

---

### `LevelEnemySelectionController`
Owns:
- applying enemy selection content for a level
- mapping roster entry → slot / preset chain

Must not:
- own progression
- own round state

---

### `LevelEncounterController`
Owns:
- applying one encounter’s time / goal / enemy selection content

Must not:
- own multi-level progression
- own result UI

---

### `LevelProgressionController`
Owns:
- current level index
- startup level application
- retry current level
- advance to next level

Must not:
- absorb encounter-application logic
- absorb HUD ownership

---

### `GameplayManager`
Owns:
- round start / running / finish state
- breakdown state
- selected item state
- combo state

Must not:
- become the executor of retry / next scene-like progression flow

---

### `HudController`
Owns:
- result panel display
- current live HUD display
- goal-aware top-left HUD text
- player-facing Retry / Next button presentation

Must not:
- become the progression system itself

---

## Current Content Architecture

The project now has two content blocks:

### Teaching block
Levels 01–03

Characteristics:
- explain goal types
- low confusion
- low boss identity complexity

### Boss-content block
Level 04+

Characteristics:
- each level should add a new boss read or break rule
- repeated generic encounters are not acceptable as “content expansion”

---

## Boss Prototype Architecture Rule

Level 04 established a critical architectural rule:

Boss-specific behavior must be introduced through:

1. dedicated `EnemyDefensePatternData`
2. dedicated `EnemyPresetData`
3. dedicated roster entry
4. level enemy selection pointing to that roster entry

Not through:
- scene-only manual component edits
- pre-Play inspector values that are overwritten at runtime

This rule exists because runtime preset application overwrites defense-related references.

---

## Current Data Authoring Principle

When adding a new boss-style enemy variant:

1. create or duplicate a dedicated `EnemyDefensePatternData`
2. create or duplicate a dedicated `EnemyPresetData`
3. add a new roster entry
4. point the level selection asset to that roster entry
5. verify runtime active enemy root, not inactive scene roots

---

## Legacy Rule

`LevelEnemyController` is legacy.

It must not coexist with the current selection/switching/progression path in the same scene-driven flow.

---

## Current Recommended Architectural Direction

Do not introduce broad new architecture before content demands it.

Current best leverage is:

- more boss identity through data
- clearer weapon-counter rules
- clean roster/preset authoring
- minimal system expansion only when content truly requires it