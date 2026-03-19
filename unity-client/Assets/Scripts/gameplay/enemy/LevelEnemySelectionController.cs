using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelEnemySelectionController : MonoBehaviour
    {
        [Header("Level Selection")]
        [SerializeField] private LevelEnemySelectionData levelSelection;

        [Header("References")]
        [SerializeField] private EnemySwitchingManager enemySwitchingManager;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = true;

        [Header("Debug")]
        [SerializeField] private bool debugLog = true;

        private void Reset()
        {
            if (enemySwitchingManager == null)
            {
                enemySwitchingManager = FindFirstObjectByType<EnemySwitchingManager>();
            }
        }

        private void Awake()
        {
            if (!applyOnAwake)
            {
                return;
            }

            ApplyLevelSelection();
        }

        [ContextMenu("Apply Level Selection")]
        public void ApplyLevelSelection()
        {
            if (levelSelection == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: levelSelection is null", this);
                return;
            }

            if (enemySwitchingManager == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: enemySwitchingManager is null", this);
                return;
            }

            if (levelSelection.roster == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: roster is null", this);
                return;
            }

            if (levelSelection.clearUnassignedSlotPresets)
            {
                enemySwitchingManager.ClearAllSlotDefaultPresets();
            }

            LevelEnemySelectionEntry[] selections = levelSelection.selectedEnemies;
            if (selections == null || selections.Length == 0)
            {
                Debug.LogWarning("LevelEnemySelectionController: selectedEnemies is empty", this);
                return;
            }

            int resolvedStartupSlotIndex = -1;

            for (int i = 0; i < selections.Length; i++)
            {
                LevelEnemySelectionEntry selection = selections[i];
                if (selection == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(selection.rosterEntryId))
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: rosterEntryId is empty at selection index {i}",
                        this);
                    continue;
                }

                EnemyRosterEntry rosterEntry = levelSelection.roster.GetEntryById(selection.rosterEntryId);
                if (rosterEntry == null)
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: roster entry not found: {selection.rosterEntryId}",
                        this);
                    continue;
                }

                if (!rosterEntry.enabled)
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: roster entry is disabled: {selection.rosterEntryId}",
                        this);
                    continue;
                }

                if (rosterEntry.preset == null)
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: preset is null on roster entry: {selection.rosterEntryId}",
                        this);
                    continue;
                }

                string resolvedSlotId = ResolveSlotId(selection, rosterEntry);
                if (string.IsNullOrWhiteSpace(resolvedSlotId))
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: resolvedSlotId is empty for roster entry: {selection.rosterEntryId}",
                        this);
                    continue;
                }

                int slotIndex = enemySwitchingManager.FindSlotIndexBySlotId(resolvedSlotId);
                if (slotIndex < 0)
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: slot not found for slotId: {resolvedSlotId}",
                        this);
                    continue;
                }

                bool configured = enemySwitchingManager.ConfigureSlotDefaultPreset(slotIndex, rosterEntry.preset);
                if (!configured)
                {
                    Debug.LogWarning(
                        $"LevelEnemySelectionController: failed to configure slot {slotIndex} for roster entry: {selection.rosterEntryId}",
                        this);
                    continue;
                }

                if (i == levelSelection.startupSelectionIndex)
                {
                    resolvedStartupSlotIndex = slotIndex;
                }

                if (debugLog)
                {
                    Debug.Log(
                        $"LevelEnemySelectionController: assigned rosterEntry={selection.rosterEntryId} " +
                        $"to slotId={resolvedSlotId} (slotIndex={slotIndex})",
                        this);
                }
            }

            if (resolvedStartupSlotIndex >= 0)
            {
                enemySwitchingManager.ConfigureStartupSlot(
                    resolvedStartupSlotIndex,
                    levelSelection.autoApplyDefaultPresetOnStart);

                if (debugLog)
                {
                    Debug.Log(
                        $"LevelEnemySelectionController: startup slot resolved to index {resolvedStartupSlotIndex}",
                        this);
                }
            }
            else
            {
                Debug.LogWarning(
                    "LevelEnemySelectionController: startup selection could not be resolved; keeping manager startup as-is",
                    this);
            }
        }

        private string ResolveSlotId(LevelEnemySelectionEntry selection, EnemyRosterEntry rosterEntry)
        {
            if (selection.useRosterRecommendedSlot)
            {
                return rosterEntry.recommendedSlotId;
            }

            if (!string.IsNullOrWhiteSpace(selection.overrideSlotId))
            {
                return selection.overrideSlotId;
            }

            return rosterEntry.recommendedSlotId;
        }
    }
}
