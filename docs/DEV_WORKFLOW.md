# DEV WORKFLOW

## Rule 1

DO NOT modify multiple systems at once

---

## Rule 2

Always follow order:

1. Data
2. Binding
3. Runtime verification

---

## Rule 3

When adding new enemy:

1. Create ArchetypeData
2. Create DefensePatternData
3. Create AiProfileData
4. Create WindowProfileData
5. Create EnemyPresetData
6. Bind via EnemyPresetApplicator
7. Test behavior difference

---

## Rule 4

If behavior is wrong:

Check in order:

1. Preset references
2. Applicator execution
3. AI runtime values
4. Window state
5. Defense result

---

## Rule 5

Prefer tuning data over modifying code

---

## Rule 6

Every milestone must be documented