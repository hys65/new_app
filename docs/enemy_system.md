# Enemy System

The enemy system controls all defensive behavior, hit reactions, and AI decisions.

The system is designed to be modular and layered.

---

# Enemy Layers

Enemy behavior is split into several layers.

Reaction Layer  
Handles hit reactions and breakdown score changes.

Defense Layer  
Controls whether the enemy is currently guarding or vulnerable.

Defense Visual Layer  
Controls visual feedback for defensive states.

AI Layer  
Determines when the enemy should enter defense states.

Preset Applicator  
Applies archetype parameters to enemy instances.

Visual Proxy Controller  
Controls primitive-based body poses.

---

# Enemy Visual Proxy

Enemy Visual Proxy 1.0 replaces the original placeholder cylinder.

The proxy body is built from Unity primitives:

Torso  
Head  
Left Arm  
Right Arm  
Guard Visual

The proxy supports readable gameplay states without requiring a full character rig.

---

# Enemy Defense States

Idle  
Enemy is vulnerable.

Prepare Defense  
Enemy telegraphs that a guard is about to happen.

Guard  
Enemy blocks incoming attacks.

Break  
Enemy defense has been broken.

Weak  
Enemy temporarily vulnerable after break.

---

# Visual Control

EnemyVisualProxyController drives the body pose using transform blending.

Pose states include:

Idle Pose  
Telegraph Pose  
Guard Pose  
Break Pose

Each pose defines transform offsets for:

Torso  
Head  
Arms  
Guard Visual

---

# AI Control

EnemyAiLayerController will determine when the enemy transitions between:

Idle  
Prepare  
Guard  
Recover

The AI will introduce timing-based gameplay rather than passive reactions.

---

# Current Status

Enemy Visual Proxy 1.0 completed.

Enemy AI behavior is the next development milestone.