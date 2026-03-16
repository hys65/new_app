\# AI Context



This repository contains a Unity prototype for the game:



Power Prank 3D



The project is a third-person prank throwing game where the player throws items at enemies.



The current prototype focuses on building gameplay systems first, with minimal art.



\---



\# Current Development Focus



Enemy AI Layer 1.0



The previous milestone "Enemy Visual Proxy 1.0" has been completed.



The enemy now has a readable proxy body built from primitives.



The next step is to implement AI behavior that controls when the enemy defends.



\---



\# Current Enemy Capabilities



The enemy currently supports:



Idle state  

Prepare defense state  

Guard state  

Defense break state  

Hit reaction feedback



Defense visuals are controlled through code-driven transform blending rather than Animator.



\---



\# Key Gameplay Systems



ThrowController  

ProjectileBehavior  

HudController  

HitPopupSpawner  



Enemy system components:



EnemyReactionLayerController  

EnemyDefenseController  

EnemyDefenseVisualLayerController  

EnemyDefenseStateWindowController  

EnemyAiLayerController  

EnemyPresetApplicator  

EnemyVisualProxyController



\---



\# Enemy Archetypes



Two archetypes currently exist:



Meeting Tyrant  

Narcissist Manager



These archetypes control different enemy behavior parameters.



\---



\# Important Development Constraints



No Animator system yet.



All enemy motion must be controlled via transforms and code.



The project currently uses a single prototype scene.



\---



\# Development Goal



Create a clear gameplay loop where the player must read enemy defense timing and throw accordingly.

