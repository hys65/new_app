# AI CONTEXT

## Project Identity

Project name:
**Power Prank 3D**

Project type:

- Unity 6.3 LTS prototype
- single-scene gameplay prototype evolving into multi-level boss flow
- third-person fixed-camera prank-throwing game

Core fantasy:

- throw prank items at exaggerated workplace-style enemy bosses
- build breakdown pressure
- read defense states
- react to boss-specific gimmicks
- choose the correct item or timing window
- clear goal-driven levels

---

## Current Stage

The project is no longer just validating combat feel.

It has already validated:

- core throw / hit / breakdown gameplay loop
- enemy reaction behavior
- enemy defense presentation
- archetype-driven enemy behavior
- AI-driven defense timing
- same-scene enemy switching
- roster-driven level enemy selection
- encounter-driven target/time/enemy authoring
- progression-driven multi-level runtime flow
- player-facing result UI with Retry / Next flow
- localized result panel support
- goal-aware HUD readability
- level-specific boss defense pattern injection through runtime preset path

The current project stage is:

**tutorial-complete prototype moving into distinct boss identity implementation**

---

## Current Completed Systems

### Gameplay
- ThrowController
- Projectile hit flow
- GameplayManager breakdown loop
- Hit popup feedback
- HUD current goal / breakdown / time / item display
- combo display
- round finish handling

### Enemy Runtime
- EnemyReactionLayerController
- EnemyDefenseController
- EnemyDefenseVisualLayerController
- EnemyDefenseStateWindowController
- EnemyAiLayerController
- EnemyVisualProxyController
- EnemyRuntimePresetController

### Enemy Data
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- EnemyPresetData
- EnemyRosterData

### Selection / Encounter / Progression
- LevelEnemySelectionData
- LevelEnemySelectionController
- LevelEncounterConfigData
- LevelEncounterController
- LevelProgressionData
- LevelProgressionController

### UI / Result Flow
- HudController
- localized Retry / Next labels
- localized result subtitle / level info / goal summary / final-level notice
- result panel hidden-at-start and shown-on-finish behavior
- goal-aware HUD text for Breakdown / Head Hits / Specific Item Hits

---

## Current Runtime Content Status

### Enemy Archetypes
- Meeting Tyrant
- Narcissist Manager

### Goal Types
- BreakdownTarget
- HeadHitCount
- SpecificItemHitCount

### Progression Content
- Levels 1-9 currently exist in progression data
- Levels 1-3 function as tutorial levels
- Level 4 has begun conversion into a boss-identity level
- Levels 5-9 exist as content placeholders / extended content and may be revised under the new boss-first design direction

---

## New Design Rule

### Tutorial Rule
Levels 1-3 are tutorial content only.

They teach:
- basic breakdown
- head-hit goals
- specific-item goals

They are not the model for long-term content repetition.

### Boss Rule
From Level 4 onward:
- each level should introduce a distinct boss identity
- each boss should have a distinct defense style
- each boss should have a distinct item-counter rule or timing rule
- avoid fake expansion through repeating the same 3 goal types with only numeric escalation

Boss identity now comes before simple goal rotation.

---

## Current Runtime Model

Current runtime model is:

- multiple enemy roots may exist in scene
- only one enemy is active at a time
- enemy switching is scene orchestration, not AI ownership
- runtime-selected roster entry decides which preset is applied
- preset application decides runtime defense pattern / AI / archetype / defense window profile
- scene-level manual defense pattern assignment may be overwritten by preset application
- progression decides which encounter is active
- HUD presents goals and result choices
- progression executes retry / next logic

---

## Important Runtime Configuration Fact

When debugging boss behavior:

Do not assume the scene object's `EnemyDefenseController` values are the final runtime truth.

Runtime preset flow may overwrite them through:

- roster entry
- level enemy selection data
- preset application

If startup behavior differs from scene values, inspect:

1. `LevelEnemySelectionData`
2. `EnemyRosterData`
3. `EnemyPresetData`
4. `EnemyPresetApplicator`

in that order.

---

## Mandatory Architecture Rules

### Preset Application
- `EnemyPresetApplicator` must remain the only preset injection layer
- do not inject preset data directly into multiple runtime components
- do not rely on scene-only defense pattern edits when preset application is active

### Defense Timing
- boss defense timing should remain runtime-controlled
- broad system ownership should stay in current enemy runtime stack
- do not reintroduce conflicting startup preset paths

### Encounter / Progression
- `LevelEncounterController` owns single encounter application
- `LevelProgressionController` owns multi-level flow
- do not merge these responsibilities

### HUD / Result Flow
- `HudController` presents goal and result text
- `HudController` must not own progression logic
- `GameplayManager` owns round state
- `GameplayManager` must not become progression executor

---

## Current Recommended Next Step

Preferred next milestone:

**Level 5 Boss Identity: Sunglasses Boss**

Reason:

- Level 4 proved the need for boss-specific preset paths
- preset overwrite behavior is now understood
- next value comes from extending boss identity, not rebuilding architecture
- item roles still need stronger mechanical differentiation

Secondary follow-up:

- restructure Levels 4-12 around unique boss identities
- introduce boss-specific goal types only when necessary
- preserve tutorial levels as Levels 1-3
