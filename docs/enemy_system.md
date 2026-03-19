# ENEMY SYSTEM

## Purpose

The enemy system defines how enemy characters:
- receive hits
- accumulate breakdown pressure
- react visually
- defend against attacks
- vary by enemy persona
- switch between configured runtime enemy identities
- be selected by reusable level content

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

### Combined behavior data asset
- EnemyPresetData

EnemyPresetData combines the lower-level behavior assets into one playable enemy preset.

### Content selection assets
- EnemyRosterData
- LevelEnemySelectionData

These assets define which enemy presets are available and which ones are selected by a level.

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

### Runtime Components

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

## Enemy Roster / Level Enemy Selection 1.0

Enemy Roster / Level Enemy Selection 1.0 extends the switching layer into reusable level content flow.

### New Content Components

#### EnemyRosterData
Reusable enemy catalog asset.

Each roster entry defines:
- entryId
- displayName
- preset
- recommendedSlotId
- enabled

#### LevelEnemySelectionData
Level-specific enemy selection asset.

Defines:
- which roster is used
- which roster entries are selected by the level
- which selected entry is used at startup
- whether slot defaults are cleared before assignment
- whether startup default preset is automatically applied

#### LevelEnemySelectionController
Startup orchestration controller.

Responsibilities:
- read LevelEnemySelectionData
- resolve selected roster entries into scene slot ids
- configure EnemySwitchingManager slot default presets
- configure startup slot index
- avoid bypassing EnemySwitchingManager / EnemyRuntimePresetController / EnemyPresetApplicator

---

## Enemy Roster / Level Selection Flow

Startup flow:

EnemyRosterData  
→ LevelEnemySelectionData  
→ LevelEnemySelectionController  
→ EnemySwitchingManager  
→ EnemyRuntimePresetController  
→ EnemyPresetApplicator  
→ enemy runtime controllers

This turns manual scene test wiring into reusable level-driven startup enemy selection.

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

### Use Case 3
Level-driven startup enemy selection.

Validated for:
- `startupSelectionIndex = 0`
- `startupSelectionIndex = 1`

Observed behavior:
- startup enemy is determined by LevelEnemySelectionData
- only the selected startup enemy preset is applied
- inactive enemy is disabled correctly
- legacy startup preset conflicts are removed after removing scene `LevelEnemyController`

---

## Legacy Note

### LevelEnemyController
`LevelEnemyController` is now a legacy startup path.

Current rule:
- it is not part of the new validated startup flow
- it must not be used in scenes driven by LevelEnemySelectionController
- it must not compete with EnemySwitchingManager startup ownership

The current validated scene has removed the old `LevelEnemyController` hookup from `Systems`.

---

## Current Boundary

The current enemy system supports:
- multiple enemy objects in scene
- single active enemy selection
- runtime preset switching
- slot-based scene setup
- level-driven startup enemy selection

It does not yet support:
- simultaneous active enemies
- separate gameplay state per enemy
- wave system
- spawn pipeline
- roster-driven runtime loading

---

## Recommended Next Step

Recommended next milestone:
- Level Content / Encounter Configuration 1.0

Reason:
The switching system and level startup enemy selection are now validated.  
The next step is to turn enemy-only startup selection into full level encounter content flow.