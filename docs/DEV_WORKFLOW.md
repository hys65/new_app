# DEV WORKFLOW

## Working Mode

This project should be developed in short, runtime-validated steps.

Do not do large speculative batches without verifying:
- actual active enemy root
- actual runtime preset
- actual encounter selection
- actual progression state

---

## Standard Development Loop

### 1. Inspect
Read docs and inspect relevant scripts.

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

### 4. Record result
If behavior is validated, update docs immediately.

---

## Current Best Practice for Boss Content

When creating a boss-style level variant:

1. duplicate or create defense pattern
2. duplicate or create preset
3. add dedicated roster entry
4. point level selection to that roster entry
5. verify runtime preset and defense pattern during Play
6. verify blocked / break / scoring behavior
7. only then tune values

This workflow was validated during:
- Level 04 briefcase-boss setup
- Level 05 sunglasses-boss setup

---

## Current Debug Order

If something is wrong at runtime, check in this order:

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
5. LevelGoalController behavior

### HUD issue
1. current goal type
2. current HUD binding
3. current live runtime state
4. only then UI code

---

## Current Scene-Wiring Rule

Do not trust pre-Play scene fields when a runtime preset will overwrite them.

If the value changes after entering Play:
- debug the data chain
- not just the scene object

---

## Documentation Rule

After any validated milestone:
- update docs before moving on
- keep docs replaceable and repository-friendly
- do not leave “next milestone” text pointing to already completed work

This is especially important now that:
- Level 04 is completed
- Level 05 is completed
- future sessions must start from Level 06 work, not repeat finished milestones