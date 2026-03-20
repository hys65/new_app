# PROJECT STATE

## Project

**Power Prank 3D**  
Unity 6.3 LTS small-scale prototype game.

Core fantasy:
- Throw prank items at enemy characters
- Build breakdown pressure
- Trigger defensive behaviors
- Create readable, funny, character-specific enemy reactions

---

## Current Milestone Status

### Completed Milestones
- Core throw / hit / breakdown gameplay loop ✅
- Enemy Reaction Layer 1.0 ✅
- Enemy Defense Visual Layer 1.0 ✅
- Enemy Archetype System ✅
- Enemy AI Layer 1.0 ✅
- Enemy Switching System 1.0 ✅
- Enemy Roster / Level Enemy Selection 1.0 ✅
- Level Content / Encounter Configuration 1.0 ✅
- Level Progression / Multi-Level Content 1.0 ✅
- Runtime Level Advance 1.0 ✅
- Victory Choice Flow 1.0 ✅

---

## Current Enemy System Status

Enemy behavior is fully data-driven.

Implemented behavior definition stack:
- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData
- Combined through EnemyPresetData
- Applied through EnemyPresetApplicator

Implemented playable archetypes:

### 1. Meeting Tyrant
- Early defense trigger
- Strong guard
- Short recover
- Stable and hard to break

### 2. Narcissist Manager
- Late defense trigger
- Long telegraph
- Short guard
- Long recover
- High head weakness
- Easy to break

Both archetypes are confirmed working and visibly different in runtime.

---

## Enemy AI Layer 1.0 Status

Completed.

Current rules:
- AI controls defense timing
- EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
- Defense window system does not self-run
- AI decides when to enter defense
- Behavior differences are driven by data, not hardcoded enemy branches

Confirmed result:
- Meeting Tyrant and Narcissist Manager produce different defensive timing and feel using the same runtime controller stack

---

## Enemy Switching System 1.0 Status

Completed and validated.

Implemented runtime switching architecture:
- EnemyRuntimePresetController
- EnemySwitchingManager
- GameplayManager runtime active reaction target switching

Validated capabilities:
1. Runtime preset switching on the same enemy object
2. Switching between multiple real enemy objects in scene
3. Single active enemy model
4. Clean integration with EnemyPresetApplicator
5. Inspector-friendly slot-based setup
6. No breakage to existing hit / defense / AI flow

System behavior:
- Scene may contain multiple enemy roots
- Only one enemy is active at a time
- Active enemy is selected by EnemySwitchingManager
- Preset application still flows through EnemyPresetApplicator
- GameplayManager is updated to use the active enemy reaction layer

---

## Enemy Roster / Level Enemy Selection 1.0 Status

Completed and validated.

Implemented content selection architecture:
- EnemyRosterData
- LevelEnemySelectionData
- LevelEnemySelectionController
- Extended EnemySwitchingManager startup configuration flow

Validated capabilities:
1. Reusable enemy roster asset definition
2. Level-specific enemy selection asset
3. Level-driven startup enemy selection
4. Startup slot assignment through level content
5. Clean reuse of EnemySwitchingManager without bypassing preset injection rules
6. Removal of old competing startup setup path from the current scene

Validated startup selection result:
- `startupSelectionIndex = 0` starts with Meeting Tyrant
- `startupSelectionIndex = 1` starts with Narcissist Manager
- In both cases only one enemy remains active during play
- Preset application occurs once for the selected startup enemy
- Runtime result is clean after removing legacy `LevelEnemyController` scene hookup

---

## Level Content / Encounter Configuration 1.0 Status

Completed and validated.

Implemented:
- LevelEncounterConfigData
- LevelEncounterController

Capabilities:
- bind LevelEnemySelectionData into encounter config
- define target breakdown per level
- define round duration / time limit per level
- apply encounter config into runtime systems
- reduce manual scene-only gameplay setup

Validated encounter content:
- `level_01_encounter_config`
- `level_02_encounter_config`
- `level_03_encounter_config`

Validated runtime result:
- target breakdown changes correctly per encounter
- timer changes correctly per encounter
- enemy selection is applied through encounter config
- encounter application no longer relies on manual scene tweaking

---

## Level Progression / Multi-Level Content 1.0 Status

Completed and validated.

