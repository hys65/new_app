\# Project State



Project: Power Prank 3D  

Engine: Unity 6.3 LTS  

Repository: https://github.com/hys65/new\_app



\---



\# Current Development Stage



Enemy Visual Proxy 1.0 — Completed



The placeholder cylinder enemy has been replaced with a readable proxy body built from Unity primitives.



The proxy enemy now supports readable gameplay states:



Idle  

Prepare Defense  

Guard  

Defense Break  

Hit Reaction



This version intentionally avoids using Animator or character rigs.  

All visual state transitions are controlled via code-driven transform blending.



\---



\# Current Enemy Structure



EnemyRoot



EnemyBodyPivot  

DefenseBodyPivot  



Torso  

LeftArmPivot  

LeftArmVisual  



RightArmPivot  

RightArmVisual  



DefenseHeadPivot  

HeadVisual  

HeadCollider  



DefenseVisualAnchor  

GuardVisual  



\---



\# Current Gameplay Status



Working systems:



ThrowController  

ProjectileBehavior  

HudController  

HitPopupSpawner  



Enemy systems:



EnemyReactionLayerController  

EnemyDefenseController  

EnemyDefenseVisualLayerController  

EnemyDefenseStateWindowController  

EnemyAiLayerController  

EnemyPresetApplicator  

EnemyVisualProxyController



Two enemy archetypes exist:



Meeting Tyrant  

Narcissist Manager



\---



\# Known Limitations



Enemy AI behavior is currently minimal.



The enemy can defend but does not yet actively make strategic decisions.



Defense timing logic will be expanded in the next phase.



\---



\# Next Development Target



Enemy AI Layer 1.0



Goal:



Implement a basic enemy decision loop that determines when to:



Idle  

Prepare defense  

Guard  

Recover



The AI will introduce readable gameplay timing rather than passive defense triggers.



\---

