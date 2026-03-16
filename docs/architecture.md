# Power Prank 3D Architecture

Engine
Unity 6.3 LTS

Project Type
Single Scene Prototype

---

# System Overview

Player
↓
Throw System
↓
Projectile
↓
Hit Detection
↓
Enemy System
↓
Breakdown System
↓
HUD Update

---

# System Modules

Gameplay

ThrowSystem
Handles projectile spawning and trajectory calculation.

Combat
Handles hit detection and score calculation.

EnemyAI
Handles enemy behaviour and defense logic.

Items
Defines projectile item data.

Systems

GameplayManager
Global game state controller.

UI

HUD
Displays game status.

Effects

Stains
Impact visual decals.

---

# Enemy Logic

EnemyRoot

EnemyVisual
HeadCollider

Components

EnemyDefenseSystem
EnemyHitReceiver
EnemyBreakdownController

---

# Game Flow

Start Level
↓
Player Throws Item
↓
Projectile Hit
↓
Enemy Reaction
↓
Breakdown Increase
↓
Target Breakdown Reached
↓
Level Complete