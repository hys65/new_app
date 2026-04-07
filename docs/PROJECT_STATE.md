# PROJECT_STATE.md

## Project: Power Prank 3D

------

# CURRENT PROJECT STATE (CRITICAL)

## Core Gameplay Loop — COMPLETE ✅

- Throw system (drag-based)
- Hit detection (head / body separation)
- Breakdown accumulation system
- Hit feedback (score pop, reaction)

System is stable and production-usable.

------

## Enemy Systems — COMPLETE (V1) ✅

### Enemy Reaction Layer 1.0

- Hit reactions per body part
- Breakdown interruption behavior

### Enemy Defense Visual Layer 1.0

- Guard state visual signaling
- Block feedback ("BLOCK")

### Enemy Archetype System

- Data-driven enemy definition
- Archetype-based behavior switching

### Enemy AI Layer 1.0

- Defense timing logic
- Attack window exposure

------

## Enemy Routing / Level Integration — COMPLETE ✅

### Enemy Switching System 1.0

- Runtime enemy swap via roster

### Enemy Roster System

- Central enemy pool definition

### Level Enemy Selection

- Per-level enemy binding

### Encounter Configuration

- Per-level gameplay configuration

### Level Progression System

- Multi-level progression flow

### Runtime Level Advance

- Auto progression between levels

------

## Result / Flow Systems — COMPLETE ✅

### Victory Choice Flow

- Next / Retry selection

### Result Panel

- Stable UI flow after win

------

## Gameplay Balancing Systems — COMPLETE (PASS 1) ✅

### Level Goal Variety

- TargetCount-based progression

### Combat Pacing Pass

- Throw cooldown per item

------

## Boss System — FUNCTIONALLY COMPLETE (NOT FINAL) ⚠️

### What is COMPLETE

- Preset-driven boss configuration
- Defense pattern + state window + visual profile linkage
- EnemyPresetApplicator runtime binding
- Boss routing through:
  - Roster
  - Level Selection
  - Encounter Config
- Verified working boss levels:
  - Level 05
  - Level 07
  - Level 08
  - Level 09

All boss content is:

- Runtime-valid
- Data-driven
- Fully integrated into level flow

------

### What is NOT FINAL

Current boss content is **NOT production-locked**.

Recent work includes:

- Sunglasses-based identity (Level 05)
- Face-guard arm silhouette (Level 09)
- Head-target emphasis exploration (Level 11)

These are:

- Visual experiments
- Rapid prototypes
- Not final art direction

------

## CRITICAL DECISION

> Bosses will undergo **major redesign in a later production phase**.

This includes:

- Visual identity
- Gameplay expression
- Recognition signals

------

## CURRENT DEVELOPMENT FOCUS

### Primary Goal

- Documentation consolidation
- System clarity
- Architecture stability

### NOT current focus

- Final boss balancing
- Final boss visuals
- Fine-tuning per-level difficulty

------

## RULES GOING FORWARD

1. Do NOT treat current boss visuals as final
2. Do NOT hardcode scene-level adjustments
3. All future boss redesign must:
   - Stay within preset system
   - Use data-driven profiles
   - Go through roster + level routing

------

## SUMMARY

- System layer → STABLE
- Level flow → STABLE
- Boss pipeline → COMPLETE (structure-wise)
- Boss content → TEMPORARY / SUBJECT TO REDESIGN

------

## NEXT PHASE

- Docs consolidation (current)
- Boss redesign planning
- Boss identity redefinition
- Controlled reimplementation using existing architecture
