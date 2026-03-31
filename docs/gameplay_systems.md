# gameplay_systems.md

## Overview

Gameplay currently consists of six connected layers:

1. throw input and throw gating
2. projectile resolution
3. defense evaluation
4. breakdown scoring
5. level goal evaluation
6. round / progression flow

The game is no longer using breakdown-only win conditions for all levels.

Goal evaluation now supports multiple encounter-specific success rules.

Throw pacing is also no longer effectively unrestricted.

Each item can now define its own cooldown.

Current presentation work has also pushed gameplay display away from raw counters and toward clearer rule-facing language.

---

## Core Throw / Hit Flow

### Throw Input and Gating

Player throws the currently selected item through `ThrowController`.

Before a throw is accepted, the system checks:

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

## Defense Evaluation Layer

Defense evaluation happens before final score and before goal progress.

This layer is now one of the core gameplay gates.

### Runtime authority

`EnemyDefenseController` evaluates the hit and produces a `DefenseHitResult`.

That result can mark a hit as:

- blocked
- break-defense success
- weak-window success
- normal pass-through hit

### Practical meaning

A collision is not automatically a valid hit.

A projectile can visibly collide with the enemy and still fail to count for the current goal if defense evaluation rejects or reshapes the hit.

This is critical for boss-rule encounters.

### Important production lesson

Goal systems must consume final hit resolution, not visual contact assumptions.

This is already validated in live encounter design.

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

`ProjectileBehavior` sends blocked state into goal evaluation.

Practical effect:

- a visually successful collision is not automatically a valid scoring hit
- if defense evaluation marks the hit as blocked, zero-mistake goals can reset immediately

This avoids false progress on boss-rule encounters.

---

## HUD Goal Presentation

HUD now explicitly branches per goal type and presents the active encounter as a rule, not just a raw counter.

This is a major readability shift.

### BreakdownTarget

HUD now shows:

- `Goal: Build Breakdown X / Y`
- `Rule: Reach the breakdown target before time runs out`

### HeadHitCount

HUD now shows:

- `Goal: Land Head Hits X / Y`
- `Rule: Only successful head hits count`

### SpecificItemHitCount

HUD now shows:

- `Goal: Land [Required Item] Hits X / Y`
- `Rule: Only [Required Item] advances this goal`

### UnblockedHitStreak

HUD now shows:

- `Goal: Chain Clean Hits X / Y`
- `Rule: Any blocked hit resets the streak`

### Why this changed

Older HUD behavior exposed counters but did not clearly state encounter demand.

Current behavior is intentionally more explicit.

The HUD should answer:

- what do I need to do
- what kind of hit counts
- what kind of mistake fails the rule

This is more valuable for boss encounters than generic score display.

---

## Result Panel Goal Summary

Result panel goal summary now uses the same goal-language family as the live HUD.

This keeps success / failure review aligned with live encounter rules.

### Current result summary language

#### BreakdownTarget

- `Goal: Build Breakdown X / Y`

#### HeadHitCount

- `Goal: Land Head Hits X / Y`

#### SpecificItemHitCount

- `Goal: Land [Required Item] Hits X / Y`

#### UnblockedHitStreak

- `Goal: Chain Clean Hits X / Y`

### Practical effect

Player sees the same rule framing:

- during the round
- at the end of the round

This reduces confusion and prevents the result screen from sounding like a different system.

---

## Combat Result Language

Combat now has a usable first-pass result language split between:

- normal successful hit
- `BLOCK`
- `WEAK`
- `BREAK`

### Normal successful hit

Represents a valid scoring hit without special defensive override.

This should feel like the baseline combat success state.

### BLOCK

Represents a defended hit that did not count as a valid scoring success for the relevant rule.

Presentation target:

- hard
- short
- stable
- clearly “guarded”

### WEAK

Represents a valid hit during a weakness / scoring window.

Presentation target:

- opportunity
- exposure
- rewarded timing
- distinct from both block and break

### BREAK

Represents a defense-break event.

Presentation target:

- defense opened
- posture lost
- longer and more unstable than block
- clearly distinct from ordinary hit feedback

### Important Level 11 note

Level 11 intentionally has no break path in its defense pattern.

That means Level 11 should not be used to judge `BREAK` readability.

`BREAK` readability must be sampled from a boss that actually supports break-item logic, such as Level 04.

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

## Encounter-Facing Gameplay Readability

Current gameplay presentation now prioritizes decision clarity over raw stat exposure.

### What the player should understand quickly

- what the goal is
- what type of hit counts
- what type of mistake is punished
- whether the result was block / weak / break / normal score

### What is explicitly not treated as solved yet

Final art-grade defensive posing is not considered finished.

Current primitive proxy posing is accepted only as baseline readability support.

Do not confuse:

- acceptable prototype readability
with
- final presentation quality

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

## Level 11 Gameplay Rule

Level 11 is the clearest current use of late-window head-pressure language.

Validated rule set:

- only successful head hits count toward the goal
- breakdown still exists as secondary combat context
- weak-window readability matters more than raw breakdown pressure
- break readability is not part of this encounter because break paths are intentionally disabled

This encounter is not about:

- item restriction
- body-route optimization
- anti-predictability pressure

It is about:

**later scoring windows and head-focused conversion**

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

---

## Current Presentation-Pass Boundary

The current pass is about:

- clearer rule language
- clearer encounter demand
- clearer hit-result differentiation

The current pass is **not** about:

- final art-grade posing
- final release-balance tuning
- reopening deep stain-system work
- overfitting primitive model pivots into hero-quality animations

This boundary should be kept firm.

---