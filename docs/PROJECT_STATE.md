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

---

## Canonical Repository Layout

### Scripts
```text
unity-client/Assets/Scripts/gameplay/
  Core/
  Data/
  Enemy/
  UI/
  VFX/
```

### Enemy data
```text
unity-client/Assets/Data/Enemy/
  AI/
  Archetypes/
  Defense/
    Patterns/
    StateWindows/
    Visuals/
  Presets/
  Rosters/
```

### Level data
```text
unity-client/Assets/Data/Levels/
  Encounters/
  EnemySelections/
  Progression/
```

### Gameplay items
```text
unity-client/Assets/ScriptableObjects/GameplayItems/
```

Repository cleanliness rule:
- do not place enemy or level config assets back into `Assets/` root
- do not restore `Assets/ScriptableObjects/Enemy/`
- do not restore duplicate legacy naming families

---

## Runtime Authority Rule

Runtime preset application remains authoritative.

Boss-specific behavior must be authored through:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

Do not rely on scene-only edits for final boss behavior.

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

### Combat Pacing Lessons
- High throw frequency can invalidate otherwise good boss balance.
- Throw-rate control must live at the throw decision point, not inside hit resolution.
- Per-item pacing is more compatible with current architecture than a single global cooldown.
- Future boss balancing should assume non-spam throw pacing as the baseline.

### Geometry / Readability Lessons
- A hit-zone judgment encounter requires clear visual separation between head and body.
- Primitive-body enemies can invalidate good rule design if the silhouette does not support the intended aiming choice.
- For current sphere-head enemies, head stain visuals are not reliable enough to justify deep polish right now.

---

## Immediate Next Milestone

**Level 10 boss identity design and authoring**

Constraint:
- keep it meaningfully different from Levels 04–09
- preserve current architecture and cleaned asset layout
- prefer data authoring over new system churn
