# enemy_system.md

## Overview

Enemy behavior is fully data-driven and runtime-routed.

Behavior definition stack:

1. `EnemyArchetypeData`
2. `EnemyDefensePatternData`
3. `EnemyAiProfileData`
4. `EnemyDefenseStateWindowProfileData`
5. `EnemyDefenseVisualProfileData`
6. `EnemyPresetData`
7. `EnemyRosterData`
8. `LevelEnemySelectionData`

Runtime authority comes from preset application, not scene-only tuning.

---

## Core Runtime Rule

Do not treat scene-edited defense values as authoritative boss configuration.

Actual runtime behavior must be authored through:

**pattern → state window profile → visual profile → preset → roster entry → level selection → runtime slot routing**

This is now the project’s full enemy architecture rule.

Visual presentation authoring is part of that rule.

---

## Main Runtime Controllers

### EnemyDefenseController

`EnemyDefenseController` is the main gameplay gate for:

- defense active state
- timed activation
- repeated-hit activation
- block / break / weak evaluation
- cooldown handling
- defense-state-driven hit resolution

### EnemyDefenseStateWindowController

`EnemyDefenseStateWindowController` owns authored defense phases.

Current runtime states:

- `None`
- `Telegraph`
- `Active`
- `Recover`

It also controls:

- whether defense logic is currently active
- whether a weak window is currently open
- how long each phase lasts
- whether cycles are AI-driven, auto-cycled, or otherwise authored by profile

### EnemyAiLayerController

`EnemyAiLayerController` is the current behavior-reading layer.

It does not replace the authored defense pattern.

Instead, it influences **when** a defense cycle should begin based on:

- observed hit cadence
- current threat
- reaction stage
- recent head-hit pressure
- recover lock state

### EnemyDefenseVisualLayerController

`EnemyDefenseVisualLayerController` is the presentation-side bridge between defense result and readable combat feedback.

It receives and differentiates:

- `GUARD`
- `BLOCK`
- `BREAK`
- `WEAK`

It now also receives its authored profile through the runtime preset application chain.

This is now an important part of combat readability work.

### EnemyVisualProxyController

`EnemyVisualProxyController` is a lightweight proxy-pose layer for primitive enemy rigs.

It can support baseline readability, but it is **not** a reliable final-art posing solution on the current primitive enemy setup.

This limitation is now accepted and documented.

---

## Defense Visual Profile as Part of the Enemy System

This is now a real enemy-system fact.

### Current authored bundle

`EnemyPresetData` now includes:

- `EnemyArchetypeData archetype`
- `EnemyDefensePatternData defensePattern`
- `EnemyAiProfileData aiProfile`
- `EnemyDefenseStateWindowProfileData defenseStateWindowProfile`
- `EnemyDefenseVisualProfileData defenseVisualProfile`

### Current runtime application

`EnemyPresetApplicator` now applies the preset-owned visual profile into:

- `EnemyDefenseVisualLayerController`

### Why this matters

Boss identity is no longer authored only through hidden logic.

It is now also authored through preset-owned readable combat presentation.

This fixes an important production weakness:

- behavior no longer lives in data while visual readability drifts in the scene

That separation is no longer acceptable for boss content.

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

A pure `defenseActive` boolean produced two bad edge cases.

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

- prevents `GUARD + successful score` on the same impact
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

Important constraints:

- weak-window bypass should only apply when defense is truly active and the state window explicitly exposes weakness
- zero-mistake rule content must not accidentally inherit permissive scoring from unrelated weak-window behavior
- later head-focused bosses must still rely on explicit authored window logic rather than vague visual timing

Weak window is now treated as a readable combat-language event, not merely a hidden internal multiplier state.

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

### Narcissist Manager precision paint boss

Meaning:

- item-locked precision pressure
- correct item usage matters, but readable presentation still matters for timing and commitment

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

## Current Defense Visual Authoring Status

Defense visual profile authoring now exists at two levels:

