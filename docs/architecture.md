# architecture.md

## Overview

**Power Prank 3D** now runs on a layered gameplay architecture.

The project is no longer a loose prototype with one enemy and one win rule.

It is now a structured boss-reference game flow with:

- data-authored enemy behavior
- data-authored encounter goals
- runtime enemy routing
- runtime level progression
- player-facing rule language in HUD and result flow

The core architectural principle is:

**author behavior in data, apply it at runtime, then expose the rule clearly to the player**

---

## High-Level Runtime Stack

Current gameplay runs through these main layers:

1. **Throw Layer**
2. **Projectile Resolution Layer**
3. **Defense Evaluation Layer**
4. **Breakdown / Score Layer**
5. **Goal Evaluation Layer**
6. **Enemy Presentation Layer**
7. **HUD / Result Presentation Layer**
8. **Level Progression Layer**

These layers are already connected and should not be casually restructured.

---

## Core Gameplay Runtime Flow

### 1. Throw Layer

Primary runtime owner:

- `ThrowController`

Responsibilities:

- drag-based throw input
- UI-blocked input rejection
- item selection shortcuts
- per-item cooldown gating
- projectile spawn
- throw-force calculation
- aim preview
- trajectory preview

Important rule:

Throw permission is decided **before** projectile existence.

This is why item cooldown belongs here, not inside projectile collision code.

---

### 2. Projectile Resolution Layer

Primary runtime owner:

- `ProjectileBehavior`

Responsibilities:

- detect collision with enemy or ground
- determine head hit vs body hit
- find enemy runtime controllers
- evaluate defense result
- apply final score if allowed
- notify goal controller with final resolved hit data
- trigger popup / reaction / visual feedback
- spawn impact VFX / SFX / stain

Important rule:

A collision is not automatically a valid scoring hit.

A projectile must pass through defense evaluation first.

---

### 3. Defense Evaluation Layer

Primary runtime owner:

- `EnemyDefenseController`

Supporting owners:

- `EnemyDefenseStateWindowController`
- `EnemyAiLayerController`

Responsibilities:

- determine whether a hit is blocked
- determine whether a hit breaks defense
- determine whether a weak-window conversion applies
- manage defense cooldowns
- manage activation conditions
- enforce block-on-activation behavior
- preserve block readability near state edges

Important architectural meaning:

This layer is the **combat gate** between raw contact and final outcome.

Without it, boss-rule encounters collapse into generic hit trading.

---

## Why Defense Is Split Across Multiple Components

Enemy defense is intentionally not owned by a single giant monolith.

The split is:

### `EnemyDefensePatternData`
Defines authored defensive rule behavior.

Examples:

- can block head / body
- random block chance
- break-item permissions
- passive hit shaping
- weakness behavior

### `EnemyDefenseStateWindowProfileData`
Defines authored defense phase timing.

Examples:

- telegraph duration
- active duration
- recover duration
- weak-window start / end
- whether auto-cycle exists

### `EnemyAiProfileData`
Defines observed / predictive behavior.

Examples:

- how fast enemy reacts to repeated hits
- how much lead time is used
- how easily threat triggers defense
- recover lock behavior

### `EnemyDefenseController`
Combines pattern rules with live hit evaluation.

### `EnemyDefenseStateWindowController`
Owns the live current phase:

- `None`
- `Telegraph`
- `Active`
- `Recover`

### `EnemyAiLayerController`
Observes hit rhythm and decides when to trigger defense cycles.

This split is intentional.

It lets the project create different boss demands without inventing new systems every time.

---

## Score and Breakdown Layer

Primary runtime owner:

- `GameplayManager`

Responsibilities:

- current breakdown value
- target breakdown value
- timer
- selected item
- combo state
- round running state
- round finish state

Important rule:

`GameplayManager` owns breakdown and round state.

It does **not** own:

- throw gating
- defense evaluation
- goal-type-specific completion logic

Those are separate layers on purpose.

---

## Goal Evaluation Layer

Primary runtime owner:

- `LevelGoalController`

Data input:

- `LevelGoalDefinition`
- `LevelGoalType`
- `LevelEncounterConfigData`

Supported goal types:

1. `BreakdownTarget`
2. `HeadHitCount`
3. `SpecificItemHitCount`
4. `UnblockedHitStreak`

Responsibilities:

- apply encounter goal config
- track current progress
- interpret final resolved hits
- force win for non-breakdown goals when target is met
- provide goal summary text for review UI

Important rule:

Goal logic consumes **final post-defense hit state**.

It must not rely on naive collision assumptions.

That is why `CombatHitInfo` includes:

- `isHeadHit`
- `wasBlocked`
- `itemId`
- `gainedScore`

---

## Enemy Runtime Authoring Chain

This is the most important enemy architecture rule in the project.

Boss behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

### Layer meanings

#### `EnemyPresetData`
Bundles together:

- archetype
- defense pattern
- AI profile
- defense state window profile

#### `EnemyRosterData`
Adds:

- entry id
- preset reference
- recommended slot id

#### `LevelEnemySelectionData`
Chooses which roster entry a level uses.

#### `EnemySwitchingManager`
Routes the selected preset into the correct live slot.

#### `EnemyRuntimePresetController`
Applies the chosen preset to the live enemy object.

#### `EnemyPresetApplicator`
Pushes preset references into runtime controllers.

This chain is authoritative.

Do not rely on scene-only edits for final boss behavior.

---

## Level Runtime Flow

Primary runtime owners:

- `LevelEncounterController`
- `LevelProgressionController`

### `LevelEncounterController`

Responsibilities:

- apply encounter config
- apply round target / duration
- configure whether breakdown is the win condition
- apply enemy selection
- apply goal config

