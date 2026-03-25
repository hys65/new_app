# PROJECT_STATE.md

## Current Project

**Power Prank 3D**  
Unity 6.3 LTS  
Single-scene prototype evolving into structured multi-level content.

---

## Current Development Status

The following systems are implemented and runtime-validated:

### Core Gameplay
- Core throw / hit / breakdown gameplay loop
- Enemy Reaction Layer 1.0
- Enemy Defense Visual Layer 1.0
- Goal HUD Readability 1.0

### Enemy Architecture
- Enemy Archetype System
- Enemy AI Layer 1.0
- Enemy Switching System 1.0
- Enemy Roster / Level Enemy Selection 1.0
- Boss Preset Override Debugging Pass
- Enemy gameplay hitbox structure repair
- Enemy stain attachment repair

### Level Architecture
- Level Content / Encounter Configuration 1.0
- Level Progression / Multi-Level Content 1.0
- Runtime Level Advance 1.0
- Victory Choice Flow 1.0
- Result Panel Polish 1.0
- Level Goal Variety 1.0

---

## Validated Goal Types

Currently implemented goal types:

1. `BreakdownTarget`
2. `HeadHitCount`
3. `SpecificItemHitCount`
4. `UnblockedHitStreak`

### Goal Type Notes

#### BreakdownTarget
Classic score target mode.
Round is won by reaching target breakdown value.

#### HeadHitCount
Counts only successful head hits with gained score.

#### SpecificItemHitCount
Counts only successful hits from one required item id.

#### UnblockedHitStreak
Counts only successful hits that are **not blocked**.
If a blocked hit occurs, current progress is reset to zero.

This goal type was added specifically to support boss-style “zero mistake” encounters.

---

## Runtime Authority Rule

Runtime preset application remains authoritative.

Do not rely on scene-only edits for boss behavior.

Boss-specific behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

---

## Production Lessons Already Validated

### Data / Runtime Lessons
- Wrong `recommendedSlotId` can make a valid boss appear missing at runtime.
- Unity Inspector changes are not always immediately serialized to disk.
- When asset values matter, always verify actual GitHub file contents after push.

### Combat / Goal Lessons
- Goal logic must consume final hit resolution, not raw visual assumptions.
- HUD must explicitly support each new goal type.
- Boss-rule encounters cannot rely on vague defense visuals.
- Defense visual window and blocked evaluation window must remain tightly aligned.
- A defense activation triggered by the current hit must not allow that same hit to score for zero-mistake content.

---

## Validated Boss Reference Levels

### Level 04
**Meeting Tyrant briefcase boss**

Reference purpose:
- deterministic briefcase blocking
- break-to-open behavior

### Level 05
**Narcissist Manager sunglasses boss**

Reference purpose:
- face guard identity
- paint invalidation while guarded

### Level 06
**Meeting Tyrant weak-window boss**

Reference purpose:
- weak-window pressure
- short opening exploitation

### Level 07
**Narcissist Manager precision paint boss**

Reference purpose:
- specified item goal
- paint-ball identity

Final validated tuning:
- Goal Type = `SpecificItemHitCount`
- Required Item Id = `item_paint_ball`
- Target Count = `10`
- Round Duration Seconds = `32`

### Level 08
**Zero-Mistake Boss**

Reference purpose:
- no-error timing discipline
- blocked hit resets progress
- clean-hit streak pressure

Final validated tuning:
- Goal Type = `UnblockedHitStreak`
- Target Count = `6`
- Round Duration Seconds = `32`

---

## Current Design Direction

- Levels 01–03 remain teaching levels
- Levels 04–08 are boss-identity reference levels
- New levels must continue boss identity and rule differentiation
- Do not continue with fake repeated encounters
- Do not add architecture churn unless current systems provably fail

---

## Immediate Next Focus

Next milestone should build on the now-validated boss identity ladder after Level 08.

This means:

- preserve Levels 04–08 as reference implementations
- avoid redesigning finished boss levels unless runtime behavior proves a real issue
- extend content with a meaningfully different boss problem, not a disguised repeat