### Baseline archetype level

Used for:

- generic archetype presentation
- non-boss or default content
- fallback presentation identity

### Boss level

Used for:

- boss-specific guard language
- boss-specific weak / break / block readability flavor
- keeping presentation ownership aligned with preset identity

Important rule:

Boss content should prefer boss-level defense visual profiles instead of falling back to generic archetype visual profiles whenever distinct boss readability is required.

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

### Important runtime note

Level 11 intentionally has **no break path** in its defense pattern.

This means:

- `BREAK` is not part of Level 11’s intended combat language
- Level 11 should not be used to judge break readability
- break readability sampling belongs on a boss with real break-item logic, such as Level 04

---

## Combat Result Language

Enemy combat now has a usable first-pass readable split between:

- normal successful hit
- `BLOCK`
- `WEAK`
- `BREAK`

### BLOCK

Target presentation meaning:

- defended
- hard
- short
- stable
- clearly “you did not convert this hit”

### WEAK

Target presentation meaning:

- exposed moment
- scoring opportunity
- rewarded timing
- clearly distinct from both block and break

### BREAK

Target presentation meaning:

- defense opened
- posture lost
- line broken
- more unstable and more extended than block

### Normal successful hit

Target presentation meaning:

- basic scoring success
- should remain readable
- should not visually overpower special result states

This result split is currently accepted at a prototype readability level.

It is not yet treated as final art polish.

---

## Enemy Visual Presentation Boundary

Current project status has now validated an important production boundary.

### What the proxy visual layer is good for

- baseline telegraph readability
- basic guard / weak / break differentiation
- lightweight pose-state support on primitive enemies

### What the proxy visual layer is **not** good for

- final hero-quality defensive posing
- reliable “cover the face” arm choreography on the current primitive rig
- high-confidence polish under awkward local pivot directions

### Current production rule

Do not waste time overfitting proxy arm rotations on the current primitive enemy setup.

If primitive pivot behavior fights intended defensive posing:

- stop forcing it
- keep baseline readability
- defer final pose polish to later art/presentation work

This is now an accepted rule, not a temporary guess.

---

## Enemy Production Discipline

For future boss authoring:

- first define the boss demand
- then author the behavior through the existing preset chain
- then validate runtime routing
- then validate readable player-facing combat language
- then validate runtime-applied defense visual identity
- only after that consider final balancing

Do not solve weak identity by immediately proposing:

- new architecture
- new runtime injection paths
- new goal-type churn
- scene-only manual overrides

Also do not solve primitive visual awkwardness by over-tuning temporary proxy poses.

That is low-value work unless the project has already entered final art polish.

---

## Current Accepted Enemy-System Lessons

### Runtime / Data lessons

- runtime preset application is authoritative
- roster routing and slot routing are part of actual behavior setup, not optional metadata
- defense visual profile ownership should live in presets, not scene leftovers
- GitHub asset verification remains necessary when inspector edits matter

### Gameplay lessons

- defense logic and hit resolution must stay tightly coupled
- blocked evaluation must remain stricter than misleading visuals
- goal logic must consume final hit resolution, not raw collision optimism

### Presentation lessons

- weak-window readability matters for real encounter identity
- result language (`BLOCK / WEAK / BREAK / normal hit`) should be readable without relying on logic knowledge
- primitive enemy visual rigs support baseline readability, not final presentation quality
- boss presentation now depends on both behavior tuning and preset-owned visual profile authoring

---

## Current Enemy-System Boundary

The enemy system is currently considered strong enough for:

- boss identity authoring
- runtime routing
- defense-rule differentiation
- goal-aware encounter design
- first-pass combat readability work
- preset-owned boss-level defense visual profile authoring

It is not currently being expanded toward:

- new enemy architecture branches
- new goal-type-driven enemy rewrites
- final art-grade defensive posing systems

That boundary should remain firm until later presentation work justifies reopening it.

---