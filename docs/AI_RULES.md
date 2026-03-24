# AI RULES

## Mandatory Working Rules

Any AI working on this project must follow these rules.

---

## 1. Read docs first, scripts second

Always:

1. read the documentation chain
2. inspect actual scripts
3. compare docs against runtime/code reality

If there is mismatch:
- trust actual repository code and runtime wiring
- explicitly call out doc drift

---

## 2. Respect current ownership boundaries

Do not blur these responsibilities:

- `EnemyPresetApplicator` = preset injection
- `EnemySwitchingManager` = active enemy switching
- `LevelEnemySelectionController` = level-driven enemy selection
- `LevelEncounterController` = single-encounter application
- `LevelProgressionController` = multi-level flow
- `HudController` = display and input presentation
- `GameplayManager` = round state

---

## 3. Keep enemy behavior data-driven

Boss identity must be authored through:

- defense pattern
- preset
- roster entry
- level selection

Do not solve boss content by scene-only hacks that disappear at runtime.

---

## 4. Treat runtime preset overwrite as a first-class debugging suspect

If a scene value looks correct before Play and wrong after Play, inspect:

- current preset
- preset applicator
- roster entry
- level enemy selection

before rewriting systems.

This rule is proven by the Level 04 briefcase-boss debugging path and reinforced by Level 05 sunglasses-boss authoring.

---

## 5. Do not expand content through fake repetition

Levels 01–03 are teaching levels.

From Level 04 onward:
- each level should justify itself through new boss identity or break logic
- avoid “same fight, different numbers” content expansion

---

## 6. Prefer exact, minimal extensions

Do not introduce broad abstractions unless current content truly needs them.

Prefer:
- dedicated preset
- dedicated pattern
- dedicated roster entry
- exact file replacement
- exact inspector instructions

over:
- speculative system rewrites
- giant architecture replacements

---

## 7. When code changes are safer as full replacement, provide full files

Especially for:
- defense logic
- preset application interactions
- HUD logic

Do not provide fragile partial snippets when full replacement is safer.

---

## 8. Debug actual active instances

In Play mode:
- verify the active enemy root
- do not debug inactive template objects
- inspect runtime fields on the actually selected runtime enemy

This is mandatory in the current multi-root scene setup.

---

## 9. Preserve validated boss levels

Level 04 and Level 05 are now validated boss-foundation content.

Do not casually redesign them while building later levels.

Use them as reference implementations for:
- preset overwrite-safe authoring
- boss-specific item-counter rules
- goal-aligned combat structure