# GAMEPLAY SYSTEMS

## Core Loop

Throw  
→ Hit  
→ Defense Evaluation  
→ Breakdown / Block / Break Result  
→ Reaction  
→ HUD Update  
→ Round Result  
→ Retry or Next Level

---

## Core Gameplay Systems

### ThrowController
Handles:
- mouse drag input
- drag distance based throw force
- aim preview
- trajectory preview
- projectile spawn and throw

Current rules:
- click alone does not throw
- dragging below minimum threshold does not throw
- throw input is disabled when round is not running
- drag / preview state resets on round finish

---

### ProjectileBehavior
Handles:
- projectile collision
- item-based hit resolution
- interaction with enemy defense system
- ground hit behavior
- impact VFX / stain / popup spawning hooks

Supports:
- body hit
- head hit
- ground hit

---

### GameplayManager
Gameplay-side owner of:
- CurrentBreakdownValue
- TargetBreakdownValue
- RemainingTimeSeconds
- round running state
- current selected item
- combo state

Current responsibilities:
- apply encounter settings
- start round
- finish round
- add breakdown
- restart round
- update active enemy reaction layer

---

### HudController
Handles:
- current breakdown text
- target breakdown text
- timer text
- selected item text
- combo display
- result panel display
- Retry / Next button delegation

Result flow:
- on victory: show Retry + Next if another level exists
- on failure: show Retry only

---

## Hit Result Types

1. Normal Hit
2. Block
3. Break
4. Weak Hit
5. Ground Hit

---

## Breakdown Rules

- Breakdown increases on successful hits
- Breakdown pauses during Block
- Breakdown accelerates on weakness hits
- Breakdown target is level-driven through encounter config
- Round ends in victory when breakdown reaches target

---

## Interaction with Enemy Systems

### Reaction Layer
Controls visual feedback.

### Defense Pattern
Controls whether hit is blocked / weak / broken.

### Defense Window
Controls when block is valid.

### AI Layer
Controls when defense starts.

### Key Rule
Block is only valid during:

Defense Window  
→ Active

### Weak Window
Inside Active state:
- vulnerable timing window
- allows skilled hits
- rewards timing instead of spam

---

## Player Skill Expression

Player can:
- read telegraph
- time attacks
- exploit weak window
- break defense
- choose Retry / Next after victory

Gameplay is not spam.  
Gameplay = timing + rhythm + reading enemy state.

---

## Enemy Selection System

### EnemyRosterData
Defines reusable enemy catalog entries.

### LevelEnemySelectionData
Defines:
- roster source
- selected roster entries
- startupSelectionIndex
- startup preset / slot application rules

### LevelEnemySelectionController
Resolves a level enemy selection asset into:
- slot default preset setup
- startup slot selection
- immediate runtime active enemy switching

---

## Encounter System

### LevelEncounterConfigData
Defines a single playable level encounter.

Fields:
- levelId
- displayName
- enemySelection
- targetBreakdownValue
- roundDurationSeconds
- autoStartRound

This is the asset that defines:
- which enemy selection this level uses
- how much breakdown is required
- how much time the round gives

### LevelEncounterController
Applies encounter config into runtime systems:
- gameplay target breakdown
- round duration
- level enemy selection

---

## Progression System

### LevelProgressionData
Defines:
- ordered list of encounter configs
- startup level index

### LevelProgressionController
Handles:
- startup level application
- apply level by index
- next level
- restart current level
- next-level availability check

This enables:
- one scene
- multiple authored level configs
- runtime progression across multiple encounters

---

## Runtime Level Transition Behavior

Current runtime transition supports:
- changing target breakdown between levels
- changing timer between levels
- changing active enemy between levels
- reapplying enemy preset at runtime
- starting a fresh round after level change

Stability rules:
- drag state is reset on round finish
- runtime transitions avoid carrying old throw state forward
- result panel must disappear when a new round starts

---

## Current Authored Level Content

Current validated content includes:
- Level 01 encounter
- Level 02 encounter
- Level 03 encounter

Current validated enemy selection assets include:
- Meeting Tyrant startup selection
- Narcissist Manager startup selection

Current validated progression behaviors include:
- startup level selection
- manual next level
- runtime next level
- retry current level
- victory-choice driven next / retry flow