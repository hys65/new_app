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
Defines reusable behavior and level content assets.

Current data assets:
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- EnemyPresetData
- EnemyRosterData
- LevelEnemySelectionData
- LevelEncounterConfigData
- LevelProgressionData

This layer defines reusable configuration content.  
It does not execute runtime behavior.

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

Enemy runtime architecture is split into five layers:

### 1. Data Layer
Defines enemy behavior and selection assets.

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
Handles reusable content-driven startup and runtime selection.

- LevelEnemySelectionController
- LevelEnemySelectionData
- EnemyRosterData

Responsibility:
- define a reusable enemy catalog
- define which roster entries are used by a level
- resolve startup enemy from level content
- configure EnemySwitchingManager without bypassing runtime switching architecture
- switch to resolved active slot during runtime level application

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

Selection flow:

EnemyRosterData  
→ LevelEnemySelectionData  
→ LevelEnemySelectionController  
→ EnemySwitchingManager.ConfigureSlotDefaultPreset(...)  
→ EnemySwitchingManager.ConfigureStartupSlot(...)  
→ EnemySwitchingManager.SwitchToSlot(...)  
→ EnemyRuntimePresetController  
→ EnemyPresetApplicator

This keeps level content selection above the scene switching layer, while preserving a single preset injection point.

---

## Encounter Architecture

### LevelEncounterConfigData
Defines the content of one playable encounter.

Fields:
- levelId
- displayName
- enemySelection : LevelEnemySelectionData
- targetBreakdownValue
- roundDurationSeconds
- autoStartRound

Purpose:
- bind enemy startup selection together with gameplay target/time
- move single-level gameplay authoring out of manual inspector-only scene setup

### LevelEncounterController
Applies a single encounter config into runtime systems.

Responsibilities:
- set target breakdown in GameplayManager
- set round duration in GameplayManager
- apply LevelEnemySelectionData through LevelEnemySelectionController

Important boundary:
- LevelEncounterController owns single-encounter application only
- it must not own multi-level progression logic
- it must not inject presets directly

---

## Progression Architecture

### LevelProgressionData
Defines:
- progressionId
- displayName
- ordered encounter list
- startupLevelIndex

### LevelProgressionController
Handles:
- ApplyStartupLevel()
- ApplyLevelByIndex(int)
- AdvanceToNextLevel()
- RestartCurrentLevel()
- HasNextLevel()

Responsibilities:
- choose which encounter is active
- apply startup level from progression asset
- advance or restart at runtime
- orchestrate single-scene multi-level flow

Important boundary:
- Progression owns level flow
- it must not own HUD presentation
- it must not bypass LevelEncounterController
- it must not inject enemy preset data directly

---

## Result Flow Architecture

Victory / failure UI is handled through HudController.

Flow:
GameplayManager.OnRoundFinished  
→ HudController shows result panel  
→ Retry button calls LevelProgressionController.RestartCurrentLevel()  
→ Next button calls LevelProgressionController.AdvanceToNextLevel()

Responsibilities:
- HUD presents options
- Progression executes level flow
- GameplayManager owns round state

Current player-facing result behavior:
- victory shows Retry + Next when another level exists
- failure shows Retry only
- final level hides Next

---

## Full Runtime Level Flow

Current level-flow stack:

LevelProgressionData  
→ LevelProgressionController  
→ LevelEncounterController  
→ GameplayManager + LevelEnemySelectionController  
→ EnemySwitchingManager  
→ EnemyRuntimePresetController  
→ EnemyPresetApplicator

This is now the main validated runtime architecture for the current scene.

---

## GameplayManager Runtime Sync

GameplayManager remains the gameplay-side owner of:
- current breakdown state
- target breakdown state
- round duration / remaining time
- selected item
- round running state
- reaction stage refresh

Runtime support now includes:
- replacing the active EnemyReactionLayerController reference
- applying encounter target/time values
- starting fresh rounds after level application

This keeps gameplay state centralized while allowing both active enemy and current level to change.

---

## Runtime Transition Rules

When switching or advancing levels at runtime:
1. Apply next encounter config
2. Apply next enemy selection
3. Switch active enemy slot immediately
4. Start a fresh round
5. HUD refresh hides prior result panel when the next round starts

To support stable transitions:
- ThrowController resets drag / preview state on round finish
- progression transitions may be delayed slightly to avoid same-frame end/start contamination

---

## Current Runtime Model

Current model is:
- multiple enemy roots may exist in scene
- only one enemy is active at a time
- switching occurs at scene orchestration level
- AI remains per-enemy and does not manage scene switching
- defense window `autoCycle` must remain FALSE
- AI still controls defense timing
- one Unity scene can host multiple encounter configs
- progression decides which encounter is active
- HUD result flow decides whether player retries or advances

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
- it must not coexist with the new encounter / progression startup flow

Reason:
- this creates competing startup preset setup paths
- it breaks clean startup ownership
- it causes duplicate or incorrect preset application during scene startup

The current validated switching / progression scene has removed the old `LevelEnemyController` hookup.

---

## Important Boundary

Current architecture is not:
- a multi-enemy combat system
- a wave spawning system
- a runtime enemy loading system
- a separate per-enemy breakdown ownership model

It currently supports:
- multiple enemy objects in scene
- single active enemy switching
- runtime preset switching
- slot-based inspector setup
- level-driven startup enemy selection
- encounter-driven gameplay targets and time limits
- ordered multi-level progression in one scene
- player-controlled post-victory advancement