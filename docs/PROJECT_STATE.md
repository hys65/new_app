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
- Enemy Hitbox Structure Repair ✅
- Enemy Stain Attachment Repair ✅
- Goal-aware HUD Readability 1.0 ✅
- Boss Preset Override Debugging Pass ✅
- Level 4 Briefcase Boss Foundation ✅

---

## Current Goal System Status

Implemented and validated:

- `BreakdownTarget`
- `HeadHitCount`
- `SpecificItemHitCount`

Validated tutorial flow:

- Level 1 -> BreakdownTarget
- Level 2 -> HeadHitCount
- Level 3 -> SpecificItemHitCount(item_egg)

Current note:
These goal types are still the active production goal set, but future boss content may require a minimal set of new boss-specific goals.

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
- `EnemyPresetApplicator`
- `EnemyRuntimePresetController`
- `EnemySwitchingManager`

Important fact:

Runtime preset application can overwrite scene-level defense pattern and defense window configuration.
Boss behavior must be authored through the actual preset used by the selected roster entry.

---

## Current Known Working Enemy Content

### Meeting Tyrant
- base archetype exists
- preset exists
- boss-specific briefcase preset path now exists
- boss-specific briefcase defense pattern now exists
- Level 4 has begun transition into a distinct boss identity encounter

### Narcissist Manager
- base archetype exists
- preset exists
- still uses standard behavior path
- intended next boss identity direction: sunglasses-based defense behavior

---

## Current Content Structure

### Tutorial Levels
- Level 1 = tutorial breakdown
- Level 2 = tutorial head hits
- Level 3 = tutorial specific item

### Boss Transition
- Level 4 = first distinct boss identity level
- current identity: Briefcase Boss / Hammer Break rule
- current implementation uses a dedicated roster entry and dedicated preset path

### Extended Content
- Levels 5-9 exist in progression content
- these levels may be revised to align with the new boss-first structure

---

## Current Major Design Rule

Do NOT treat Levels 4+ as simple repeated content rotation.

Invalid direction:
- repeat breakdown / head-hit / specific-item loops with only numeric escalation

Required direction:
- each boss level must have a distinct defense identity
- each boss level must create a distinct item usage pattern
- boss identity takes priority over raw goal repetition

---

## Current Technical Debug Lessons

### Preset Override Lesson
Scene-level edits to `EnemyDefenseController.defensePattern` are not reliable if runtime preset application is active.

Correct debug order:
1. level enemy selection
2. roster entry
3. preset
4. runtime controller
5. scene object final state

### Boss Foundation Lesson
For boss work:
- first make runtime preset path correct
- then verify runtime-selected defense pattern
- then verify defense activation
- then verify blocked / break combat result
- only after that add higher-level boss goals

---

## Current Recommended Next Step

**Level 5 Boss Identity: Sunglasses Boss**

High-level direction:
- create boss-specific Narcissist Manager preset
- assign boss-specific defense pattern through preset path
- make paint ineffective until sunglasses defense is broken
- preserve current progression / encounter / goal architecture

---

## Current Red Flags To Avoid

- Do not bypass `EnemyPresetApplicator`
- Do not rely on scene-only defense pattern assignment for runtime boss behavior
- Do not regress repaired hitbox structure
- Do not reintroduce `EnemyVisual` as main gameplay collider
- Do not rebuild architecture before checking preset / roster / selection wiring
- Do not design post-tutorial levels as fake repetition
