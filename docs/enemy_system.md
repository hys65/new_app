# ENEMY SYSTEM

## Overview

The enemy system is data-driven and preset-applied.

Enemy runtime behavior is formed by combining:

- archetype
- defense pattern
- AI profile
- defense state window profile

through:

- `EnemyPresetData`
- `EnemyPresetApplicator`

---

## Runtime Components

### Core runtime enemy components
- `EnemyReactionLayerController`
- `EnemyDefenseController`
- `EnemyDefenseVisualLayerController`
- `EnemyDefenseStateWindowController`
- `EnemyAiLayerController`
- `EnemyPresetApplicator`
- `EnemyRuntimePresetController`
- `EnemyVisualProxyController`

---

## Data Components

### `EnemyArchetypeData`
Defines broad enemy identity tendencies.

### `EnemyDefensePatternData`
Defines defense behavior details, such as:
- blockability
- duration
- cooldown
- break items
- timed activation
- reactive activation
- weakness settings
- boss-specific special handling

### `EnemyAiProfileData`
Defines enemy timing/personality control where applicable.

### `EnemyDefenseStateWindowProfileData`
Defines defense window profile behavior.

### `EnemyPresetData`
Combines:
- archetype
- defense pattern
- AI profile
- defense state window profile

### `EnemyRosterData`
Maps:
- roster entry id
- preset
- recommended slot

---

## Critical Runtime Rule

Do not treat scene component references as final truth.

Why:
- runtime preset application can overwrite defense pattern and defense window profile references

Therefore:
- if a boss pattern “looks correct before Play” but changes on startup,
  inspect the active runtime preset and roster chain first

---

## Current Enemy Types

### 1. `meeting_tyrant`
Base Meeting Tyrant runtime identity.

### 2. `narcissist_manager`
Base Narcissist Manager runtime identity.

### 3. `meeting_tyrant_briefcase_boss`
Boss-variant roster entry created for Level 04.

Supports:
- dedicated preset
- dedicated defense pattern
- boss-style hammer-break rule

### 4. `narcissist_manager_sunglasses_boss`
Boss-variant roster entry created for Level 05.

Supports:
- dedicated preset
- dedicated defense pattern
- boss-style foam-break / paint-finish rule

---

## Level 04 Briefcase Boss

### Purpose
Validate the first true boss-defense identity.

### Runtime authoring chain
- `meeting_tyrant_briefcase_boss_defense_pattern`
- `enemy_preset_meeting_tyrant_briefcase_boss`
- `meeting_tyrant_briefcase_boss` roster entry
- `level_enemy_selection_level_04`

### Rule
When briefcase guard is active:
- sponge hammer breaks defense
- non-hammer items are blocked

---

## Level 05 Sunglasses Boss

### Purpose
Validate the second boss-defense identity and push weapon-role meaning further.

### Runtime authoring chain
- `narcissist_manager_sunglasses_boss_defense_pattern`
- `enemy_preset_narcissist_manager_sunglasses_boss`
- `narcissist_manager_sunglasses_boss` roster entry
- `level_enemy_selection_level_05`

### Rule
When sunglasses face guard is active:
- paint is ineffective
- foam breaks defense
- paint becomes valid again after break

### Content meaning
This is not just “another block.”  
It forces a two-step combat read:

1. read boss defense timing
2. use the breaker item
3. switch to the actual scoring item

---

## Enemy Debugging Rules

When enemy behavior appears wrong:

### Step 1
Confirm you selected the actual active runtime enemy root.

### Step 2
Inspect runtime fields during Play:
- current preset
- defense pattern
- defense active
- runtime elapsed
- next timed activation

### Step 3
If scene values changed on startup:
- inspect `EnemyPresetApplicator`
- inspect `EnemyPresetData`
- inspect the roster entry used by current level

### Step 4
Only after data-chain validation, inspect controller code

---

## Collision / Hit Rules

Validated gameplay collision structure:

- `EnemyVisual` = visual only
- visual collider disabled
- `Torso` = body hit collider
- `HeadCollider` tagged `Head`
- hit reaction not attached to `EnemyVisual`

This structure must not be regressed.

---

## Current Recommended Enemy-System Direction

Next enemy-system work should focus on:
- boss identity expansion through new presets / patterns / roster entries
- stronger weapon-role readability
- avoiding repeated generic enemies after Level 03

Not on:
- broad rewrites
- abandoning preset-driven runtime injection