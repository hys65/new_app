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

Defines enemy timing / personality control where applicable.

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

- if a boss pattern looks correct before Play but changes on startup, inspect the active runtime preset and roster chain first

---

## Runtime Authoring Rule

For runtime-authored boss content, the reliable chain is:

1. defense pattern
2. defense state window profile
3. preset
4. roster entry
5. level enemy selection
6. runtime slot routing
7. active runtime enemy state during Play

This is the real boss-authoring chain.

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

### 5. `meeting_tyrant_weak_window_boss`

Boss-variant roster entry created for Level 06.

Supports:

- dedicated preset
- dedicated defense pattern
- dedicated defense state window profile
- boss-style long-defense / short-window timing rule

---

## Level 04 Briefcase Boss

### Purpose

Validate the first true breaker-boss identity.

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

This is not just another block.  
It forces a two-step combat read:

1. read boss defense timing
2. use the breaker item
3. switch to the actual scoring item

---

## Level 06 Weak-Window Boss

### Purpose

Validate a third boss identity that is timing-based rather than breaker-based.

### Runtime authoring chain

- `meeting_tyrant_weak_window_boss_defense_pattern`
- `defense_state_window_meeting_tyrant_weak_window_boss`
- `enemy_preset_meeting_tyrant_weak_window_boss`
- `meeting_tyrant_weak_window_boss` roster entry
- `level_enemy_selection_level_06`

### Rule

The final validated Level 06 behavior is:

- defense stays active for most of the cycle
- regular attacks are blocked during that defended period
- only a short weakness window allows valid scoring head hits
- boss identity is driven by timing pressure, not breaker-item usage

### System meaning

Level 06 established an important runtime logic rule:

- `defenseActive` defines whether the boss currently has a defense gate
- `EnemyDefenseStateWindowController` defines when weakness is exposed
- weakness timing should not be allowed to globally disable defense outside the exposed window

This distinction is what enables a true long-defense / short-window boss.

---

## Enemy Debugging Rules

When enemy behavior appears wrong:

### Step 1

Confirm you selected the actual active runtime enemy root.

### Step 2

Inspect runtime fields during Play:

- current preset
- last applied preset
- defense pattern
- defense active
- runtime elapsed
- next timed activation
- defense state window profile
- current slot / active slot if switching is involved

### Step 3

If scene values changed on startup:

- inspect `EnemyPresetApplicator`
- inspect `EnemyPresetData`
- inspect the roster entry used by current level
- inspect the slot chosen by runtime routing

### Step 4

Only after data-chain validation, inspect controller code

### Step 5

After logic is correct, tune pacing:

- defense duration
- activation interval
- weak window normalized range

Because boss identity depends on pacing, not just binary correctness.

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
- stronger timing / punishment readability
- avoiding repeated generic enemies after Level 03

Not on:

- broad rewrites
- abandoning preset-driven runtime injection
- falling back to scene-only boss setup