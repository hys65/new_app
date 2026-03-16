\# Power Prank 3D – AI Context



Project repository  

https://github.com/hys65/new\_app



This file is the entry point for AI assistants.



Before giving any suggestion:

1\. Read this file

2\. Read docs/architecture.md

3\. Read docs/enemy\_system.md

4\. Read docs/gameplay\_systems.md

5\. Check the GitHub repository structure



\---



\# Project Overview



Power Prank 3D is a Unity 6.3 LTS prototype.



Core gameplay:



Player throws prank items at enemies.



Items include:



\- Egg

\- Tomato

\- Paint Ball

\- Foam Sprayer

\- Hammer



Enemy systems include:



\- Reaction Layer

\- Defense System

\- AI Layer

\- Defense Visual Layer

\- Defense State Window



The game is designed around \*\*readable enemy reactions\*\* instead of complex animation systems.



\---



\# Current State



Implemented systems:



Projectile system  

Hit popup system  

Breakdown score system  

Enemy defense system  

Enemy reaction layer  

Enemy AI layer  

Enemy preset system  

Enemy level config system  



Two enemy archetypes exist:



Meeting Tyrant  

Narcissist Manager



Differences:



Meeting Tyrant  

\- earlier defense

\- hammer easier to break defense



Narcissist Manager  

\- later defense

\- foam easier to break defense



\---



\# Unity Environment



Unity version:



Unity 6.3 LTS



Single scene prototype.



Enemy currently uses procedural motion instead of Animator.



\---



\# Naming Rules



Scripts use PascalCase.



Example:



EnemyDefenseController.cs  

EnemyReactionLayerController.cs  



ScriptableObjects use snake\_case.



Example:



enemy\_preset\_meeting\_tyrant  

enemy\_archetype\_narcissist\_manager  

defense\_pattern\_meeting\_tyrant  



\---



\# File Placement Rules



Scripts



Assets/Scripts/gameplay/



ScriptableObjects



Assets/ScriptableObjects/Enemy/



Scenes



Assets/Scenes/



Documentation



docs/



\---



\# AI Coding Rules



When modifying scripts:



1\. Always check the repository first

2\. Never guess code structure

3\. Provide \*\*complete replacement file\*\*

4\. Do not suggest partial edits

5\. Respect existing architecture



\---



\# Current Development Focus



Enemy Visual Proxy 1.0



Goal:



Replace simple cylinder enemy with a readable proxy body built from Unity primitives.



Expected hierarchy:



EnemyRoot  

EnemyBodyPivot  

Torso  

HeadAnchor  

HeadVisual  

HeadCollider  

LeftArmPivot  

LeftArmVisual  

RightArmPivot  

RightArmVisual  

DefenseVisualAnchor  

GuardVisual



This proxy must visually communicate:



Idle  

Prepare defense  

Guard  

Hit reaction  

Defense break



\---



\# Session Log



See:



docs/SESSION\_LOG.md

