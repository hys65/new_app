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

---

## Face Guard Expansion

Level 09 established a new accepted Narcissist Manager boss variant:

**Face Guard Boss**

This variant is not a simple repeat of Level 05.

Its purpose is different:

- Level 05 focuses on face-guard identity plus paint invalidation while guarded
- Level 09 focuses on hit-zone judgment pressure

Design meaning of Level 09:

- head hits are intentionally low-value over time
- body hits are the primary reliable scoring route
- the encounter teaches “do not greed for the face” rather than “use one specific item”

This is an important boss-reference distinction.

---

## Geometry and Hit-Zone Constraint

A boss encounter built around hit-zone judgment depends on readable enemy shape.

This means:

- head and body must be visually separable
- body must be a stable main target
- silhouette matters as much as defense logic

A correct defense pattern can still fail as an encounter if:

- the head dominates the silhouette
- the body is too small to be a reliable target
- the current throw style makes body targeting feel random

This was a real production lesson during Level 09 iteration.

---

## Stain Visual Constraint

Enemy stain visuals are currently acceptable on flat or broad surfaces.

However:

- stain visuals are not fully trustworthy on the current sphere-head setup
- a flat quad stain cannot perfectly conform to a curved head surface
- head stain polish is currently not treated as a blocker

Important rule:
- do not reopen deep stain-system work unless it becomes necessary for a later milestone
- current priority remains gameplay clarity and pacing, not perfect curved-surface stain projection

---

## Boss Reference Identities

### Level 04 – Meeting Tyrant Briefcase Boss
Identity:
- deterministic hard block
- explicit break item logic

### Level 05 – Narcissist Manager Sunglasses Boss
Identity:
- face guard
- paint invalid while guarded

### Level 06 – Meeting Tyrant Weak-Window Boss
Identity:
- short vulnerability windows
- pressure through timing bursts

### Level 07 – Narcissist Manager Precision Paint Boss
Identity:
- required-item success rule
- paint-ball precision objective

### Level 08 – Zero-Mistake Boss
Identity:
- clean-hit streak objective
- blocked hit resets progress
- boundary clarity is part of the encounter design

### Level 09 – Narcissist Manager Face Guard Boss
Identity:
- head is intentionally low-value
- body is the primary scoring route
- player must learn zone choice rather than greed for face hits

---

## Production Lessons

- Wrong slot routing can make a valid boss appear absent.
- Runtime preset application is the source of truth.
- Defense visuals and block logic must remain aligned.
- A boss encounter can fail not because of numbers, but because of unreadable boundaries.
- Future boss-rule content should be evaluated first on clarity, second on difficulty.
- Geometry and control feel can invalidate a theoretically good boss rule.