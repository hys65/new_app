# AI CONTEXT

## Project Identity

Project name: **Power Prank 3D**

Project type:
- Unity 6.3 LTS prototype
- single-scene runtime with multi-level content progression
- fixed-camera prank-throwing boss game

Core fantasy:
- throw prank items
- build breakdown pressure
- read defense timing
- react to boss identity
- clear goal-driven encounters

---

## Current Stage

The project is now in this stage:

**cleaned data-driven boss-reference prototype with unified asset layout**

Already implemented and runtime-validated:
- core throw / hit / breakdown gameplay loop
- enemy reaction layer
- enemy defense visual layer
- enemy archetype system
- enemy AI layer
- enemy switching / roster / level selection
- level encounter configuration
- level progression / runtime level advance
- victory choice flow
- result panel polish
- level goal variety
- goal HUD readability
- boss preset override debugging pass
- combat pacing / per-item throw cooldown pass

Validated boss-reference ladder:
- Level 04 = Meeting Tyrant briefcase boss
- Level 05 = Narcissist Manager sunglasses boss
- Level 06 = Meeting Tyrant weak-window boss
- Level 07 = Narcissist Manager precision paint boss
- Level 08 = Zero-Mistake Boss
- Level 09 = Narcissist Manager Face Guard Boss

---

## Canonical Repository Structure

### Runtime scripts
All gameplay scripts now live under:

```text
unity-client/Assets/Scripts/gameplay/
  Core/
  Data/
  Enemy/
  UI/
  VFX/
```

Important rule:
- there is no separate lowercase `gameplay/enemy/` runtime path anymore
- `gameplay/Enemy/` is the only valid enemy-runtime script folder

### Gameplay data assets
Canonical data asset roots:

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

unity-client/Assets/Data/Levels/
  Encounters/
  EnemySelections/
  Progression/
```

### Gameplay item assets
Gameplay items remain here:

```text
unity-client/Assets/ScriptableObjects/GameplayItems/
```

Important rule:
- enemy data must not be reintroduced into `Assets/ScriptableObjects/Enemy/`
- enemy presets / patterns / AI / archetypes / level configs must not be left in `Assets/` root

---

## Current Runtime Model

The runtime model is:

- multiple enemy roots may exist in one scene
- only one enemy is active at a time
- progression decides which encounter is applied
- encounter decides which enemy selection and goal data are active
- runtime preset application decides final enemy behavior

This is not:
- a procedural encounter generator
- a multi-enemy combat sandbox
- a scene-manual boss tuning workflow

It is:
- a reusable single-scene multi-level boss-content prototype

---

## Mandatory Architecture Rules

### Runtime authority
Runtime preset application remains authoritative.

Do not treat pre-Play inspector values as final truth when preset routing is active.

Boss-specific behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

### Canonical asset naming
Use the current naming families only:
- `enemy_ai_*`
- `enemy_archetype_*`
- `defense_pattern_*`
- `defense_state_window_*`
- `enemy_preset_*`
- `level_*_encounter_config`
- `level_enemy_selection_level_*`

Do not create or restore alternate legacy families such as:
- `*_ai_profile`
- `*_archetype_data`
- `*_defense_pattern_data`
- `*_defense_state_window_profile`
- `*_enemy_preset_data`

### Scene startup
`LevelEnemyController` is legacy.
Do not build new work around it if `LevelEnemySelectionController` and progression are active in the same scene.

---

## Current Production Direction

Levels 01–03 are the teaching block.  
Levels 04–09 are the validated boss-reference block.

Current next design target:
- create a clean Level 10 boss identity
- reuse current systems before proposing new ones
- avoid fake repeats of Levels 04–09
- preserve the cleaned repository structure while authoring new content
