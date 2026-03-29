# DEV WORKFLOW

## Working Mode

This project should be developed in short, runtime-validated steps.

Do not do large speculative batches without verifying:
- actual active enemy root
- actual runtime preset
- actual encounter selection
- actual progression state

---

## Repository Structure Discipline

All new work must respect the current canonical layout.

### Runtime scripts
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

Hard rules:
- do not put enemy or level config assets in `Assets/` root
- do not recreate `Assets/ScriptableObjects/Enemy/`
- do not recreate duplicate legacy asset families
- do not recreate a lowercase `gameplay/enemy/` script path

---

## Standard Development Loop

### 1. Inspect
Read docs and inspect relevant scripts and data assets.

### 2. Change one layer only
Prefer changing one of:
- data asset layer
- preset layer
- controller logic layer
- HUD layer

Avoid changing all of them at once.

### 3. Validate in Play mode
Check:
- active enemy root
- runtime preset
- runtime defense pattern
- active level index
- HUD output
- goal completion

### 4. Verify repository truth
After an important asset change:
- let Unity serialize
- check Git diff
- push if needed
- verify the actual repository file contents

### 5. Record result
If behavior is validated, update docs immediately.

---

## Current Best Practice for Boss Content

When creating a boss-style level variant:

1. duplicate or create defense pattern
2. duplicate or create state window profile if needed
3. duplicate or create preset
4. add dedicated roster entry
5. point level selection to that roster entry
6. verify runtime preset and defense pattern during Play
7. verify blocked / break / scoring behavior
8. only then tune values

---

## Current Debug Order

### Enemy behavior issue
1. active runtime enemy root
2. runtime current preset
3. runtime defense pattern
4. roster entry mapping
5. preset asset contents
6. only then controller code

### Goal issue
1. current level encounter config
2. current level progression index
3. current goal type / target
4. projectile hit reporting
5. `LevelGoalController` behavior

### HUD issue
1. current goal type
2. current HUD binding
3. current live runtime state
4. only then UI code

### Repository issue
1. check canonical asset path
2. check for duplicate naming family
3. check whether Unity moved the `.asset` or duplicated it
4. only then inspect logic

---

## Documentation Rule

After any validated milestone:
- update docs before moving on
- keep docs replaceable and repository-friendly
- remove stale “next milestone” text once that milestone is complete
- keep file-structure guidance aligned with the real repository
