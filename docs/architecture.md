# ARCHITECTURE

## Core Design

Data-driven layered architecture

---

## Layers

### 1. Reaction Layer

Input:
- Hit events

Driven by:
- EnemyArchetypeData

Output:
- Visual reactions

---

### 2. Defense Logic Layer

Input:
- Hit validation

Driven by:
- EnemyDefensePatternData

Output:
- Block / Break decision

---

### 3. Defense Window Layer

State Machine:
- None
- Telegraph
- Active
- Recover

Driven by:
- EnemyDefenseStateWindowProfileData

IMPORTANT:
autoCycle must be FALSE

---

### 4. AI Layer

Core brain:

EnemyAiLayerController

Responsibilities:

- Observe player rhythm
- Predict next hit
- Evaluate threat
- Trigger defense cycle
- Handle break recovery

Driven by:
- EnemyAiProfileData

---

### 5. Preset Layer

EnemyPresetData:

- archetype
- defensePattern
- aiProfile
- defenseStateWindowProfile

Applied via:

EnemyPresetApplicator

---

## Data Flow

Preset
→ Applicator
→ Controllers
→ Runtime behavior

---

## Design Rule

NO cross-layer hard dependency

ALL behavior comes from data injection