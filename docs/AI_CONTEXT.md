# AI_CONTEXT.md

## Project Identity

**Power Prank 3D**

Unity 6.3 LTS

Single-scene prototype evolved into a structured multi-level boss-reference project.

The project is no longer in early core-loop construction.

The project is now in a later prototype phase where:

- core systems already exist
- boss identities through Level 11 already exist
- runtime routing already exists
- current work is focused on readability, presentation clarity, and production-safe continuation

---

## Current Repository Truth

When docs and code differ, treat **actual current code and runtime structure** as source of truth.

Do not rely on old chat summaries.

Important confirmed runtime stack:

- `GameplayManager`
- `LevelEncounterController`
- `LevelProgressionController`
- `LevelGoalController`
- `EnemySwitchingManager`
- `EnemyRuntimePresetController`
- `EnemyPresetApplicator`
- `EnemyDefenseController`
- `EnemyDefenseStateWindowController`
- `EnemyAiLayerController`
- `EnemyDefenseVisualLayerController`
- `EnemyVisualProxyController`

Important confirmed goal types:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`
- `UnblockedHitStreak`

Do not invent new goal types casually.

---

## Current Completed Systems

The following are already implemented and treated as real completed project ground:

### Core Gameplay

- core throw / hit / breakdown gameplay loop
- per-item throw cooldown pacing
- projectile hit resolution through final defense evaluation
- combo support
- goal-aware hit routing

### Enemy Systems

- enemy reaction layer
- enemy defense visual layer
- enemy archetype system
- enemy AI layer
- enemy switching system
- roster / level enemy selection
- preset-driven runtime behavior routing

### Level Systems

- level encounter configuration
- level progression / multi-level flow
- runtime level advance
- victory choice flow
- result panel polish
- level goal variety

### Repository / Production Discipline

- canonical asset-layout cleanup
- canonical script-folder cleanup
- runtime-first debugging discipline
- GitHub verification habit for important serialized values

---

## Current Boss Ladder Status

Levels 01–03 remain early teaching levels.

Levels 04–11 are the validated boss-reference ladder.

### Level 04
Meeting Tyrant briefcase boss

Meaning:
- deterministic hard defense
- explicit break-route learning

### Level 05
Narcissist Manager sunglasses boss

Meaning:
- face-guard identity
- guarded face route is hostile to the wrong item choice

### Level 06
Meeting Tyrant weak-window boss

Meaning:
- short opening exploitation
- window-timing pressure

### Level 07
Narcissist Manager precision paint boss

Meaning:
- specific item precision requirement

Validated rule:
- `SpecificItemHitCount`
- required item = `item_paint_ball`
- target count = `10`
- round duration = `32`

### Level 08
Zero-Mistake Boss

Meaning:
- blocked hit resets progress
- timing discipline and clean conversion

Validated rule:
- `UnblockedHitStreak`
- target count = `6`
- round duration = `32`

### Level 09
Narcissist Manager Face Guard Boss

Meaning:
- head is intentionally low-value
- body is the main reliable route
- pressure comes from hit-zone judgment

Validated rule:
- `BreakdownTarget`
- target breakdown = `180`
- round duration = `34`

### Level 10
Adaptive Shutdown Boss

Meaning:
- predictable rhythm is punished
- mixed rhythm improves efficiency
- pressure comes from anti-predictability

Validated runtime result:
- fixed rhythm gets blocked materially more often than mixed rhythm

### Level 11
Head Hunter Boss

Meaning:
- later scoring opportunity
- head-focused conversion pressure
- distinct from Level 06 weak-window pressure
- distinct from Level 09 body-route pressure
- distinct from Level 10 anti-predictability pressure

Validated rule:
- `HeadHitCount`
- target count = `7`
- target breakdown = `160`
- round duration = `38`

Important Level 11 truth:
- the pattern intentionally has **no break path**
- do not use Level 11 to sample `BREAK` readability

---

## Runtime Authority Rule

Boss behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

Do not trust scene-only manual tuning as final boss behavior.

Runtime preset application is authoritative.

---

## Current Readability / Presentation Status

The project has already moved into:

**Combat Readability / Boss Presentation Pass**

This pass is in progress and has already advanced materially.

### Already achieved

- weak-window readability improved to an acceptable gameplay level
- HUD goal language rewritten from raw counters to rule-facing text
- result panel goal summary aligned with the same rule language
- block / weak / break / normal-hit readability inspected through runtime sampling
- block and break differentiation pushed further apart
- result panel hierarchy reviewed and accepted for current prototype phase

### Current readable result-language split

Prototype-level readable split now exists between:

- normal successful hit
- `BLOCK`
- `WEAK`
- `BREAK`

This is accepted as prototype readability success.

It is **not** final art polish.

---

## Important Accepted Limitations

### 1. Primitive proxy-arm posing is limited

The current primitive enemy rig and local pivot directions make proxy-arm overfitting low-value.

Important rule:

- do not keep wasting time forcing hero-quality “cover the face” guard posing on the current primitive setup
- use the proxy layer for baseline readability only
- defer final art-grade defensive pose polish to later art/presentation work

### 2. Head stain visuals are still imperfect

Especially on current sphere-head setup.

Accepted rule:

- do not reopen deep stain-system work unless it becomes a true blocker

### 3. Full release balancing is intentionally postponed

Do not treat the current stage as final release-balance tuning.

Correct order remains:

1. keep boss identities valid
2. continue readability / presentation work
3. do final 04–11 balancing later, closer to release

---

## Current Gameplay Presentation Rules

HUD should now explain the encounter rule, not just display counters.

Current desired language style:

### BreakdownTarget
- Goal: Build Breakdown X / Y
- Rule: Reach the breakdown target before time runs out

### HeadHitCount
- Goal: Land Head Hits X / Y
- Rule: Only successful head hits count

### SpecificItemHitCount
- Goal: Land [Item] Hits X / Y
- Rule: Only [Item] advances this goal

### UnblockedHitStreak
- Goal: Chain Clean Hits X / Y
- Rule: Any blocked hit resets the streak

Result panel goal summary should mirror the same rule family.

Do not let live HUD and result review sound like different systems.

---

## Current Engineering / Design Discipline

When continuing work:

- validate runtime behavior, not just inspector assumptions
- validate GitHub asset contents after important serialized changes
- prefer content authoring through current data systems before proposing code churn
- reject fake complexity when existing systems already support the required boss demand
- evaluate readability alongside logic correctness
- stop low-value visual micro-tuning when it starts fighting primitive rig limitations

---

## What Not To Reopen Casually

Do not casually reopen:

- finished boss identity levels
- new goal-type invention
- deep stain-system work
- full release balancing
- proxy-arm overfitting for primitive rigs
- architecture churn just because a boss feels imperfect before final art polish

Only reopen these if actual runtime evidence proves a real blocker.

---

## Best Next-Step Direction

If continuing from this project state, the safest direction is:

- continue the broader `Combat Readability / Boss Presentation Pass`
- keep improvements focused on decision clarity
- preserve the current boss ladder
- avoid premature final balancing
- avoid low-value micro-polish on temporary primitive visuals

This is the correct continuation line unless newer runtime evidence proves otherwise.
