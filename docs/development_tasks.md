# Development Tasks

This file tracks the current milestone and upcoming tasks.

---

# Current Milestone

Enemy Archetype Behavior 1.0

Goal:

Different enemies should have different defense timing, recovery feel, and tactical identity.

The shared AI loop now exists.
The next task is to differentiate behavior between archetypes rather than expanding the base loop again.

Target directions:

- Meeting Tyrant should feel more rigid and earlier to defend
- Narcissist Manager should feel more performative and less stable
- Archetypes should vary in:
  - defense trigger threshold
  - lead time
  - recover duration
  - reaction to repeated head hits
  - break vulnerability

---

# Previous Milestone

Enemy AI Layer 1.0 — Completed

Completed results:

- Added a readable AI state loop
- AI states now map to gameplay readability:
  - Idle
  - Observe
  - Prepare Defense
  - Guard
  - Recover
- AI now decides when to start a defense cycle
- Defense window system still controls Telegraph / Active / Recover
- Existing defense logic was preserved
- Break recovery lock remains supported

---

# Earlier Milestone

Enemy Visual Proxy 1.0 — Completed

The enemy proxy body clearly communicates gameplay states using primitive geometry.

States supported:

- Idle
- Prepare Defense
- Guard
- Defense Break
- Hit Reaction

---

# Next Milestones

## Enemy Archetype Behavior 1.0

Add archetype-specific behavior parameters so enemies no longer feel identical.

## Enemy Feedback Polish

Improve readability of telegraph, guard, break, and recover transitions.

## Enemy Animation Layer (future)

Eventually replace proxy poses with real animation while preserving the gameplay timing structure.