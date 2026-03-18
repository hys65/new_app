# AI RULES

## Core Principle

AI is predictive, not reactive

---

## Observation

AI tracks:

- hit timestamps
- interval consistency

---

## Decision Conditions

Defense triggers when:

- sufficient samples collected
- predicted next hit exists
- threat exceeds threshold

---

## Defense Flow

Observe
→ PrepareDefense
→ Guard
→ Recover

---

## Important Constraints

### 1. AI owns defense start

NOT allowed:

- DefenseWindow auto cycle

### 2. Data controls behavior

DO NOT:

- hardcode behavior in controller

### 3. No randomness-first design

Random is secondary modifier

---

## Break Behavior

On break:

- enter recover lock
- clear observation memory
- reset prediction

---

## Design Goal

Enemy feels:

- readable
- fair
- reactive
- different per archetype