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

### Still-open progression levels

- Level 07
- Level 08
- Level 09

These later levels still exist in progression but should continue the boss-first structure already proven by Levels 04–06.

---

## Current Priority

**Level 07 Boss Identity design and implementation**

---

## Immediate Next Tasks

1. Preserve Levels 01–03 as teaching levels
2. Preserve Level 04 as first validated breaker-boss reference
3. Preserve Level 05 as second validated break-then-score boss reference
4. Preserve Level 06 as validated long-defense / short-window timing boss reference
5. Design Level 07 as a genuinely new boss-identity encounter
6. Expand weapon meaning or punishment logic without breaking current architecture
7. Keep boss authoring data-driven:
   - pattern
   - preset
   - roster entry
   - level selection

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

---

## Current Design Direction For Level 07+

Level 07 and beyond should continue the same content logic:

- distinct boss read
- distinct counter logic, restriction logic, or punishment logic
- distinct pacing identity
- minimal clean extension
- no fake repetition

Good direction examples:

- counter-state boss
- punishment-on-wrong-item boss
- short punishable arrogance window
- boss that teaches “do not attack now” rather than only “attack now”

Bad direction examples:

- Level 06 again but with bigger numbers
- another simple breaker swap
- another “paint but different color” variant
- another mostly-open enemy with cosmetic blocking

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

Level 06 is complete as a runtime-validated boss-reference level.

That means future sessions should stop treating Level 06 as “next design target” and instead treat it as:

- a finished reference implementation
- a proof that `HeadHitCount` can support boss timing content
- a proof that long-defense / short-window identity works inside the current system