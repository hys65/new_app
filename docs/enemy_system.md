# Enemy System

Current Enemy

Placeholder cylinder model.

Structure

EnemyRoot
 ├ EnemyVisual
 └ HeadCollider

---

# Enemy Components

EnemyReactionLayerController
Handles procedural enemy motion and hit reactions.

EnemyDefenseController
Handles defense state, block logic and break defense.

EnemyDefenseVisualLayerController
Handles guard visual feedback.

EnemyDefenseStateWindowController
Controls defense timing windows.

EnemyAiLayerController
Handles enemy behavior prediction.

EnemyPresetApplicator
Applies enemy preset configuration.

---

# Defense Behaviour

Enemy can enter defense state.

When defense active

Breakdown does not increase.

HUD shows

BLOCK

---

# Enemy Archetypes

Meeting Tyrant

Uses briefcase to block attacks.

Narcissist Boss

Defense actions

Adjust glasses
Fix hair
Sunglasses protection

---

# Current Limitation

Enemy model is placeholder.

No limbs
No facial expression

Defense actions not visually readable.

---

# Next Step

Enemy Defense Visual Layer

Goal

Clear defense animations
Clear hit reactions
