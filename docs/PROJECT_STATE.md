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

## Current Scene Validation Result

Validated scene setup:
- EnemyRoot_MeetingTyrant
- EnemyRoot_NarcissistManager
- EnemySwitchingManager with two enemy slots
- LevelEnemySelectionController with LevelEnemySelectionData

Validated runtime result:
- Before Play, both enemy objects exist in scene
- After Play, only the active enemy remains enabled
- Startup active slot works
- `startupSelectionIndex = 0 / 1` both work
- Inactive enemy is disabled correctly
- Preset application logs confirm correct startup selection

---

## Current Architecture Position

The project has moved from:

**enemy behavior definition and scene test switching**

to:

**level-driven enemy content selection**

The current enemy stack is now:

Data  
→ EnemyPresetData  
→ EnemyPresetApplicator  
→ EnemyRuntimePresetController  
→ EnemySwitchingManager  
→ LevelEnemySelectionController / LevelEnemySelectionData

Gameplay sync:

EnemySwitchingManager  
→ GameplayManager.SetActiveEnemyReactionLayer(...)

---

## Current Runtime Model

Current model is:
- multiple enemy roots may exist in scene
- only one enemy is active at a time
- scene switching is orchestration logic
- AI remains per-enemy
- EnemyDefenseStateWindowProfile.autoCycle must remain FALSE
- AI controls defense timing, not the window system

Important boundary:
- this is NOT a full multi-enemy combat system
- this is NOT a wave spawning system
- this is NOT a roster-driven runtime loading system
- this is a reusable level startup enemy selection system built on top of the validated switching layer

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

**system validation**

into:

**content scaling and level authoring**

Next recommended milestone:
- Level Content / Encounter Configuration 1.0
- Enemy Content Expansion 1.0
- Runtime Debug Enemy Switching UI