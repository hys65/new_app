\# Project State



Project: Power Prank 3D  

Engine: Unity 6.3 LTS  

Repository: https://github.com/hys65/new\_app



\---



\# Current Development Stage



Enemy AI Layer 1.0 — Completed



The enemy now supports a readable AI-driven defense loop on top of the existing defense window system.



Implemented AI gameplay states:



\- Idle

\- Observe

\- Prepare Defense

\- Guard

\- Recover



The AI does not directly replace the defense system.

Instead, it decides when to start a defense cycle, while the defense window system still controls the actual phase flow:



\- None

\- Telegraph

\- Active

\- Recover



This keeps the system modular and preserves the existing defense logic.



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



Core enemy components:



\- EnemyReactionLayerController

\- EnemyDefenseController

\- EnemyDefenseStateWindowController

\- EnemyDefenseVisualLayerController

\- EnemyAiLayerController

\- EnemyPresetApplicator

\- EnemyVisualProxyController



\---



\# Current Gameplay Status



Working systems:



\- ThrowController

\- ProjectileBehavior

\- HudController

\- HitPopupSpawner



Enemy systems:



\- EnemyReactionLayerController

\- EnemyDefenseController

\- EnemyDefenseStateWindowController

\- EnemyDefenseVisualLayerController

\- EnemyAiLayerController

\- EnemyPresetApplicator

\- EnemyVisualProxyController



Two enemy archetypes exist:



\- Meeting Tyrant

\- Narcissist Manager



Enemy defense behavior now follows a readable gameplay loop:



\- Observe player throw rhythm

\- Predict likely next hit timing

\- Start prepare-defense window

\- Enter active guard window

\- Recover after defense

\- Re-enter observation



The proxy body remains code-driven and does not use Animator.



\---



\# Verified Gameplay Behavior



Expected verified loop:



\- Light or irregular throws keep enemy in Idle or Observe

\- Repeated rhythmic throws increase AI confidence

\- Enemy enters Prepare Defense before Guard

\- During Guard, BLOCK is shown and Breakdown does not increase

\- After defense, enemy enters Recover

\- After defense break, enemy enters recover lock before defending again



\---



\# Known Limitations



Enemy AI is currently a basic timing-based defense loop.



Current limitations:



\- No advanced bait / fake-out behavior

\- No archetype-specific tactical pattern yet

\- No animation rig or Animator layer yet

\- AI readability depends on proxy poses and defense windows



\---



\# Next Development Target



Enemy Archetype Behavior 1.0



Goal:



Different enemy archetypes should not feel identical.



Next step:



\- Give each archetype different defense timing

\- Vary trigger threshold, lead time, and recovery feel

\- Make Meeting Tyrant and Narcissist Manager readable as different opponents

