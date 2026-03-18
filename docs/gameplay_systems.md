# GAMEPLAY SYSTEMS

## Core Loop

Throw → Hit → Reaction → Defense → Break

---

## Hit Result Types

1. Normal Hit
2. Block
3. Break

---

## Breakdown

- Increases on successful hits
- Pauses during Block
- Accelerates on weakness hits

---

## Interaction with Enemy Systems

### Reaction Layer
Controls visual feedback

### Defense Pattern
Controls whether hit is blocked

### Defense Window
Controls WHEN block is valid

### AI Layer
Controls WHEN defense starts

---

## Key Rule

Block only valid during:

Defense Window → Active

---

## Weak Window

Inside Active state:

- Vulnerable timing window
- Allows skilled hits

---

## Player Skill Expression

Player can:

- Read Telegraph
- Time attacks
- Exploit Weak Window
- Break defense

---

## Result

Gameplay is NOT spam

Gameplay = timing + rhythm + reading enemy