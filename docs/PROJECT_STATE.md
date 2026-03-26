# PROJECT_STATE.md

## Current Project

**Power Prank 3D**  
Unity 6.3 LTS  
Single-scene prototype evolved into structured multi-level boss-reference content.

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

This goal type was added specifically to support boss-style zero-mistake encounters.

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

### Geometry / Readability Lessons
- A hit-zone judgment encounter requires clear visual separation between head and body.
- Primitive-body enemies can invalidate good rule design if the silhouette does not support the intended aiming choice.
- For current sphere-head enemies, head stain visuals are not reliable enough to justify deep polish right now.

---

## Validated Boss Reference Levels

### Level 04
**Meeting Tyrant briefcase boss**

Reference purpose:
- deterministic hard block
- explicit break item logic

### Level 05
**Narcissist Manager sunglasses boss**

Reference purpose:
- face guard identity
- paint invalid while guarded

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

### Level 09
**Narcissist Manager Face Guard Boss**

Reference purpose:
- hit-zone judgment pressure
- head is intentionally low-value
- body is the primary reliable scoring route

Final validated tuning:
- Goal Type = `BreakdownTarget`
- Target Breakdown = `180`
- Round Duration Seconds = `34`

Important current acceptance:
- the encounter is considered playable and closed for now
- head stain visuals remain imperfect and are intentionally not the current blocker

---

## Current Design Direction

- Levels 01–03 remain teaching levels
- Levels 04–09 are boss-identity reference levels
- New levels must continue boss identity and rule differentiation
- Do not continue with fake repeated encounters
- Do not add architecture churn unless current systems provably fail

---

## Current Open Quality Notes

These are known and accepted for now:

- head-hit stain visuals on the current sphere-head setup are imperfect
- the current throw system still allows unrealistically high throw frequency because weapon cooldown has not yet been added

These are not reasons to reopen Level 09 immediately, but they are real production notes.

---

## Immediate Next Focus

Next milestone should be:

**Combat Pacing / Per-Item Throw Cooldown Pass**

This means:

- add cooldown to each weapon independently
- stop infinite high-frequency throw spam
- create trustworthy pacing for future boss balancing
- treat this as a gameplay systems pass, not as a one-off patch inside a single level
