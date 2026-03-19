# ARCHITECTURE

## Core Design Philosophy

Power Prank 3D is built around a layered, data-driven runtime architecture.

Rules:
1. Prefer data over hardcoded enemy-specific logic
2. Keep single responsibility per runtime layer
3. Preserve readable inspector workflows
4. Do not duplicate preset injection logic
5. Extend systems by adding orchestration layers, not by collapsing responsibilities

---

## High-Level Runtime Layers

### 1. Gameplay Layer
Handles:
- throw input
- projectile spawning
- hit resolution
- breakdown accumulation
- HUD updates
- gameplay loop state

Representative systems:
- ThrowController
- ProjectileBehavior
- GameplayManager
- HudController
- HitPopupSpawner

---

### 2. Enemy Runtime Layer
Handles runtime enemy behavior and presentation.

Representative systems:
- EnemyReactionLayerController
- EnemyDefenseController
- EnemyDefenseVisualLayerController
- EnemyDefenseStateWindowController
- EnemyAiLayerController
- EnemyVisualProxyController

---

### 3. Data Definition Layer
Defines reusable behavior data assets.

Current enemy data assets:
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- EnemyPresetData
- EnemyRosterData
- LevelEnemySelectionData

This layer defines behavior and selection content.  
It does not execute behavior.

---

## Enemy Preset Architecture

### EnemyPresetData
EnemyPresetData is the combined configuration object for one enemy setup.

It references:
- archetype data
- defense pattern data
- AI profile data
- defense state window profile data

This allows multiple enemy personas to be built by combining reusable data assets.

---

### EnemyPresetApplicator
EnemyPresetApplicator is the preset injection layer.

Responsibilities:
- receive EnemyPresetData
- distribute preset references to runtime enemy controllers
- remain the single source of preset application

Important rule:

**EnemyPresetApplicator must remain the only preset injection point.**

Do not:
- push preset data directly from manager scripts into multiple controllers
- duplicate preset application logic elsewhere
- bypass this layer for convenience

---

## Enemy Runtime Architecture

Enemy runtime architecture is now split into five layers:

### 1. Data Layer
Defines enemy behavior and level selection assets.

Behavior assets:
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- EnemyPresetData

Selection assets:
- EnemyRosterData
- LevelEnemySelectionData

---

### 2. Preset Application Layer
Applies combined preset data to runtime controllers.

- EnemyPresetApplicator

Responsibility:
- distribute preset data to runtime enemy controllers
- remain the single source of preset application

---

### 3. Single Enemy Runtime Layer
Handles runtime preset control for one enemy instance.

- EnemyRuntimePresetController

Responsibility:
- own runtime preset switching entry for one enemy
- forward preset application to EnemyPresetApplicator
- avoid duplicate preset injection logic elsewhere

---

### 4. Scene Enemy Switching Layer
Handles active enemy selection at scene level.

- EnemySwitchingManager

Responsibility:
- maintain enemy slots
- switch active enemy at runtime
- apply default preset per slot
- sync active enemy reaction layer into GameplayManager

---

### 5. Level Enemy Selection Layer
Handles reusable content-driven startup selection.

- LevelEnemySelectionController
- LevelEnemySelectionData
- EnemyRosterData

Responsibility:
- define a reusable enemy catalog
- define which roster entries are used by a level
- resolve startup enemy from level content
- configure EnemySwitchingManager without bypassing runtime switching architecture

---

## Enemy Switching Flow

### Runtime Preset Flow
EnemySwitchingManager  
→ EnemyRuntimePresetController  
→ EnemyPresetApplicator  
→ EnemyReactionLayerController / EnemyDefenseController / EnemyAiLayerController / EnemyDefenseStateWindowController

### Gameplay Sync Flow
EnemySwitchingManager  
→ GameplayManager.SetActiveEnemyReactionLayer(...)  
→ active EnemyReactionLayerController

---

## Level Enemy Selection Flow

Startup flow:

EnemyRosterData  
→ LevelEnemySelectionData  
→ LevelEnemySelectionController  
→ EnemySwitchingManager.ConfigureSlotDefaultPreset(...)  
→ EnemySwitchingManager.ConfigureStartupSlot(...)  
→ EnemySwitchingManager.Start()  
→ EnemyRuntimePresetController  
→ EnemyPresetApplicator

This keeps level content selection above the scene switching layer, while preserving a single preset injection point.

---

## GameplayManager Runtime Sync

GameplayManager remains the gameplay-side owner of current breakdown state and reaction stage refresh.

Runtime switching support is handled by:
- replacing the active EnemyReactionLayerController reference
- immediately refreshing reaction stage after switching target

This keeps gameplay state centralized while allowing the active enemy target to change.

---

## Current Runtime Model

Current model is:
- multiple enemy roots may exist in scene
- only one enemy is active at a time
- switching occurs at scene orchestration level
- AI remains per-enemy and does not manage scene switching
- defense window `autoCycle` must remain FALSE
- AI still controls defense timing

---

## Legacy Compatibility

### LevelEnemyController
`LevelEnemyController` is now considered a legacy startup setup path.

Historical role:
- read EnemyLevelConfig
- assign preset to EnemyPresetApplicator
- apply preset on startup

Current rule:
- it may exist only for older scenes if explicitly needed
- it must not run in scenes that use `EnemySwitchingManager`
- it must not coexist with `LevelEnemySelectionController` in the same scene

Reason:
- this would create competing preset setup paths
- it breaks clean startup ownership
- it causes duplicate or incorrect preset application during scene startup

The current validated switching scene has removed the old `LevelEnemyController` hookup.

---

## Important Boundary

Enemy Switching System 1.0 + Enemy Roster / Level Enemy Selection 1.0 is not a multi-enemy combat system.

It supports:
- multiple enemy objects in scene
- single active enemy switching
- runtime preset switching
- slot-based inspector setup
- level-driven startup enemy selection

It does not yet support:
- multiple active enemies at the same time
- separate breakdown values per enemy
- wave spawning
- roster-driven runtime enemy loading
- full combat ownership transfer per enemy instance

---

## Recommended Next Architectural Step

Next clean extension point:

### Level Content / Encounter Configuration 1.0

Goal:
Move from enemy-only startup selection into reusable level encounter authoring.

Expected outcome:
- level-driven enemy startup selection
- level-driven gameplay targets
- level-driven time limit settings
- cleaner content authoring per stage
- less manual scene-only configuration