# gameplay_systems.md

## Overview

Gameplay currently consists of five connected layers:

1. throw input and throw gating
2. projectile resolution
3. breakdown scoring
4. level goal evaluation
5. round / progression flow

The game is no longer using breakdown-only win conditions for all levels.
Goal evaluation now supports multiple encounter-specific success rules.

Throw pacing is also no longer effectively unrestricted.
Each item can now define its own cooldown.

---

## Core Throw / Hit Flow

### Throw Input and Gating

Player throws the currently selected item through `ThrowController`.

Before a throw is accepted, the system now checks:

- round must be running
- input must not start over UI
- item must be available
- item cooldown must allow a new throw
- drag distance must pass throw threshold

Practical effect:

- current item choice matters not only for damage / feedback
- it also matters for throw rhythm
- future balancing can use pacing as part of item identity

### Throw

If input is valid, player throws the currently selected item.

Cooldown is consumed only after the throw is actually spawned.

This is important.

A failed drag or cancelled input should not burn cooldown.

### Collision

`ProjectileBehavior` resolves collision with:

- enemy
- ground

### Enemy Hit Resolution

If projectile hits the enemy, hit processing flows through:

1. head / body hit detection
2. defense evaluation
3. breakdown application
4. goal notification
5. reaction / visual feedback
6. popup display

This means goal logic now consumes **final resolved hit state**, not raw contact alone.

---

## Throw Cooldown System

Per-item cooldown is data-driven.

### Data Ownership

Cooldown value belongs to:

- `GameplayItemData`

This keeps throw pacing aligned with the existing item-data architecture.

### Runtime Ownership

Cooldown permission and consumption belong to:

- `ThrowController`

This is the correct architectural boundary because throw permission is decided before projectile existence.

### Important Rule

Do not move cooldown logic into:

- `ProjectileBehavior`
- collision callbacks
- post-hit scoring logic

That would be too late and would mix pacing with hit resolution.

### Cleanup Rule

Throw cooldown runtime state must be cleared when the round ends.

This prevents one round’s pacing state from leaking into the next.

---

## Breakdown System

`GameplayManager` remains the central authority for:

- current breakdown
- target breakdown
- round timer
- selected item
- combo state
- round finish state

### Notes

- Breakdown may still be accumulated during non-breakdown-goal levels.
- Not every level uses breakdown as the actual win condition.
- Breakdown HUD remains useful as secondary combat feedback.
- `GameplayManager` is not the owner of throw-cooldown gating.

---

## Goal System

## Supported Goal Types

### 1. BreakdownTarget
Win by reaching target breakdown value.

### 2. HeadHitCount
Win by landing enough successful head hits.

Rules:
- hit must gain score
- hit must be head-tagged

### 3. SpecificItemHitCount
Win by landing enough successful hits from a required item id.

Rules:
- hit must gain score
- `itemId` must match required item id exactly

### 4. UnblockedHitStreak
Win by landing a streak of successful **unblocked** hits.

Rules:
- successful unblocked hit: progress +1
- blocked hit: progress resets to 0
- progress must reach target count in one clean chain

This goal type exists for boss encounters that punish incorrect timing instead of merely reducing score.

---

## CombatHitInfo

`CombatHitInfo` now carries:

- `isHeadHit`
- `wasBlocked`
- `itemId`
- `gainedScore`

This is critical.

Goal logic can only behave correctly if it receives final post-defense hit information.

Without `wasBlocked`, zero-mistake goals cannot work correctly.

---

## Projectile Resolution Rule

`ProjectileBehavior` now sends blocked state into goal evaluation.

Practical effect:
- a visually successful collision is not automatically a valid scoring hit
- if defense evaluation marks the hit as blocked, zero-mistake goals can reset immediately

This avoids false progress on boss-rule encounters.

---

## HUD Goal Presentation

HUD must explicitly branch per goal type.

Current HUD behavior:

### BreakdownTarget
- shows current breakdown / target breakdown

### HeadHitCount
- shows head-hit progress
- also shows current breakdown as secondary info

### SpecificItemHitCount
- shows required item progress
- also shows current breakdown as secondary info

### UnblockedHitStreak
- shows clean hit progress
- shows “Blocked Hit = Reset”

This is intentional.
Zero-mistake levels need rule clarity more than generic score display.

---

## Round Completion Rules

### BreakdownTarget
Round ends in win when breakdown target is reached.

### Non-breakdown goal types
`LevelGoalController` can force round completion on success.

This is already validated for:
- HeadHitCount
- SpecificItemHitCount
- UnblockedHitStreak

---

## Level 08 Gameplay Rule

Level 08 is the first full use of `UnblockedHitStreak`.

Validated rule set:

- only non-blocked successful hits count
- blocked hit immediately resets current streak
- target streak = 6
- round duration = 32 seconds

This encounter is not about raw damage, item choice, or short burst output.

It is about:
**timing discipline and zero-mistake execution**

---

## Combat Pacing Design Lessons

### Bad Version
Player can throw at near-unlimited rate.

Result:
- boss timing can be brute-forced
- item balance becomes blurry
- encounter pressure loses meaning

### Correct Version
Each item has its own authored throw rhythm.

Result:
- pacing becomes part of item identity
- boss windows are more trustworthy
- future balancing has a stable baseline

### Architecture Lesson
Per-item throw cooldown is correct for the current project because:

- items are already data-driven
- throw permission is already centralized
- it adds pacing without rewriting scoring or enemy systems

This alignment is now a hard requirement for future content balancing.