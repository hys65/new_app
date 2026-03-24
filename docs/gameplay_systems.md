# GAMEPLAY SYSTEMS

## Overview

Gameplay is built around a prank-throwing core loop:

1. player selects / uses items
2. projectile hits enemy or ground
3. defense logic evaluates hit
4. reaction / popup / breakdown updates apply
5. goal progress updates apply
6. round win / loss resolves
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

Show item-specific line such as:

- `Tomato Hits: X / Y`
- `Paint Ball Hits: X / Y`

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
- new timing relevance
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

- paint = invalid
- foam = break tool
- paint after break = scoring item

This is the first proof that a boss can require a sequence:

1. break defense
2. switch into real scoring behavior

---

## Level 06 Gameplay Rule

Level 06 established a different kind of boss read.

### Weak-window timing boss

- boss is defended for most of the cycle
- ordinary attacks are blocked during that defended phase
- only a short vulnerability window allows valid head-hit progress
- scoring is driven by timing correctness, not by breaker-item choice

This is the first proof that the current gameplay architecture can support a timing boss without introducing a new goal type.

---

## Current Boss Gameplay Ladder

The validated boss ladder is now:

### Level 04

**Breaker boss**

- identify the guarded state
- use the correct breaker item
- wrong items are blocked

### Level 05

**Break-then-score boss**

- identify the guarded state
- use the breaker item first
- then switch to the actual scoring item

### Level 06

**Weak-window timing boss**

- recognize that the boss is mostly defended
- wait for a short valid scoring window
- land the correct timed head hit
- progress is earned by timing discipline

This ladder matters.  
Future boss levels should extend this language rather than repeat it.

---

## Current Gameplay Tuning Lesson

A boss level is not complete when logic merely works.

It is complete when:

- its rule is readable
- its pressure profile matches intent
- its goal type matches that rule
- its timing / item demand feels distinct from prior levels

Level 06 specifically proved that pacing values are gameplay-critical:

- defense duration
- activation interval
- weakness window range

Those values are not cosmetic.  
They are part of boss identity.

---

## Current Recommended Gameplay Direction

For Level 07+:

- continue distinct boss identity design
- avoid simple number escalation
- avoid repeating Level 04 / 05 / 06 structures with renamed assets
- strengthen either punishment logic, denial logic, or counter-play logic

Good future direction:

- a boss where wrong timing is punished
- a boss where wrong item choice creates counter-pressure
- a boss where restraint matters as much as aggression

Bad future direction:

- another mostly-open enemy with decorative defense
- another simple breaker swap
- another copy of Level 06 with only shorter numbers