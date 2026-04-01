# AI_CONTEXT.md

## Project Identity

**Power Prank 3D**

Unity 6.3 LTS

Single-scene prototype evolved into a structured multi-level boss-reference project.

The project is no longer in early core-loop construction.

The project is now in a later prototype phase where:

- core systems already exist
- boss identities through the current validated ladder already exist
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
- preset-driven defense visual profile routing
- boss presentation consistency pass for active Narcissist Manager boss levels
- runtime validation that active enemies receive correct preset + state-window + visual-profile pairing

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

Validated boss-reference levels currently include:

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

Validated presentation ownership:
- independent boss preset verified at runtime
- boss state window verified at runtime
- boss visual profile verified at runtime

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

Validated presentation ownership:
- independent boss preset verified at runtime
- independent boss state window verified at runtime
- independent boss visual profile verified at runtime

### Level 08
Zero-Mistake Boss

Meaning:
- blocked hit resets progress
- timing discipline and clean conversion

Validated rule:
- `UnblockedHitStreak`
- target count = `6`
- round duration = `32`

Validated ownership update:
- no longer left on base `narcissist_manager` ownership
- now routed through independent `enemy_preset_zero_mistake_boss`
- now routed through independent `zero_mistake_boss` roster ownership

Validated presentation ownership:
- runtime preset verified
- runtime state window verified
- runtime visual profile verified

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

Validated authoring fix:
- previous state-window drift into `precision_paint_boss` authoring was corrected
- face-guard boss now owns independent `defense_state_window_narcissist_face_guard_boss`

Validated presentation ownership:
- runtime preset verified
- runtime state window verified
- runtime visual profile verified

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

**pattern → state window profile → visual profile → preset → roster entry → level selection → runtime slot routing**

Runtime preset application is authoritative.

Scene-only manual tuning is not authoritative boss behavior.

---

## Current Defense Visual Authoring Rule

Boss presentation is no longer only an archetype-level concern.

`EnemyPresetData` now includes:

- `archetype`
- `defensePattern`
- `aiProfile`
- `defenseStateWindowProfile`
- `defenseVisualProfile`

`EnemyPresetApplicator` now applies the defense visual profile into:

- `EnemyDefenseVisualLayerController`

This means combat presentation authoring is now part of the same preset runtime chain as behavior authoring.

Important rule:

Do not treat `EnemyDefenseVisualLayerController.visualProfile` as a scene-only manual style setting.

It is now part of authored preset identity.

---

## Current Readability / Presentation Status

The project has already moved into:

**Combat Readability / Boss Presentation Pass**

This pass is no longer only about HUD wording.

It now includes:

- readable combat-result language
- readable rule-facing HUD language
- boss-level defense visual profile authoring through preset routing
- boss-presentation consistency validation through runtime inspection
- cleanup of authoring drift between preset identity and state/visual ownership

### Already achieved

- weak-window readability improved to an acceptable gameplay level
- HUD goal language rewritten from raw counters to rule-facing text
- result panel goal summary aligned with the same rule language
- block / weak / break / normal-hit readability inspected through runtime sampling
- block and break differentiation pushed further apart
- preset-driven boss-level defense visual profile routing implemented and runtime-validated
- boss-level visual profile assets created and bound for current active boss presentation work
- Level 05 / 07 / 08 / 09 runtime presentation ownership inspected and validated
- Level 08 received independent boss preset / roster ownership
- Level 09 received independent face-guard state-window ownership

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

HUD should explain the encounter rule, not just display counters.

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
- keep defense visual authoring inside preset-driven asset ownership rather than scene drift
- when boss presentation drift is found, fix the asset ownership chain instead of patching runtime behavior first

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

- treat the current boss-presentation consistency work for Levels 05 / 07 / 08 / 09 as closed
- preserve the current boss ladder
- keep defense visual authoring inside the preset chain
- avoid premature final balancing
- avoid low-value micro-polish on temporary primitive visuals
- update docs whenever asset ownership changes materially

This is the correct continuation line unless newer runtime evidence proves otherwise.
