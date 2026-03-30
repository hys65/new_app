# architecture.md

## Project Runtime Model

**Power Prank 3D** is a single-scene, data-authored, multi-level boss-content prototype.

The project is not structured as:
- a scene-per-level campaign
- a manual inspector-only boss tuning workflow
- a procedural combat sandbox

The project is structured as:
- one runtime scene
- reusable enemy roots
- reusable boss presets
- reusable level encounter data
- progression-driven encounter switching

---

## High-Level Runtime Ownership

### `GameplayManager`

Owns:
- round state
- countdown timer
- breakdown accumulation
- selected throw item
- high-level win/lose state

Does not own:
- multi-level progression
- boss selection flow
- result-panel button logic
- boss preset content authoring

### `LevelEncounterController`

Owns:
- applying one encounter configuration into runtime systems
- encounter-level goal setup
- encounter-level round duration / target setup
- encounter-level enemy selection application

Does not own:
- whole progression sequencing
- retry/next UI ownership
- enemy system architecture itself

### `LevelProgressionController`

Owns:
- ordered level list
- current level index
- retry current level
- advance to next level
- integration with victory-choice flow

Does not own:
- direct boss-rule authoring
- direct HUD rendering responsibilities
- low-level combat scoring

### `HudController`

Owns:
- HUD presentation
- goal text readability
- result panel presentation
- forwarding Retry / Next user intent to progression flow

Does not own:
- progression state machine logic
- encounter rule authority
- round-state authority

---

## Enemy Content Architecture

Enemy behavior is authored through layered data.

### Core behavior layers
- `EnemyArchetypeData`
- `EnemyDefensePatternData`
- `EnemyAiProfileData`
- `EnemyDefenseStateWindowProfileData`

These are combined by:

- `EnemyPresetData`

And applied through:

- `EnemyPresetApplicator`

This is the authoritative content path.

Not through:
- scene-only manual component edits
- pre-Play inspector values that are overwritten at runtime
- duplicate legacy asset families

This rule exists because runtime preset application overwrites defense-related references.

Canonical authoring chain:

**pattern → state window profile → preset → roster entry → level selection → runtime slot routing**

---

## Enemy Runtime Flow

The active enemy at runtime is produced through this chain:

Data
→ `EnemyPresetData`
→ `EnemyPresetApplicator`
→ `EnemyRuntimePresetController`
→ `EnemySwitchingManager`
→ `LevelEnemySelectionController`
→ active enemy slot

Important runtime meaning:
- enemy roots can coexist in the scene
- only one routed enemy should be active for the encounter
- preset application defines the real runtime defense behavior
- scene-only manual defense edits are not authoritative once runtime preset routing is active

---

## Encounter Runtime Flow

Encounter flow is produced through this chain:

`LevelEncounterConfigData`
→ `LevelEncounterController`
→ `GameplayManager`
→ `HudController`

Encounter configuration is responsible for:
- which enemy selection is applied
- which goal type is active
- which target count / breakdown is active
- which round duration is active

This means a level is not defined only by which enemy appears.
A level is the combination of:
- enemy content
- goal rule
- timer pressure

---

## Progression Runtime Flow

Progression flow is produced through this chain:

`LevelProgressionData`
→ `LevelProgressionController`
→ `LevelEncounterController`
→ encounter runtime application

The progression controller is the owner of:
- current level order
- retry same level
- advance to next level
- post-victory next-step routing

This is important because:
- encounter logic should not start owning campaign flow
- HUD should not become the progression state machine
- `GameplayManager` should not be overloaded with level-sequencing responsibilities

---

## Current Content Architecture

### Teaching block

Levels 01–03:
- low confusion
- direct rule teaching
- basic onboarding for goal types

### Boss-reference block

Levels 04–10:
- each level has a distinct demand
- each level acts as a validated reference implementation
- these levels should not be casually redesigned once validated

### Expansion block

Level 11+:
- must continue unique boss identity work
- should reuse the current asset layout and runtime chain
- should avoid fake repetition of prior bosses

---

## Legacy Rule

`LevelEnemyController` is legacy.

Do not build new content flow around it if `LevelEnemySelectionController` and progression are active in the same scene.

If both coexist, the newer selection/progression path is the intended production path.

---

## Canonical Repository Layout

### Runtime scripts

    unity-client/Assets/Scripts/gameplay/
      Core/
      Data/
      Enemy/
      UI/
      VFX/

Important rule:
- `gameplay/Enemy/` is the only valid enemy-runtime script folder
- do not recreate a parallel lowercase `gameplay/enemy/` folder

### Enemy data assets

    unity-client/Assets/Data/Enemy/
      AI/
      Archetypes/
      Defense/
        Patterns/
        StateWindows/
        Visuals/
      Presets/
      Rosters/

### Level data assets

    unity-client/Assets/Data/Levels/
      Encounters/
      EnemySelections/
      Progression/

### Gameplay item assets

    unity-client/Assets/ScriptableObjects/GameplayItems/

Important rule:
- do not put enemy presets / AI / defense patterns / level configs back into `Assets/` root
- do not restore duplicate enemy data directories under `Assets/ScriptableObjects/`

---

## Boss Reference Identity Ladder

### Level 04 – Meeting Tyrant Briefcase Boss

Identity:
- deterministic hard block
- explicit break item logic

### Level 05 – Narcissist Manager Sunglasses Boss

Identity:
- face guard
- paint invalid while guarded

### Level 06 – Meeting Tyrant Weak-Window Boss

Identity:
- short vulnerability windows
- pressure through timing bursts

### Level 07 – Narcissist Manager Precision Paint Boss

Identity:
- required-item success rule
- paint-ball precision objective

### Level 08 – Zero-Mistake Boss

Identity:
- clean-hit streak objective
- blocked hit resets progress
- boundary clarity is part of the encounter design

### Level 09 – Narcissist Manager Face Guard Boss

Identity:
- head is intentionally low-value
- body is the primary scoring route
- player must learn zone choice rather than greed for face hits

### Level 10 – Adaptive Shutdown Boss

Identity:
- predictable rhythm gets punished
- varied rhythm improves hit efficiency
- player must manage whether the boss can read the throw pattern
- pressure comes from anti-predictability rather than item restriction or hit-zone judgment

---

## Combat Pacing Architecture

Per-item throw pacing is part of the runtime architecture.

Rule:
- cooldown data belongs to `GameplayItemData`
- throw gating belongs at the throw decision point
- cooldown must not be implemented as post-hit correction logic

Why this matters:
- boss balance assumes throw pacing is not effectively spam-unlimited
- anti-predictability content depends on meaningful rhythm expression
- future balancing should treat item cooldown as baseline combat truth

---

## Production Lessons

- wrong slot routing can make a valid boss appear absent
- runtime preset application is the source of truth
- defense visuals and block logic must remain aligned
- a boss encounter can fail not because of numbers, but because of unreadable boundaries
- future boss-rule content should be evaluated first on clarity, second on difficulty
- geometry and control feel can invalidate a theoretically good boss rule
- a new boss identity does not require a new system if current systems can produce a clear new player demand

---

## Current Recommended Direction

Do not introduce broad new architecture before content truly demands it.

Best leverage remains:
- more boss identity through data
- clean roster/preset authoring
- minimal system expansion only when current systems are proven insufficient

The next milestone should continue content expansion, not architecture churn.
