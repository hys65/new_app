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
- Combat Pacing / Per-Item Throw Cooldown Pass

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

### Repository Discipline

- Canonical asset-layout cleanup
- Canonical script-folder cleanup
- Current boss-ladder documentation closure through Level 11

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

---

## Validated Boss Reference Levels

### Level 04

**Meeting Tyrant briefcase boss**

- deterministic hard block
- explicit break item logic

### Level 05

**Narcissist Manager sunglasses boss**

- face guard identity
- paint invalid while guarded

### Level 06

**Meeting Tyrant weak-window boss**

- weak-window pressure
- short opening exploitation

### Level 07

**Narcissist Manager precision paint boss**

- Goal Type = `SpecificItemHitCount`
- Required Item Id = `item_paint_ball`
- Target Count = `10`
- Round Duration Seconds = `32`

### Level 08

**Zero-Mistake Boss**

- Goal Type = `UnblockedHitStreak`
- Target Count = `6`
- Round Duration Seconds = `32`

### Level 09

**Narcissist Manager Face Guard Boss**

- Goal Type = `BreakdownTarget`
- Target Breakdown = `180`
- Round Duration Seconds = `34`
- head is intentionally low-value
- body is the primary reliable scoring route

Important accepted limitation:

- head stain visuals remain imperfect on the current sphere-head setup
- do not reopen deep stain work unless it becomes a true blocker

### Level 10

**Adaptive Shutdown Boss**

- accepted as a distinct boss identity
- predictable throw rhythm is blocked significantly more often
- rhythm variation materially improves hit efficiency
- encounter pressure comes from anti-predictability pressure, not item restriction
- encounter pressure comes from anti-predictability pressure, not hit-zone judgment

Validated play result:

- fixed-rhythm test produced materially more blocked hits than mixed-rhythm test
- mixed rhythm produced reliable counterplay
- accepted subjective result: the encounter feels correct

### Level 11

**Head Hunter Boss**

- accepted as a distinct boss identity
- Goal Type = `HeadHitCount`
- Target Count = `7`
- Target Breakdown = `160`
- Round Duration Seconds = `38`

Validated design meaning:

- the player is pushed toward a later scoring opportunity
- the player is pushed toward head-focused precision rather than generic safe spam
- the encounter pressure is not primarily about item restriction
- the encounter pressure is not primarily about Level 09-style body-route judgment
- the encounter pressure is not primarily about Level 10-style anti-predictability

Accepted production status:

- the boss identity is considered playable and distinct
- later full-ladder balancing is still allowed
- final release balancing is intentionally postponed until after more presentation / art-side work

---

## Canonical Repository Layout

### Scripts

unity-client/Assets/Scripts/gameplay/

- Core/
- Data/
- Enemy/
- UI/
- VFX/

### Enemy data

unity-client/Assets/Data/Enemy/

- AI/
- Archetypes/
- Defense/
- Patterns/
- StateWindows/
- Visuals/
- Presets/
- Rosters/

### Level data

unity-client/Assets/Data/Levels/

- Encounters/
- EnemySelections/
- Progression/

### Gameplay items

unity-client/Assets/ScriptableObjects/GameplayItems/

Repository cleanliness rule:

- do not place enemy or level config assets back into `Assets/` root
- do not restore `Assets/ScriptableObjects/Enemy/`
- do not restore duplicate legacy naming families in active content

---

## Runtime Authority Rule

Runtime preset application remains authoritative.

Boss-specific behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

Do not rely on scene-only edits for final boss behavior.

---

## Production Lessons Already Validated

### Data / Runtime Lessons

- wrong `recommendedSlotId` can make a valid boss appear missing at runtime
- Unity Inspector changes are not always immediately serialized to disk
- when asset values matter, always verify actual GitHub file contents after push

### Combat / Goal Lessons

- goal logic must consume final hit resolution, not raw visual assumptions
- HUD must explicitly support each new goal type
- boss-rule encounters cannot rely on vague defense visuals
- defense visual window and blocked evaluation window must remain tightly aligned
- a defense activation triggered by the current hit must not allow that same hit to score for zero-mistake content

### Combat Pacing Lessons

- high throw frequency can invalidate otherwise good boss balance
- throw-rate control must live at the throw decision point, not inside hit resolution
- per-item pacing is more compatible with current architecture than a single global cooldown
- future boss balancing should assume non-spam throw pacing as the baseline

### Geometry / Readability Lessons

- a hit-zone judgment encounter requires clear visual separation between head and body
- primitive-body enemies can invalidate good rule design if the silhouette does not support the intended aiming choice
- for current sphere-head enemies, head stain visuals are not reliable enough to justify deep polish right now

### Level 10 Lessons

- a new boss identity can still be created with current systems if the demand shift is clear enough
- anti-predictability pressure is readable when the player can feel the difference between fixed rhythm and mixed rhythm
- once the encounter already feels correct, unnecessary “smartening up” is more dangerous than helpful

### Level 11 Lessons

- a new boss identity can still be created with current systems without adding a new goal type
- a later scoring window can become its own boss demand if the defensive presence is strong enough
- do not confuse “more throws required” with “new boss identity”
- first establish the boss demand, then leave final release balancing for later

---

## Immediate Next Milestone

**Combat Readability / Boss Presentation Pass**

Direction:

- improve telegraph clarity
- improve active defense readability
- improve weak-window readability
- improve block / break / success readability
- preserve current data-driven architecture
- avoid unnecessary system churn

Important release-planning note:

Do not perform final 04–11 release balancing yet.
That full-ladder balancing pass should happen later, after more presentation / art-side clarity work is in place.
