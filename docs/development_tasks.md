# development_tasks.md

## Power Prank 3D — Development Tasks

------

# CURRENT STAGE

> System complete → Content unstable → Enter consolidation phase

------

# COMPLETED ✅

## Core Gameplay

- Drag throw system
- Hit detection (head/body)
- Breakdown system
- Hit feedback

## Enemy Systems

- Enemy Reaction Layer 1.0
- Enemy Defense Visual Layer 1.0
- Enemy Archetype System
- Enemy AI Layer 1.0

## Level Systems

- Enemy Switching System
- Enemy Roster
- Level Enemy Selection
- Encounter Config
- Level Progression
- Runtime Level Advance

## Flow / UI

- Victory choice (Next / Retry)
- Result panel stabilization

## Gameplay Tuning (Pass 1)

- TargetCount loop
- Throw cooldown pacing

## Boss Pipeline (Structure)

- Preset-driven configuration
- Pattern → State Window → Visual Profile chain
- Runtime preset application
- Multi-level boss routing verified

------

# CURRENTLY PAUSED ⏸️

## Visual / Art Iteration

- Sunglasses variants
- Arm pose tweaking
- Head highlight experiments

## Boss Fine-Tuning

- Difficulty balancing
- Micro timing adjustments

## Per-Level Polish

- Level 04–11 final feel tuning

------

# WHY PAUSED

Because:

- Current boss versions are **not final direction**
- Continuing polish = wasted effort

------

# NEW PRIMARY TASK

## 1. Docs Consolidation Pass

Goal:

- Align all documentation with actual working system
- Remove ambiguity
- Lock architecture understanding

Scope:

- PROJECT_STATE.md
- architecture.md
- development_tasks.md
- SESSION_LOG.md
- AI_CONTEXT.md (partial)

------

## 2. Boss Redesign Planning

Goal:

- Redefine boss identity
- Redefine player recognition signals

Must define:

- Each boss “what player should instantly understand”
- Visual + gameplay coupling

------

## 3. Boss Identity Restructuring

Current problem:

- Boss differences are weak
- Recognition relies on temporary visuals

Target:

- Each boss = clear mechanic + clear visual signal

------

## 4. Visual Direction Reset

Current:

- Prototype-based
- Inconsistent

Target:

- Cohesive visual language
- System-driven presentation

------

## 5. Re-implementation Phase (Later)

After redesign:

- Rebuild boss presets
- Rebuild visual profiles
- Rebind via roster + level system

------

# HARD RULES

## Rule 1

Do NOT continue polishing current boss visuals

## Rule 2

Do NOT balance levels based on temporary content

## Rule 3

Do NOT bypass preset system

## Rule 4

All future boss work must:

- Be data-driven
- Be reproducible

------

# KNOWN TRUTH

> Current boss = functional but temporary

------

# NEXT ACTION

Continue docs updates → then move to boss redesign definition
