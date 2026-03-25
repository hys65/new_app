# gameplay_systems.md

## Overview

Gameplay currently consists of four connected layers:

1. throw / projectile resolution
2. breakdown scoring
3. level goal evaluation
4. round / progression flow

The game is no longer using breakdown-only win conditions for all levels.
Goal evaluation now supports multiple encounter-specific success rules.

---

## Core Throw / Hit Flow

### Throw
Player throws the currently selected item.

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

## Design Lessons

### Bad Version
Player sees defense posture, but hit still scores.

Result:
- rule feels fake
- player loses trust

### Correct Version
Defense posture and blocked rule are tightly aligned.

Result:
- player understands what was wrong
- failure feels owned, not random

This alignment is now a hard requirement for future boss-rule content.