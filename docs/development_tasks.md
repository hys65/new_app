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
- validated boss-reference Levels 04–08

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

---

## Level 08 Status

Level 08 is now implemented and validated as a distinct boss-rule encounter.

### Final rule
- Goal Type = `UnblockedHitStreak`
- Target Count = `6`
- Round Duration Seconds = `32`

### Validated behavior
- successful non-blocked hit increments progress
- blocked hit resets progress to zero
- HUD clearly communicates clean-hit progress and reset rule
- defense activation hit no longer gives a free score
- defense visual boundary and blocked evaluation boundary are aligned closely enough for reliable play

---

## What Is Explicitly Finished

Do **not** reopen these unless runtime testing proves a real bug:

- Level 04 reference implementation
- Level 05 reference implementation
- Level 06 reference implementation
- Level 07 reference implementation
- Level 08 zero-mistake implementation
- UnblockedHitStreak goal support
- Level 08 HUD rule readability
- Level 08 blocked-boundary evaluation fix

---

## Immediate Next Milestone

Next work should move forward to the next boss-identity content after Level 08.

Rules for next milestone:

1. do not redesign finished boss levels
2. do not add architecture churn without proof of necessity
3. preserve the validated reference ladder
4. next level must introduce a genuinely different boss problem, not a disguised repeat of:
   - weak-window burst
   - specific-item counting
   - zero-mistake clean-hit streak

---

## Next-Level Design Constraint

The next boss milestone must create a new pressure model.

That means it should not simply be:
- “hit more times”
- “same boss but faster”
- “same rule with smaller window”

It must create a different player demand.

Examples of acceptable direction:
- sequencing pressure
- bait / punish pressure
- rule inversion
- multi-step identity behavior

These are examples of direction only, not locked implementation.

---

## Ongoing Discipline

For every future boss level:

- verify runtime GitHub asset contents after push
- treat runtime preset application as authoritative
- validate both logic and readability
- reject content that feels unclear even if technically correct