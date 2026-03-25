# PROJECT STATE

## Project

**Power Prank 3D**

Unity 6.3 LTS small-scale prototype game.

Core fantasy:

- throw prank items at enemy bosses
- build breakdown pressure
- trigger defensive behaviors
- read boss identity
- use the right item or timing
- clear goal-driven encounters

---

## Current Milestone Status

### Completed Milestones

- Core throw / hit / breakdown gameplay loop ✅
- Enemy Reaction Layer 1.0 ✅
- Enemy Defense Visual Layer 1.0 ✅
- Enemy Archetype System ✅
- Enemy AI Layer 1.0 ✅
- Enemy Switching System 1.0 ✅
- Enemy Roster / Level Enemy Selection 1.0 ✅
- Level Content / Encounter Configuration 1.0 ✅
- Level Progression / Multi-Level Content 1.0 ✅
- Runtime Level Advance 1.0 ✅
- Victory Choice Flow 1.0 ✅
- Result Panel Polish 1.0 ✅
- Level Goal Variety 1.0 ✅
- Enemy gameplay hitbox structure repair ✅
- Enemy stain attachment repair ✅
- Goal-aware HUD Readability 1.0 ✅
- Boss Preset Override Debugging Pass ✅
- Level 04 Briefcase Boss Foundation ✅
- Level 05 Sunglasses Boss Foundation ✅
- Level 06 Weak-Window Boss Foundation ✅
- Level 07 Precision Paint Boss Foundation ✅

---

## Current Goal System Status

Implemented and validated:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`

Validated teaching flow:

- Level 01 -> `BreakdownTarget`
- Level 02 -> `HeadHitCount`
- Level 03 -> `SpecificItemHitCount(item_egg)`

Validated boss flow:

- Level 04 -> `BreakdownTarget` + briefcase boss rule
- Level 05 -> `SpecificItemHitCount(item_paint_ball)` + sunglasses boss rule
- Level 06 -> `HeadHitCount` + weak-window boss rule
- Level 07 -> `SpecificItemHitCount(item_paint_ball)` + precision paint boss rule

Current note:

These three goal types remain sufficient for the current production content set.

Level 07 proved that the same goal type can support a different boss identity when defense logic, scoring restriction, and item loop are meaningfully different.

---

## Current Enemy System Status

Enemy behavior remains data-driven.

Main data stack:

- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`
- `EnemyPresetData`

Runtime application path:

- `EnemyRosterData`
- `LevelEnemySelectionData`
- `EnemySwitchingManager`
- `EnemyRuntimePresetController`
- `EnemyPresetApplicator`

Important fact:

Runtime preset application can overwrite scene-level defense pattern and defense window configuration.

Boss behavior must be authored through the actual preset used by the selected roster entry.

---

## Current Known Working Enemy Content

### Meeting Tyrant

- base archetype exists
- base preset exists
- briefcase boss preset path exists
- briefcase boss defense pattern exists
- weak-window boss preset path exists
- weak-window boss defense pattern exists
- Level 04 uses Meeting Tyrant as a validated breaker boss encounter
- Level 06 uses Meeting Tyrant as a validated timing / weak-window boss encounter

### Narcissist Manager

- base archetype exists
- base preset exists
- sunglasses boss preset path exists
- sunglasses boss defense pattern exists
- precision paint boss preset path exists
- precision paint boss defense pattern exists
- Level 05 uses this as a validated break-then-score boss encounter
- Level 07 uses this as a validated repeat precision-loop boss encounter

---

## Current Content Structure

### Tutorial Levels

- Level 01 = tutorial breakdown
- Level 02 = tutorial head hits
- Level 03 = tutorial specific item

### Boss Block

- Level 04 = first distinct boss identity level
  - identity: Briefcase Boss / Hammer Break rule
  - implementation uses dedicated roster entry + dedicated preset path

- Level 05 = second distinct boss identity level
  - identity: Sunglasses Boss / Foam Break / Paint Finish rule
  - implementation uses dedicated roster entry + dedicated preset path

