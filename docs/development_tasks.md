# development_tasks.md

## Current Position

The project has completed:

- core throw / hit / breakdown loop
- enemy reaction layer
- defense visual layer
- archetype system
- enemy AI layer
- enemy switching
- roster / level enemy selection
- level encounter configuration
- level progression / multi-level flow
- runtime level advance
- victory choice flow
- result panel polish
- level goal variety
- boss preset override debugging pass
- combat pacing / per-item throw cooldown pass
- repository asset-structure cleanup
- Level 10 boss identity closure
- Level 11 boss identity closure

---

## Completed Boss Reference Ladder

### Level 04

Meeting Tyrant briefcase boss

### Level 05

Narcissist Manager sunglasses boss

### Level 06

Meeting Tyrant weak-window boss

### Level 07

Narcissist Manager precision paint boss

### Level 08

Zero-Mistake Boss

### Level 09

Narcissist Manager Face Guard Boss

### Level 10

Adaptive Shutdown Boss

### Level 11

Head Hunter Boss

---

## Level 09 Status

Level 09 is implemented and accepted as playable.

### Final rule

- Goal Type = `BreakdownTarget`
- Target Breakdown = `180`
- Round Duration Seconds = `34`

### Final identity

- head is intentionally low-value
- body is the primary reliable scoring route
- encounter pressure comes from hit-zone judgment rather than item restriction or streak reset

### Important accepted limitation

- head stain visuals remain imperfect on the current sphere-head setup
- this is accepted for now
- do not reopen deep stain polish unless it becomes a true blocker later

---

## Level 10 Status

Level 10 is implemented and accepted as playable.

### Final identity

- predictable throw rhythm is punished
- mixed rhythm materially improves hit efficiency
- encounter pressure comes from anti-predictability pressure
- the encounter is not primarily about item restriction
- the encounter is not primarily about hit-zone judgment
- the encounter is not a weak-window repeat

### Validation outcome

- fixed-rhythm test produced materially more blocked hits than mixed-rhythm test
- mixed rhythm provided reliable counterplay
- accepted subjective result: the encounter feels right

### Design meaning

- the boss ladder now includes an anti-predictability encounter
- this expands the ladder without requiring new core systems

---

## Level 11 Status

Level 11 is implemented and accepted as playable.

### Final rule

- Goal Type = `HeadHitCount`
- Target Count = `7`
- Target Breakdown = `160`
- Round Duration Seconds = `38`

### Final identity

- the player is pushed toward a later scoring opportunity
- the player is pushed toward head-focused precision rather than generic safe spam
- the encounter is not primarily about item restriction
- the encounter is not primarily about body-route judgment
- the encounter is not primarily about anti-predictability
- the identity is not a weak-window repeat even though later conversion timing matters

### Validation outcome

- the initial authoring pass was too permissive and allowed easy wins
- the boss identity only started to hold after defensive pressure was increased
- after the pressure increase, the encounter was accepted as basically correct
- later full-ladder balance tuning is still allowed

### Design meaning

- the boss ladder now includes a late-window head-pressure encounter
- this expands the ladder without requiring new goal-type churn
- the important step was identity closure, not premature release balancing

---

## Repository Cleanup Status

This is now treated as finished:

- enemy data lives under `Assets/Data/Enemy/...`
- level data lives under `Assets/Data/Levels/...`
- gameplay items remain under `Assets/ScriptableObjects/GameplayItems/`
- enemy runtime scripts use `Assets/Scripts/gameplay/Enemy/`
- duplicate legacy naming families were removed from active use

Do not reopen cleanup work unless a real duplicate or misplaced asset reappears.

---

## What Is Explicitly Finished

Do **not** reopen these unless runtime testing proves a real bug:

- Level 04 reference implementation
- Level 05 reference implementation
- Level 06 reference implementation
- Level 07 reference implementation
- Level 08 zero-mistake implementation
- Level 09 face-guard implementation
- Level 10 adaptive shutdown implementation
- Level 11 head-hunter implementation
- UnblockedHitStreak goal support
- Level 08 HUD rule readability
- Level 08 blocked-boundary evaluation fix
- Level 09 silhouette / hit-zone repair at the current accepted level
- Per-item throw cooldown baseline
- Repository structure cleanup pass

---

## Immediate Next Milestone

**Combat Readability / Boss Presentation Pass**

### Constraints

- preserve current architecture and data-driven authoring flow
- do not casually redesign finished boss levels
- improve readability before final release balancing
- prefer content / presentation clarification before system churn
- do not reopen deep stain-system work unless it becomes a true blocker

### Required output

- clearer telegraph readability
- clearer active defense readability
- clearer weak-window readability
- clearer block / break / success readability
- stronger combat-language consistency across boss encounters

---

## Release-Planning Note

Full 04–11 balancing should happen later.

Do not treat the current stage as the final release-balance pass.

The correct order is:

1. keep current boss identities valid
2. continue readability / presentation work
3. then perform final full-ladder balancing closer to release

---

## Ongoing Discipline

For every future boss level:

- verify runtime GitHub asset contents after push
- treat runtime preset application as authoritative
- validate both logic and readability
- reject content that feels unclear even if technically correct
- evaluate geometry, control feel, and hit-zone readability alongside raw rule tuning
- keep new assets in canonical folders from the start