### `LevelProgressionController`

Responsibilities:

- apply startup level
- track current level index
- advance to next level
- restart current level
- support next-level flow after victory

Important rule:

Levels are not hardcoded combat states.

They are data-authored encounter packages.

---

## Presentation Architecture

Presentation is now split into two major branches:

1. **Enemy combat presentation**
2. **Player-facing rule presentation**

This split became much more important during the readability pass.

---

## Enemy Combat Presentation Branch

### `EnemyReactionLayerController`

Responsibilities:

- stage-based enemy reaction
- hit response intensity
- annoyance / agitation / meltdown progression

### `EnemyDefenseVisualLayerController`

Responsibilities:

- convert defense result into readable pose feedback
- separate `BLOCK`, `WEAK`, `BREAK`, and guard-related states
- apply body/head offsets and timed response layers
- bridge gameplay result into visible combat language

This component is no longer just “extra juice”.

It is now a core readability bridge:

**combat result → readable visual state**

### `EnemyVisualProxyController`

Responsibilities:

- lightweight proxy poses for primitive enemies
- rough telegraph / guard / break / weak silhouette support
- coarse pose-state differentiation

### Important architecture boundary

`EnemyVisualProxyController` is useful for:

- baseline readability
- quick primitive pose support

It is **not** a reliable final-art animation solution.

This boundary has now been tested in practice.

Do not overfit primitive proxy-arm rotations trying to force final hero-quality defensive poses.

That belongs to a later art/presentation phase.

---

## Player-Facing Rule Presentation Branch

Primary runtime owner:

- `HudController`

Responsibilities:

- live HUD display
- timer text
- selected item text
- combo UI
- result panel
- goal summary display

### Why this matters architecturally

Earlier project stages mostly treated goals as internal logic.

Current project state is different.

Now the goal system also has a **presentation contract**.

That means goal semantics must be readable in two places:

1. **during live play**
2. **during result review**

This is a real architecture fact now.

---

## HUD Rule Language Architecture

Current principle:

**HUD should explain the encounter rule, not just expose counters.**

Examples:

### BreakdownTarget
- `Goal: Build Breakdown X / Y`
- `Rule: Reach the breakdown target before time runs out`

### HeadHitCount
- `Goal: Land Head Hits X / Y`
- `Rule: Only successful head hits count`

### SpecificItemHitCount
- `Goal: Land [Item] Hits X / Y`
- `Rule: Only [Item] advances this goal`

### UnblockedHitStreak
- `Goal: Chain Clean Hits X / Y`
- `Rule: Any blocked hit resets the streak`

This means `HudController` is no longer just “stat display”.

It is now part of encounter explanation architecture.

---

## Result Panel Architecture

Current result panel responsibilities include:

- win / fail title
- status subtitle
- level label
- goal summary
- final-level completion notice
- retry / next-level actions

Important architecture rule:

Result-panel goal language should match live HUD goal language.

Do not let result review feel like a different system is explaining the encounter.

This is why `goalSummaryText` now belongs to the same language family as the HUD rule text.

---

## Combat Result Language Architecture

The project now has a first-pass readable combat result language split between:

- normal successful hit
- `BLOCK`
- `WEAK`
- `BREAK`

### Meaning of each result

#### Normal successful hit
Baseline scoring success.

#### `BLOCK`
The defense held.

Should feel:

- hard
- short
- stable
- denied

#### `WEAK`
The defense exposed a scoring opportunity.

Should feel:

- intentional
- rewarded
- readable as a conversion window

#### `BREAK`
The defense was opened.

Should feel:

- unstable
- broken
- more extended than block
- clearly different from normal hit

### Important design boundary

Not every boss supports every result.

For example:

- Level 11 intentionally has no break path
- therefore Level 11 is not the right encounter for evaluating `BREAK` readability

This is not just a tuning note.

It is an architecture / authoring truth.

---

## Boss Ladder as an Architectural Product

The current boss ladder is not just content.

It is proof that the architecture can produce multiple different encounter demands without new core-system churn.

### Current validated reference meanings

#### Level 04
Break-route learning

#### Level 05
Face-guard identity

#### Level 06
Weak-window timing

#### Level 07
Specific-item precision

#### Level 08
Zero-mistake clean chain

#### Level 09
Body-route judgment

#### Level 10
Anti-predictability pressure

#### Level 11
Later-window head conversion

This matters because future work should prefer:

- better authoring through the existing stack

instead of:

- inventing new systems too early

---

## Current Accepted System Boundaries

### Already strong enough

The current architecture is already strong enough for:

- multi-level encounter flow
- multiple goal types
- boss-specific enemy routing
- runtime preset application
- readable combat-result differentiation
- HUD / result rule explanation

### Not solved yet

The architecture does **not** yet claim:

- final art-grade enemy posing
- final polished combat presentation
- final 04–11 release balancing
- high-fidelity final enemy silhouette work

These are later-phase concerns.

Do not pretend they are architecture failures.

---

## Production Rules Going Forward

When extending the project:

### Prefer this order

1. define boss demand
2. author it through current data stack
3. verify runtime routing
4. verify readable player-facing rule language
5. verify readable combat-result language
6. only then consider final balance or later art polish

### Avoid this order

1. feel mild dissatisfaction
2. invent new architecture
3. add new goal type
4. add new injection path
5. rewrite working systems

That is the wrong development pattern for this project.

---

## Final Summary

The architecture of **Power Prank 3D** is now built around three linked truths:

### 1. Behavior is authored in data
Not in scene hacks.

### 2. Runtime application is authoritative
Not inspector wishful thinking.

### 3. Rules must be readable to the player
Not only correct in code.

That is the current architectural foundation of the project.
