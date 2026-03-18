\# AI CONTEXT



\## Project



Power Prank 3D  

Unity 6.3 LTS  

Single-scene prototype



\## Core Loop



Player throws prank items → Enemy reacts → Enemy defends → Player breaks defense



\## Architecture Layers



1\. Gameplay Layer

&#x20;  - ThrowController

&#x20;  - GameplayManager



2\. Enemy Reaction Layer

&#x20;  - EnemyReactionLayerController

&#x20;  - Driven by EnemyArchetypeData



3\. Enemy Defense Layer

&#x20;  - EnemyDefenseController

&#x20;  - Driven by EnemyDefensePatternData



4\. Enemy Defense Window Layer

&#x20;  - EnemyDefenseStateWindowController

&#x20;  - Driven by EnemyDefenseStateWindowProfileData



5\. Enemy AI Layer

&#x20;  - EnemyAiLayerController

&#x20;  - Driven by EnemyAiProfileData



6\. Preset Layer

&#x20;  - EnemyPresetData

&#x20;  - Applied by EnemyPresetApplicator



\---



\## Current State (IMPORTANT)



System has moved from:



Static enemy behavior



→



Data-driven multi-archetype AI system



\---



\## Implemented Enemy Archetypes



\### 1. Meeting Tyrant



\- Early defense trigger

\- Strong guard

\- Short recover

\- Hard to break

\- Low weakness



\### 2. Narcissist Manager



\- Late defense trigger

\- Long telegraph

\- Short guard

\- Long recover

\- High head weakness

\- Easy to break



\---



\## Key Principle



ALL behavior differences come from DATA



NOT from:



\- hardcoded logic

\- branching in scripts



\---



\## AI Ownership



Defense cycle is controlled ONLY by:



EnemyAiLayerController



NOT by:



EnemyDefenseStateWindowController



(autoCycle must remain FALSE)

