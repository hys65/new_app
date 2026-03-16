# AI Development Rules

Project
Power Prank 3D

Engine
Unity 6.3 LTS

Language
C#

---

# Naming Rules

Files

lowercase_with_underscore

example

enemy_meeting_tyrant
enemy_narcissist_boss
enemy_defense_system

Scripts

PascalCase

example

EnemyDefenseSystem
ThrowController
GameplayManager

---

# Architecture Rules

Never mix systems.

Each system must be isolated.

Example

ThrowSystem
EnemySystem
CombatSystem
UISystem

---

# Unity Rules

Scripts must not exceed 300 lines.

Separate logic and visuals.

Example

EnemyDefenseSystem
EnemyDefenseVisual

---

# Gameplay Rules

Body Hit
+10 breakdown

Head Hit
+20 breakdown

BLOCK state

Breakdown must not increase.

---

# AI Development Rules

Before writing code

1 analyze existing system
2 propose plan
3 wait for approval

Never modify unrelated files.

Always explain changes.