- Level 06 = third distinct boss identity level
  - identity: Weak-Window Boss / Long Defense / Short Timing Window rule
  - implementation uses dedicated roster entry + dedicated preset path
  - primary goal uses `HeadHitCount`

- Level 07 = fourth distinct boss identity level
  - identity: Precision Paint Boss / Repeated Foam Break / Paint Head Score rule
  - implementation uses dedicated roster entry + dedicated preset path
  - primary goal uses `SpecificItemHitCount(item_paint_ball)`

### Extended Content

- Levels 08–09 still exist in progression content
- these levels should continue boss-first content expansion
- they should not regress into fake repeated encounters

---

## Current Major Design Rule

Do NOT treat Levels 04+ as simple repeated content rotation.

Invalid direction:

- repeat breakdown / head-hit / specific-item loops with only numeric escalation

Required direction:

- each boss level must have a distinct defense identity
- each boss level must create a distinct item usage pattern or timing pattern
- boss identity takes priority over raw goal repetition

---

## Current Technical Debug Lessons

### Preset Override Lesson

Scene-level edits to `EnemyDefenseController.defensePattern` are not reliable if runtime preset application is active.

Correct debug order:

1. level enemy selection
2. roster entry
3. switching manager target slot
4. runtime preset controller
5. preset applicator
6. scene object final state

### Slot Targeting Lesson

For multi-slot runtime setups, a correct preset can still fail to produce the intended gameplay if it is applied to the wrong slot / wrong active enemy root.

Correct question:

- not only “is the preset valid?”
- but also “did the runtime route this preset into the intended active slot?”

### Weak-Window Defense Lesson

Level 06 established an important logic distinction:

- `defenseActive` determines whether defense exists
- `DefenseStateWindow` determines when weakness is exposed
- defense should not be globally disabled just because the state window was outside the weak phase

### Asset Serialization Lesson

Unity Inspector changes are not always immediately reflected in the on-disk `.asset` file.

Correct workflow:

1. edit asset values
2. explicitly save
3. confirm the file actually changed in source control
4. verify GitHub file contents directly when the exact serialized value matters

---

## Current Validated Boss Rules

### Level 04

- timed briefcase guard
- sponge hammer breaks defense
- non-hammer items are blocked

### Level 05

- timed sunglasses face guard
- paint is ineffective while defense is active
- foam breaks defense
- paint becomes the real scoring item after break
- level goal uses `SpecificItemHitCount(item_paint_ball)`

### Level 06

- Meeting Tyrant weak-window boss
- long-duration defense is active for most of the cycle
- general attacks are blocked during defense
- only a short weak window allows valid head-hit scoring
- level goal uses `HeadHitCount`
- final tuning intentionally favors “mostly defended, briefly vulnerable”

### Level 07

- Narcissist Manager precision paint boss
- repeated sunglasses defense cycling controls when the head is meaningfully scoreable
- foam is the practical breaker item
- paint ball is the required scoring item
- repeated paint-head scoring loop is the intended mastery pattern
- level goal uses `SpecificItemHitCount(item_paint_ball)`
- final tuning uses `TargetCount = 10` and `RoundDurationSeconds = 32`

---

## Current Recommended Next Step

**Level 08 Boss Identity design and implementation**

High-level direction:

- preserve Levels 04–07 as reference implementations
- do not redesign finished boss levels without proof of regression
- create a fifth boss identity that is not just another breaker or weak-window clone
- continue using the preset-authoritative runtime path

---

## Current Red Flags To Avoid

- Do not bypass `EnemyPresetApplicator`
- Do not rely on scene-only defense pattern assignment for runtime boss behavior
- Do not regress repaired hitbox structure
- Do not reintroduce `EnemyVisual` as main gameplay collider
- Do not rebuild architecture before checking preset / roster / slot wiring
- Do not design post-tutorial levels as fake repetition
- Do not let weak-window bosses devolve into mostly-open enemies with only brief blocking
