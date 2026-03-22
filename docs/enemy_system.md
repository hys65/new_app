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

This is not just a renamed copy.
It exists to support:
- dedicated preset
- dedicated defense pattern
- boss-style break rule

---

## Level 04 Briefcase Boss Prototype

### Purpose
Validate the first true boss-defense identity.

### Runtime authoring chain
- `meeting_tyrant_briefcase_boss_defense_pattern`
- `enemy_preset_meeting_tyrant_briefcase_boss`
- `meeting_tyrant_briefcase_boss` roster entry
- `level_enemy_selection_level_04`

### Briefcase boss rules
When briefcase guard is active:
- sponge hammer breaks defense
- non-hammer items are blocked

### Important debugging lesson
Scene-only edits were overwritten by preset application.
The fix was:
- author a dedicated preset
- author a dedicated roster entry
- route the level through that entry

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