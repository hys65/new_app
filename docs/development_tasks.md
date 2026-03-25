# DEVELOPMENT TASKS

## Current Completed Content

### Teaching levels

- Level 01 -> `BreakdownTarget`
- Level 02 -> `HeadHitCount`
- Level 03 -> `SpecificItemHitCount(item_egg)`

### Boss-reference levels

- Level 04 -> `BreakdownTarget` + Meeting Tyrant briefcase boss
- Level 05 -> `SpecificItemHitCount(item_paint_ball)` + Narcissist Manager sunglasses boss
- Level 06 -> `HeadHitCount` + Meeting Tyrant weak-window boss
- Level 07 -> `SpecificItemHitCount(item_paint_ball)` + Narcissist Manager precision paint boss

### Still-open progression levels

- Level 08
- Level 09

These later levels still exist in progression and should continue the boss-first structure already proven by Levels 04–07.

---

## Current Priority

**Level 08 Boss Identity design and implementation**

---

## Immediate Next Tasks

1. Preserve Levels 01–03 as teaching levels
2. Preserve Level 04 as first validated breaker-boss reference
3. Preserve Level 05 as second validated break-then-score boss reference
4. Preserve Level 06 as validated long-defense / short-window timing boss reference
5. Preserve Level 07 as validated repeated precision-loop boss reference
6. Design Level 08 as a genuinely new boss-identity encounter
7. Keep boss authoring data-driven through:
   - defense pattern
   - defense state window profile
   - preset
   - roster entry
   - level enemy selection
   - runtime slot routing

---

## Strict Content Rule

Do not continue content expansion by repeating old levels with only:

- bigger numbers
- same boss read
- same break logic
- same timing logic with different labels

From Level 04 onward, each level must justify itself as a new boss encounter.

---

## Current Boss Reference Rules

### Level 04 reference

- boss = Meeting Tyrant briefcase boss
- defense = timed briefcase guard
- breaker = sponge hammer
- blocked = non-hammer items

### Level 05 reference

- boss = Narcissist Manager sunglasses boss
- defense = timed face guard
- breaker = foam sprayer
- scorer = paint ball
- blocked while active = paint

### Level 06 reference

- boss = Meeting Tyrant weak-window boss
- defense = long active defense cycle
- dominant pacing = mostly blocked
- vulnerability = very short exposed scoring window
- scorer = valid head hits during weak timing
- goal = `HeadHitCount`

### Level 07 reference

- boss = Narcissist Manager precision paint boss
- defense = repeated face-guard cycling
- practical breaker = foam sprayer
- required scorer = paint ball
- intended mastery = repeatedly execute foam-break -> paint-head loop
- goal = `SpecificItemHitCount(item_paint_ball)`

---

## Current Design Direction For Level 08+

Level 08 and beyond should continue the same content logic:

- distinct boss read
- distinct counter logic, restriction logic, or punishment logic
- distinct pacing identity
- minimal clean extension
- no fake repetition

Good direction examples:

- punishment-on-wrong-item boss
- precision-sequence boss
- short punishable arrogance window
- boss that teaches “prepare correctly, then score correctly”

Bad direction examples:

- Level 06 again but with bigger numbers
- another simple breaker swap
- another generic breakdown level with a different label
- another mostly-open enemy with cosmetic blocking
- Level 07 again with only a higher paint count

---

## Current Technical Rule For Boss Authoring

Boss-specific behavior must be authored through:

1. defense pattern
2. defense state window profile
3. preset
4. roster entry
5. level enemy selection
6. runtime slot routing

Do not assume scene component values are authoritative after Play starts.

---

## Current Production Note

Level 07 is complete as a runtime-validated boss-reference level.

That means future sessions should stop treating Level 07 as a design target and instead treat it as:

- a finished reference implementation
- a proof that `SpecificItemHitCount(item_paint_ball)` can support more than one boss identity
- a proof that repeated break-score loops can form a clean boss mastery check without architecture churn