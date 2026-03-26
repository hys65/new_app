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
- validated boss-reference Levels 04–09

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

---

## Level 09 Status

Level 09 is now implemented and accepted as playable.

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

## What Is Explicitly Finished

Do **not** reopen these unless runtime testing proves a real bug:

- Level 04 reference implementation
- Level 05 reference implementation
- Level 06 reference implementation
- Level 07 reference implementation
- Level 08 zero-mistake implementation
- Level 09 face-guard implementation
- UnblockedHitStreak goal support
- Level 08 HUD rule readability
- Level 08 blocked-boundary evaluation fix
- Level 09 silhouette / hit-zone repair at the current accepted level

---

## Immediate Next Milestone

**Combat Pacing / Per-Item Throw Cooldown Pass**

This is the next high-priority gameplay milestone.

### Why this matters
Current throw frequency is effectively unrestricted.  
This allows unrealistic spam throwing and undermines:

- weak-window timing pressure
- item-identity balance
- zero-mistake pacing
- future boss balancing trustworthiness

### Required outcome
- every weapon gets its own cooldown
- throw rate becomes intentionally paced
- balancing assumptions can rely on non-spam input behavior

### Important constraint
Do not solve this as a level-specific hack.  
This must be handled as a shared gameplay systems pass.

---

## Next-Level Design Constraint

After the cooldown pass, future boss work must continue the identity ladder.

That means new content should not simply be:
- the same boss but faster
- the same rule with smaller windows
- another disguised item-count encounter
- another disguised streak-reset encounter

Each future boss should create a genuinely different player demand.

---

## Ongoing Discipline

For every future boss level:

- verify runtime GitHub asset contents after push
- treat runtime preset application as authoritative
- validate both logic and readability
- reject content that feels unclear even if technically correct
- evaluate geometry, control feel, and hit-zone readability alongside raw rule tuning