\# AGENTS.md



\## Project Overview



Project Name

Power Prank 3D



Engine

Unity 6.3 LTS



Project Type

Single-scene prototype game.



Concept

Player throws prank items at office enemies.

The goal is to increase the enemy \*\*Breakdown\*\* level to complete the level.



\---



\# Source of Truth



Before doing any work, read these files:



docs/project\_state.md

docs/architecture.md

docs/gameplay\_systems.md

docs/enemy\_system.md

docs/DEV\_WORKFLOW.md

docs/TASKS/current\_task.md



These documents describe the project state, architecture, systems, and current development task.



Do not assume project structure without reading them.



\---



\# Repository Structure



Root



unity-client/

docs/



Unity project is located in:



unity-client/



All project scripts should be placed under:



unity-client/Assets/\_Project/



\---



\# Unity Directory Structure



Unity assets should follow this structure:



Assets/\_Project/



Scripts

Prefabs

ScriptableObjects

Scenes

Materials

VFX

Audio

UI



Scripts must be further organized:



Assets/\_Project/Scripts/



Gameplay

Enemy

UI

Systems

Shared



Example:



Assets/\_Project/Scripts/Enemy

Assets/\_Project/Scripts/Gameplay



\---



\# Coding Rules



Language

C#



Naming rules



Class names



PascalCase



Examples



EnemyDefenseSystem

ThrowController

GameplayManager



File naming



lowercase\_with\_underscore where applicable.



Example



enemy\_meeting\_tyrant

enemy\_narcissist\_boss



\---



\# Script Size Rule



Scripts should normally not exceed:



300 lines.



If a system grows too large, split into:



Logic component

Visual component



Example



EnemyDefenseSystem

EnemyDefenseVisual



\---



\# System Separation



Systems must remain independent.



Never mix unrelated systems.



Example system boundaries



ThrowSystem

EnemySystem

CombatSystem

UISystem



Scripts should stay inside their system folder.



\---



\# Gameplay Rules



Breakdown System



Body hit

+10 Breakdown



Head hit

+20 Breakdown



BLOCK state



When BLOCK is active:



Breakdown must not increase.



HUD should display BLOCK state.



\---



\# Enemy Structure



Enemy hierarchy:



EnemyRoot

├ EnemyVisual

└ HeadCollider



Core enemy components:



EnemyDefenseSystem

EnemyHitReceiver

EnemyBreakdownController



Enemy visuals must remain separated from logic.



\---



\# Development Workflow



All development follows this sequence.



1 Analyze existing system



2 Propose implementation plan



3 Wait for approval



4 Implement in small steps



5 Provide verification steps



Do not jump directly to large code changes.



\---



\# Code Modification Rules



Before modifying files:



1 Identify the system involved

2 Check related scripts

3 Confirm change scope



Rules



Do not modify unrelated files.

Avoid global refactors unless explicitly requested.



Prefer minimal safe changes.



\---



\# Validation Steps



After implementing a change, always verify:



Unity Console errors

Gameplay behavior

UI feedback



Provide manual testing steps.



\---



\# Current Development Mode



The project is currently in:



Prototype phase.



Focus on:



Gameplay clarity

Fast iteration

Readable behavior



Avoid premature optimization.



\---



\# Current Development Entry



The current task is always defined in:



docs/TASKS/current\_task.md



Always read it before starting work.



\---



\# AI Behavior Rules



When assisting development:



1 Explain reasoning clearly

2 Provide step-by-step plan

3 Avoid unnecessary complexity

4 Prefer simple maintainable systems



If information is missing, ask before implementing.



\---



END



