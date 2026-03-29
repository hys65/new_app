# ARCHITECTURE

## Architectural Summary

`Power Prank 3D` is a single-scene, data-driven Unity prototype.

Its architecture is built around:
- one active enemy at runtime
- multiple enemy roots coexisting in one scene
- level-driven encounter configuration
- progression-driven multi-level flow
- preset-driven enemy behavior injection
- HUD-driven player-facing feedback

---

## Canonical Repository Structure

### Scripts
```text
unity-client/Assets/Scripts/gameplay/
  Core/
  Data/
  Enemy/
  UI/
  VFX/
```

Rules:
- `gameplay/Enemy/` is the only valid enemy runtime script directory
- do not recreate a parallel lowercase `gameplay/enemy/` tree
- do not scatter gameplay runtime scripts into alternate folders without a clear module reason

### Enemy data assets
```text
unity-client/Assets/Data/Enemy/
  AI/
  Archetypes/
  Defense/
    Patterns/
    StateWindows/
    Visuals/
  Presets/
  Rosters/
```

### Level data assets
```text
unity-client/Assets/Data/Levels/
  Encounters/
  EnemySelections/
  Progression/
```

### Gameplay item data
```text
unity-client/Assets/ScriptableObjects/GameplayItems/
```

Rules:
- enemy data must not live in `Assets/ScriptableObjects/Enemy/`
- enemy or level config assets must not be left in `Assets/` root
- legacy naming families must not be reintroduced

---

## High-Level Runtime Flow

### Enemy data layer
- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`
- `EnemyDefenseVisualProfileData`

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

### `EnemySwitchingManager`
Owns:
- choosing which scene enemy root is active

Must not:
- own encounter rules
- own progression logic

### `LevelEnemySelectionController`
Owns:
- applying enemy selection content for a level
- mapping roster entry → slot / preset chain

Must not:
- own progression
- own round state

### `LevelEncounterController`
Owns:
- applying one encounter’s time / goal / enemy selection content

Must not:
- own multi-level progression
- own result UI

### `LevelProgressionController`
Owns:
- current level index
- startup level application
- retry current level
- advance to next level

Must not:
- absorb encounter-application logic
- absorb HUD ownership

### `GameplayManager`
Owns:
- round start / running / finish state
- breakdown state
- selected item state
- combo state

Must not:
- become the executor of retry / next progression flow

### `HudController`
Owns:
- current live HUD display
- result panel display
- goal-aware HUD text
- player-facing Retry / Next presentation

Must not:
- become the progression system itself

---

## Boss Authoring Rule

Boss-specific behavior must be introduced through:

1. dedicated `EnemyDefensePatternData`
2. dedicated `EnemyDefenseStateWindowProfileData` when needed
3. dedicated `EnemyPresetData`
4. dedicated roster entry
5. level enemy selection pointing to that roster entry

Not through:
- scene-only manual component edits
- pre-Play inspector values that are overwritten at runtime
- duplicate legacy asset families

This rule exists because runtime preset application overwrites defense-related references.

Canonical authoring chain:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

---

## Current Content Architecture

### Teaching block
Levels 01–03
- explain goal types
- low confusion
- low boss identity complexity

### Boss-reference block
Levels 04–09
- each level has a distinct boss demand
- each level is a data-authored reference implementation
- these levels should not be casually redesigned once validated

### Expansion block
Level 10+
- must continue unique boss identity work
- should reuse the current asset layout and runtime chain
- should avoid fake repetition of prior bosses

---

## Legacy Rule

`LevelEnemyController` is legacy.

Do not build new content flow around it if `LevelEnemySelectionController` and progression are active in the same scene.

---

## Current Recommended Direction

Do not introduce broad new architecture before content truly demands it.

Best leverage remains:
- more boss identity through data
- clean roster/preset authoring
- minimal system expansion only when current systems are proven insufficient
