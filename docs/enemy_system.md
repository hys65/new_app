# ENEMY SYSTEM

## Purpose

The enemy system defines how enemy characters:

- receive hits
- accumulate breakdown pressure
- react visually
- defend against attacks
- vary by enemy persona
- switch between configured runtime enemy identities

The system must remain readable, data-driven, and expandable.

---

## Core Runtime Controllers

### EnemyReactionLayerController
Controls enemy reaction stage changes based on gameplay breakdown state.

Responsibilities:
- evaluate breakdown stage
- update reaction presentation layer
- expose a single current reaction state for the active enemy

---

### EnemyDefenseController
Controls defense state transitions.

Responsibilities:
- enter defense
- maintain active defense window
- exit defense
- support runtime data-driven tuning

---

### EnemyDefenseVisualLayerController
Controls visual defense presentation.

Responsibilities:
- arm positions
- body posture
- readable guard visuals
- visible block feedback

---

### EnemyDefenseStateWindowController
Handles defense state timing windows.

Responsibilities:
- telegraph
- guard
- recover

Important rule:
- `autoCycle` must remain FALSE

This system does not self-drive enemy defense timing.
It only provides the timing structure once activated.

---

### EnemyAiLayerController
Controls defense timing decisions.

Responsibilities:
- observe hit rhythm / pressure
- choose when to trigger defense
- produce character-specific timing behavior using AI profile data

Important rule:
- AI controls defense timing
- not the defense window system

---

### EnemyVisualProxyController
Provides visual mapping support between runtime logic and visible enemy transforms.

---

## Enemy Data Model

Enemy behavior is fully data-driven.

### Base data assets
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData

### Combined data asset
- EnemyPresetData

EnemyPresetData combines the lower-level behavior assets into one playable enemy preset.

---

## Enemy Preset Application

### EnemyPresetApplicator
EnemyPresetApplicator is the runtime preset injection layer.

Responsibilities:
- receive EnemyPresetData
- assign data references to runtime enemy controllers
- centralize all preset application logic

Design rule:

**EnemyPresetApplicator remains the only preset injection entry.**

Do not:
- distribute preset data directly from manager scripts
- duplicate preset application logic in orchestration layers
- bypass EnemyPresetApplicator for convenience

Correct flow:

EnemyPresetApplicator
→ EnemyReactionLayerController
→ EnemyDefenseController
→ EnemyAiLayerController
→ EnemyDefenseStateWindowController

---

## Implemented Enemy Archetypes

### Meeting Tyrant
Behavior identity:
- early defense trigger
- strong guard
- short recover
- stable and hard to break

### Narcissist Manager
Behavior identity:
- late defense trigger
- long telegraph
- short guard
- long recover
- high head weakness
- easy to break

Both archetypes have been runtime-tested and confirmed visually distinct.

---

## Enemy AI Layer 1.0

Status: completed.

Validated behavior principles:

- AI timing is data-driven
- defense trigger timing differs between presets
- defense window profile remains passive until AI triggers it
- no preset-specific hardcoded branches are required in runtime logic

This confirms the enemy stack can scale through data expansion instead of custom script forks.

---

## Enemy Switching System 1.0

Enemy Switching System 1.0 has been implemented as a scene-level orchestration layer above the preset system.

### New Runtime Components

#### EnemyRuntimePresetController
Single-enemy runtime preset entry.

Responsibilities:
- hold reference to EnemyPresetApplicator
- expose runtime preset apply / reapply functions
- keep preset switching centralized per enemy instance

#### EnemySwitchingManager
Scene-level enemy switching manager.

Responsibilities:
- define enemy slots
- select active slot
- apply default preset for slot
- switch active enemy at runtime
- sync GameplayManager with active enemy reaction layer

---

## Runtime Enemy Slot Model

Each slot contains:

- slotId
- displayName
- enemyRoot
- runtimePresetController
- reactionLayer
- defaultPreset

This makes the workflow inspector-friendly and future-ready for multiple scene enemies.

---

## Enemy Switching Flow

Runtime flow:

EnemySwitchingManager
→ EnemyRuntimePresetController
→ EnemyPresetApplicator
→ enemy runtime controllers

Gameplay sync flow:

EnemySwitchingManager
→ GameplayManager.SetActiveEnemyReactionLayer(...)
→ active EnemyReactionLayerController

---

## Current Validated Use Cases

### Use Case 1
Same enemy object, runtime preset switching.

Validated for:
- Meeting Tyrant preset
- Narcissist Manager preset

### Use Case 2
Two real enemy objects in scene, single active enemy switching.

Validated for:
- EnemyRoot_MeetingTyrant
- EnemyRoot_NarcissistManager

Observed behavior:
- only one enemy remains active during play
- active slot index updates correctly
- runtime preset application logs confirm switching
- existing hit / defense / AI flow remains intact

---

## Current Boundary

Enemy Switching System 1.0 supports:

- multiple enemy objects in scene
- single active enemy selection
- runtime preset switching
- slot-based scene setup

It does not yet support:

- simultaneous active enemies
- separate gameplay state per enemy
- wave system
- spawn pipeline
- roster-driven level loading

---

## Recommended Next Step

Recommended next milestone:

- Enemy Roster / Level Enemy Selection 1.0

Reason:
The switching system is now validated.
The next step is to turn test-scene switching into reusable content flow.