# GAMEPLAY SYSTEMS

## Overview

Gameplay is built around a prank-throwing core loop:

1. player selects / uses items
2. projectile hits enemy or ground
3. defense logic evaluates hit
4. reaction / popup / breakdown updates apply
5. goal progress updates apply
6. round win/loss resolves
7. result UI handles retry / next

---

## Core Gameplay Systems

### ThrowController
Owns projectile launch input and spawn usage.

### ProjectileBehavior
Owns projectile hit handling.

Key responsibilities:
- detect enemy vs ground hit
- query `EnemyDefenseController`
- convert defense result into breakdown contribution
- spawn hit popup
- report `CombatHitInfo` into `LevelGoalController`
- handle stain placement

### GameplayManager
Owns:
- round state
- breakdown values
- selected item state
- combo state
- finish / retry behavior

### LevelGoalController
Owns:
- current goal definition
- current progress
- goal completion
- summary text generation

### HudController
Owns:
- current top-left HUD state
- timer display
- selected item display
- goal-aware primary line display
- combo display
- result panel display

---

## Current Goal HUD Rule

HUD must reflect the real win condition.

### BreakdownTarget
Show breakdown-oriented HUD lines.

### HeadHitCount
Show `Head Hits: X / Y`

### SpecificItemHitCount
Show item-specific line such as `Tomato Hits: X / Y` or `Paint Ball Hits: X / Y`

Breakdown may still be shown as secondary combat information.

This change is complete and validated.

---

## Current Goal Types

### `BreakdownTarget`
Win by reaching target breakdown.

### `HeadHitCount`
Win by reaching valid head-hit count.

### `SpecificItemHitCount`
Win by reaching valid hit count with required item.

These three are implemented and working.

---

## Current Content Rule

Levels 01–03 are teaching levels.

After that, gameplay content should not expand by simple number reuse alone.

New levels must increasingly justify themselves through:
- new boss behavior
- new item relevance
- new break logic
- new player read

---

## Level 04 Gameplay Rule

Level 04 introduced the first gameplay rule where item choice materially matters.

### Briefcase guard active
- sponge hammer = break
- other items = blocked

This is the first proof that weapons can have roles beyond raw hit score.

---

## Level 05 Gameplay Rule

Level 05 expanded this design.

### Sunglasses face guard active
- paint ball = ineffective
- foam sprayer = break
- paint ball = real scoring item after break

This is the first validated level where:

- one item is the breaker
- another item is the actual goal item
- the player must execute a sequence, not just pick one weapon forever

---

## Known Gameplay-Side Debugging Lessons

1. If a blocked hit still gives score, inspect whether `wasBlocked` is actually being returned
2. If a pattern seems correct before Play but not during Play, inspect runtime preset overwrite first
3. If a goal seems wrong, verify current encounter config and current level selection asset
4. If a HUD line is misleading, fix display logic before changing underlying systems
5. For boss content, validate blocked-hit behavior and goal-hit behavior separately

---

## Current Recommended Gameplay Direction

The next gameplay step should be:

**Boss Defense Identity Expansion 2.0**

Meaning:
- keep teaching block stable
- preserve Level 04 and Level 05 as reference boss levels
- continue expanding item-specific meaning
- add more boss reads after Level 05
- avoid fake content repetition