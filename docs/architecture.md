# architecture.md

## Power Prank 3D — System Architecture

------

# CORE PRINCIPLE

> The entire game is **data-driven**.

No gameplay behavior should depend on:

- Scene manual setup
- Hardcoded logic in MonoBehaviours

Everything must flow through:

> Data → Profile → Preset → Runtime Application

------

# ENEMY SYSTEM ARCHITECTURE

## Full Runtime Chain

```
EnemyArchetypeData
    ↓
EnemyDefensePatternData
    ↓
EnemyDefenseStateWindowProfileData
    ↓
EnemyDefenseVisualProfileData
    ↓
EnemyPreset
    ↓
EnemyPresetApplicator (Runtime)
    ↓
Enemy Instance (Scene)
```

------

## Layer Responsibilities

### 1. Archetype Layer

Defines:

- Enemy identity
- Base behavior rules

------

### 2. Defense Pattern Layer

Defines:

- Timing structure
- Guard rhythm

------

### 3. State Window Profile Layer

Defines:

- When enemy is blocking
- When enemy is vulnerable

------

### 4. Visual Profile Layer

Defines:

- How defense looks
- Arm pose / guard posture / visual feedback

------

### 5. Preset Layer

Combines:

- Archetype
- Pattern
- State window profile
- Visual profile

This is the **single source of truth** for an enemy configuration.

------

### 6. Runtime Application

Handled by:

- `EnemyPresetApplicator`

Rules:

- Applies preset at runtime
- Overwrites scene state
- Ensures consistency

------

# LEVEL SYSTEM ARCHITECTURE

## Full Chain

```
EnemyRoster
    ↓
LevelEnemySelection
    ↓
LevelEncounterConfig
    ↓
LevelProgressionData
    ↓
Runtime Level Loader
```

------

## Responsibilities

### EnemyRoster

- Defines all available enemies

### LevelEnemySelection

- Selects enemy per level

### LevelEncounterConfig

- Defines gameplay parameters (TargetCount, duration, etc.)

### LevelProgressionData

- Defines progression order

### Runtime

- Loads level
- Applies encounter
- Spawns correct enemy

------

# CRITICAL ARCHITECTURE RULES

## Rule 1 — No Parallel Systems

Do NOT:

- Add alternate injection paths
- Bypass preset system
- Modify enemy directly in scene

Everything must go through:

> Preset → Applicator

------

## Rule 2 — Scene Is Not Source of Truth

Scene objects are:

- Temporary
- Overwritten at runtime

Do NOT treat:

- Transform tweaks
- Manual mesh positioning

As final behavior definitions.

------

## Rule 3 — Boss Must Stay in Pipeline

Even during redesign:

Boss must still follow:

```
Preset → Roster → Level Selection → Runtime
```

No exceptions.

------

# CURRENT VISUAL PROTOTYPE STATUS

Recent work includes:

- Sunglasses identity (Level 05)
- Face guard arm pose (Level 09)
- Head target emphasis (Level 11)

These are:

- Scene-level adjustments
- Visual exploration
- Not formalized in data layer

------

## IMPORTANT

> These prototypes must NOT be treated as architecture features.

They are:

- Temporary
- Disposable

------

# FUTURE BOSS REDESIGN REQUIREMENT

All future boss work must:

1. Be defined in data (profiles / presets)
2. Not rely on manual hierarchy edits
3. Be reproducible via preset application

------

# SUMMARY

- Architecture is complete and stable
- Data-driven pipeline is validated
- Runtime application is consistent
- Level system is fully integrated

------

# CURRENT LIMITATION (KNOWN)

- Visual identity is not yet systematized
- Boss presentation still relies on temporary scene adjustments

------

# NEXT STEP (ARCHITECTURE LEVEL)

Not to change architecture.

Instead:

- Redesign boss content
- Then re-encode into existing system

------

# FINAL STATEMENT

> Do NOT change architecture for content problems.

Fix content inside the architecture.
