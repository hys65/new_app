# SESSION LOG

## Session Summary

This session completed **Level Goal Variety 1.0** runtime validation and repaired the enemy gameplay hitbox / stain attachment structure that blocked reliable goal progression.

Work moved beyond the previously validated result panel flow and focused on making encounter goals actually function in runtime, especially for head-hit and item-specific objectives.

---

## Starting State

Before this session:
- Core throw / hit / breakdown gameplay loop was already complete
- Enemy switching, level selection, encounter config, progression, runtime next-level flow, and victory choice flow were already validated
- Result Panel Polish 1.0 was already complete
- encounter goals still effectively behaved like pure breakdown goals
- no runtime-validated non-breakdown encounter objective flow existed yet
- current enemy collision structure still allowed visual-shell collider interception
- enemy stains could detach or fall away after impact in some cases

---

## Main Changes

### 1. Level goal data and runtime controller were added
Implemented:
- `LevelGoalType`
- `LevelGoalDefinition`
- `CombatHitInfo`
- `LevelGoalController`

This created a dedicated runtime layer for encounter objectives without moving round-state ownership out of `GameplayManager`.

### 2. Encounter config was extended with primary goal support
`LevelEncounterConfigData` was extended with:
- `primaryGoal`

This made goal definition part of encounter content instead of scene-only logic.

### 3. Encounter application flow was extended
`LevelEncounterController` now:
- applies encounter target/time as before
- configures breakdown win-condition behavior
- applies encounter primary goal into `LevelGoalController`

### 4. GameplayManager was extended for non-breakdown victory
`GameplayManager` now supports:
- configurable breakdown-based win condition enable / disable
- `ForceFinishRound(true)` for externally completed non-breakdown goals

This preserved gameplay-state ownership while allowing goal-driven victory.

### 5. ProjectileBehavior was extended to report hit results
`ProjectileBehavior` now reports:
- `isHeadHit`
- `itemId`
- `gainedScore`

into `LevelGoalController.NotifyHitResolved(...)`

This completed the runtime goal pipeline.

### 6. HudController was extended to display runtime goal summary
`HudController` now reads active goal summary from `LevelGoalController` and presents it on the result panel.

### 7. Enemy hitbox structure was repaired
A major runtime issue was discovered:
- projectile collision first hit `EnemyVisual`
- `EnemyVisual` was `Untagged`
- head-hit goals stayed at `0 / target`

Fixes applied:
- `EnemyVisual` collider disabled
- gameplay body collision moved to dedicated `Torso` collider
- `HeadCollider` kept as dedicated head hitbox with Tag = `Head`
- `EnemyHitReaction` moved from `EnemyVisual` to shared gameplay parent (`DefenseBodyPivot`)
- `HeadCollider` coverage was enlarged / repositioned to cover upper-head gameplay space

### 8. Enemy stain attachment was repaired
A second runtime issue was discovered:
- enemy-hit stains were not properly attached to enemy target hierarchy
- stain rigidbody / gravity caused them to fall out of the scene

Fixes applied:
- enemy-hit stains now parent to enemy target hierarchy
- stain rigidbody gravity / motion disabled on spawn
- ground stains remain world-rooted

---

## Runtime Issues Discovered During Validation

### Issue 1: Head-hit goal stayed at 0
Observed behavior:
- result panel displayed `Head Hits 0 / 3`
- repeated visible head hits did not advance progress

Root cause:
- projectile collided with `EnemyVisual` collider first
- collision tag resolved as `Untagged`
- `isHeadHit` remained false

### Issue 2: Disabling EnemyVisual collider caused projectiles to pass through
Root cause:
- no proper gameplay body collider had been established as replacement
- body hitbox structure was incomplete

### Issue 3: Head-only coverage was too low
Observed behavior:
- lower-head hits worked
- upper-head hits could miss or pass through

Root cause:
- `HeadCollider` size / position did not fully cover upper-head gameplay space

### Issue 4: Enemy stains fell away after hit
Root cause:
- stains were not parented correctly for enemy-hit cases
- stain rigidbody/gravity remained active

---

## Validation Result

Validated runtime behavior after fixes:

### Goal system
- `BreakdownTarget` works
- `HeadHitCount` works
- `SpecificItemHitCount` works

### Head-hit flow
- head-hit objective can now reach completion
- head-hit result panel correctly shows `Goal: Head Hits 3 / 3`

### Item-specific flow
- item-specific objective works correctly with configured `requiredItemId`

### Hitbox flow
- visual shell no longer steals collision
- body hits register correctly
- head hits register correctly
- upper-head coverage is now playable

### Stains
- enemy hit stains remain attached
- enemy hit stains no longer fall out of the scene

---

## Architecture Status After Session

The validated runtime stack is now:

Data  
→ EnemyPresetData  
→ EnemyPresetApplicator  
→ EnemyRuntimePresetController  
→ EnemySwitchingManager  
→ LevelEnemySelectionController / LevelEnemySelectionData  
→ LevelEncounterController / LevelEncounterConfigData  
→ LevelGoalController  
→ LevelProgressionController / LevelProgressionData  
→ HudController result presentation

Gameplay ownership remains clean:
- `GameplayManager` owns round-state
- `LevelProgressionController` owns level flow
- `LevelEncounterController` applies encounter content
- `LevelGoalController` owns encounter objective runtime progress
- `HudController` owns player-facing result presentation

---

## Recommended Next Step

Next recommended milestone:

**Content Expansion toward 12 Levels**

Reason:
- three-goal baseline is now runtime-validated
- hitbox structure is now stable enough for content production
- the next leverage is expanding authored encounter content before adding new goal types