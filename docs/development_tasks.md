# DEVELOPMENT TASKS

## Completed
- Core throw / hit / breakdown gameplay loop
- Enemy Reaction Layer 1.0
- Enemy Defense Visual Layer 1.0
- Enemy Archetype System
- Enemy AI Layer 1.0
- Enemy Switching System 1.0
- Enemy Roster / Level Enemy Selection 1.0
- Level Content / Encounter Configuration 1.0
- Level Progression / Multi-Level Content 1.0
- Runtime Level Advance 1.0
- Victory Choice Flow 1.0
- Result Panel Polish 1.0
- Level Goal Variety 1.0
- Enemy gameplay hitbox structure repair
- Enemy stain attachment repair

---

## Level Goal Variety 1.0 Summary

Implemented:
- encounter primary goal support through `LevelEncounterConfigData.primaryGoal`
- `LevelGoalController`
- `CombatHitInfo`
- hit reporting from `ProjectileBehavior`
- support for three validated goal types:
  - `BreakdownTarget`
  - `HeadHitCount`
  - `SpecificItemHitCount`

Validated:
- result panel goal summary now reflects active encounter goal
- non-breakdown goals can finish rounds without reaching target breakdown
- head-hit-only objective works
- item-specific objective works
- three goal-driven sample encounters are now possible

---

## Enemy Hitbox Repair Summary

Implemented:
- `EnemyVisual` converted to visual-only role
- `Torso` body collider added / tuned
- `HeadCollider` preserved as dedicated head hitbox with Tag = `Head`
- `EnemyHitReaction` moved to shared gameplay parent
- upper-head coverage tuned by repositioning / resizing `HeadCollider`

Validated:
- body hits register consistently
- head hits register consistently
- head-hit goals no longer stall at `0 / target`
- visual shell no longer steals collision

---

## Enemy Stain Attachment Repair Summary

Implemented:
- enemy hit stains now parent to enemy hit target hierarchy
- enemy hit stain rigidbody motion and gravity are disabled on spawn
- ground stains remain under world `Stains` root

Validated:
- enemy stains remain attached after impact
- enemy stains no longer fall out of scene

---

## Current Baseline Content Targets

Recommended official early baseline:
- Level 1 → `BreakdownTarget`
- Level 2 → `HeadHitCount`
- Level 3 → `SpecificItemHitCount(item_egg)`

This three-level sequence should remain the baseline validation flow before adding more complex goals.

---

## Next Recommended Milestones

### 1. Content Expansion toward 12 Levels
Goal:
Expand from the current validated 3-goal baseline toward the planned first content set.

Suggested scope:
- author more encounter configs
- reuse enemy roster entries intentionally
- vary target breakdown and time pressure
- vary encounter goal type across progression
- mix enemy archetypes intentionally across early levels

### 2. Enemy Visual Identity Upgrade
Goal:
Make Meeting Tyrant and Narcissist Manager more visually distinct.

Suggested scope:
- silhouette separation
- color / accessory variation
- stronger visual read during defense states
- enemy-specific identity polish

### 3. Additional Goal Types
Goal:
Expand beyond the current three-goal baseline only after baseline stabilization.

Suggested future scope:
- `BreakCount`
- `WeakHitCount`
- `BlockCount`

Important rule:
- do not add more goal types until the current three-goal baseline is fully documented and stable across both enemy variants

### 4. Result Panel Polish 1.1
Optional refinement milestone.

Suggested scope:
- stronger dimmer presentation
- improved card styling
- better button styling
- hide or fade regular HUD while result panel is active
- improve typography hierarchy