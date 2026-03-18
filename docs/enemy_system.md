# ENEMY SYSTEM

## Structure

Enemy = Composition of 4 systems

1. Reaction
2. Defense Pattern
3. Defense Window
4. AI

---

## EnemyPresetData

Single entry point for configuration

Contains:

- EnemyArchetypeData
- EnemyDefensePatternData
- EnemyAiProfileData
- EnemyDefenseStateWindowProfileData

---

## EnemyPresetApplicator

Responsibilities:

- Apply preset to all controllers
- Sync data at runtime
- Ensure consistency

---

## Current Enemy Types

### Meeting Tyrant

Profile:

- Defensive
- Stable
- Predictive

Behavior:

- Fast PrepareDefense
- Long Guard
- Short Recover

---

### Narcissist Manager

Profile:

- Reactive
- Expressive
- Fragile

Behavior:

- Late PrepareDefense
- Long Telegraph
- Short Guard
- Long Recover
- Head weakness

---

## Key Insight

Enemy difference is NOT animation

Enemy difference =

AI timing  
+ Defense logic  
+ Window timing  
+ Reaction scaling