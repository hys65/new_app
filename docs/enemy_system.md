# enemy_system.md

## Overview

Enemy behavior is now fully data-driven and runtime-routed.

Behavior definition stack:

1. `EnemyArchetypeData`
2. `EnemyDefensePatternData`
3. `EnemyAiProfileData`
4. `EnemyDefenseStateWindowProfileData`
5. `EnemyPresetData`
6. `EnemyRosterData`
7. `LevelEnemySelectionData`

Runtime authority comes from preset application, not scene-only tuning.

---

## Core Runtime Rule

Do not treat scene-edited defense values as authoritative boss configuration.

Actual runtime behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

This remains the project’s core enemy architecture rule.

---

## Enemy Defense Controller

`EnemyDefenseController` is the main gameplay gate for:

- defense active state
- timed activation
- repeated-hit activation
- block / break / weak evaluation
- cooldown handling
- defense-state-driven hit resolution

---

## Important Runtime Behavior

### Blocking Window

Defense is no longer interpreted as a single rigid boolean in practice.

For correct boss behavior, blocked evaluation now reflects:

- active defense state
- short defense exit grace window
- activation-time block enforcement when the triggering hit should already be considered defended

---

## Why This Changed

A pure `defenseActive` boolean produced two bad edge cases:

### Edge Case 1

Enemy still looked defensive, but defense had just turned off.

Result:

- player saw defense
- hit was not blocked

### Edge Case 2

Current hit triggered defense and showed `GUARD`, but still scored.

Result:

- player saw defense begin
- hit still counted

Both cases destroyed boundary clarity.

---

## Current Fix

### Defense Exit Block Grace

A short blocked grace window now persists briefly after defense ends.

Purpose:

- blocked evaluation window should be slightly stricter than the visual exit
- prevents “looks blocked but scores anyway” failures

### Block On Activation

If the current hit activates defense and the hit type should be blockable, that same hit is evaluated as blocked immediately.

Purpose:

- prevents “GUARD + successful score” on the same impact
- keeps boss-rule levels fair and readable

---

## Defense Readability Principle

For boss-rule encounters:

**blocked evaluation window must be at least as strict as visual defense presentation**

Never allow:

- obvious defense posture
- but permissive scoring result

Slightly strict is acceptable.
Visibly blocked but logically open is not.

---

## Weak Window Interaction

Weak-window head bypass still exists for weak-window style bosses.

Important constraint:

- weak-window bypass should only apply when defense is truly active and the state window explicitly exposes weakness
- zero-mistake rule content must not accidentally inherit permissive scoring from unrelated weak-window behavior
- later head-focused bosses must still rely on explicit authored window logic rather than vague visual timing

---

## Accepted Boss Variants in the Current Ladder

### Meeting Tyrant briefcase boss

Meaning:

- deterministic hard defense
- explicit break-route learning

### Meeting Tyrant weak-window boss

Meaning:

- short opening exploitation
- the player is rewarded for recognizing a brief scoring window

### Narcissist Manager sunglasses boss

Meaning:

- face-guard identity
- guarded face route is intentionally hostile to the wrong item choice

### Narcissist Manager Face Guard Boss

Meaning:

- head is intentionally low-value
- body is the primary reliable scoring route
- this is not just a repeat of the sunglasses boss

### Adaptive Shutdown Boss

Meaning:

- the boss punishes predictable throw rhythm
- the player is pushed to vary timing

### Head Hunter Boss

Meaning:

- the boss applies enough defensive pressure to make later scoring windows matter
- the player is pushed toward head-focused conversion rather than generic spam
- the identity is not a simple weak-window repeat
- the identity is not a simple anti-predictability repeat

---

## Level 11 Authoring Lesson

Level 11 proved an important production lesson:

A new boss identity does not require a new goal type if the current systems can produce a genuinely different player demand.

What mattered was not merely raising target values.
What mattered was establishing:

- stronger defensive presence
- a meaningful later scoring opportunity
- head-focused precision pressure
- a readable identity distinct from Levels 06, 09, and 10

---

## Enemy Production Discipline

For future boss authoring:

- first define the boss demand
- then author the behavior through the existing preset chain
- then validate runtime routing
- only after that consider final balancing

Do not solve weak identity by immediately proposing:

- new architecture
- new runtime injection paths
- new goal-type churn
- scene-only manual overrides