Implemented:
- LevelProgressionData
- LevelProgressionController

Capabilities:
1. Ordered multi-level encounter list
2. Startup level index
3. Single-scene multi-level runtime setup
4. Runtime application of Level 01 / Level 02 / Level 03 encounter configs
5. Runtime next-level progression
6. Runtime current-level restart

Validated runtime result:
- `startupLevelIndex = 0` starts Level 01
- `startupLevelIndex = 1` starts Level 02
- `startupLevelIndex = 2` starts Level 03
- target/time/enemy selection all refresh correctly when changing levels
- runtime progression works without reloading the scene

---

## Runtime Level Advance 1.0 Status

Completed and validated.

Capabilities:
- manual advance to next level
- automatic runtime re-application of encounter config
- runtime restart of current level
- runtime enemy reselection
- runtime timer reset
- runtime target breakdown reset

Transition stability fixes:
- ThrowController resets drag / input transient state on round end
- runtime level transition uses delayed progression to avoid same-frame end/start contamination
- runtime enemy slot switches immediately during level application

Validated result:
- advancing from Level 01 to Level 02 works
- advancing from Level 02 to Level 03 works
- gameplay remains playable after level transition
- no stuck drag state or central-screen projectile lock remains

---

## Victory Choice Flow 1.0 Status

Completed and validated.

Implemented through:
- HudController
- LevelProgressionController integration

Capabilities:
- victory result panel shown on win
- retry current level
- next level button shown only when next level exists
- final level hides next-level option
- auto-advance on win can be disabled for player-facing flow

Validated result:
- Level 01 victory stops and presents choice
- player can click Retry to replay current level
- player can click Next to enter Level 02
- current runtime flow now waits for player decision instead of forcing auto-advance

---

## Current Scene Validation Result

Validated scene setup:
- EnemyRoot_MeetingTyrant
- EnemyRoot_NarcissistManager
- EnemySwitchingManager with two enemy slots
- LevelEnemySelectionController with LevelEnemySelectionData
- LevelEncounterController
- LevelProgressionController
- multi-level progression asset
- HUD result panel with Retry / Next flow

Validated runtime result:
- Before Play, both enemy objects exist in scene
- During play, only the active enemy remains enabled
- startup active slot works
- level-driven enemy startup works
- encounter-driven target/time works
- progression-driven level switching works
- victory choice flow works

---

## Current Architecture Position

The project has moved from:

**enemy behavior definition and scene test switching**

to:

**level-driven multi-stage encounter flow in a single scene**

The current stack is now:

Data  
→ EnemyPresetData  
→ EnemyPresetApplicator  
→ EnemyRuntimePresetController  
→ EnemySwitchingManager  
→ LevelEnemySelectionController / LevelEnemySelectionData  
→ LevelEncounterController / LevelEncounterConfigData  
→ LevelProgressionController / LevelProgressionData

Gameplay sync:
- EnemySwitchingManager → GameplayManager.SetActiveEnemyReactionLayer(...)
- LevelEncounterController → GameplayManager encounter values
- HudController → Retry / Next button delegation into LevelProgressionController

---

## Current Runtime Model

Current model is:
- multiple enemy roots may exist in scene
- only one enemy is active at a time
- scene switching is orchestration logic
- AI remains per-enemy
- EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
- AI controls defense timing, not the window system
- one scene may host multiple level encounter configs
- progression decides which encounter is active
- HUD result flow decides whether player retries or advances

Important boundary:
- this is NOT a full multi-enemy combat system
- this is NOT a wave spawning system
- this is NOT a roster-driven runtime loading system
- this is a reusable level / encounter / progression system built on top of the validated switching layer

---

## Legacy Status

`LevelEnemyController` is now treated as a legacy setup path.

Current rule:
- It must not be used in switching-oriented scenes
- It must not coexist with `LevelEnemySelectionController` in the same scene
- The current validated scene has removed the old `LevelEnemyController` hookup from `Systems`

---

## Current Development Position

The project is now ready to move from:

**multi-level runtime flow validation**

into:

**UI polish + content expansion**

Next recommended milestone:
- Result Panel Polish 1.0
- Level Goal Variety 1.0
- Content Expansion toward 12 Levels
- Enemy Visual Identity Upgrade
