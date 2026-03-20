using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelEnemySelectionController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemySwitchingManager enemySwitchingManager;

        [Header("Config")]
        [SerializeField] private LevelEnemySelectionData selectionData;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = false;

        [Header("Runtime Debug")]
        [SerializeField] private string lastAppliedLevelId;
        [SerializeField] private int lastResolvedStartupSlotIndex = -1;

        private void Reset()
        {
            if (enemySwitchingManager == null)
            {
                enemySwitchingManager = FindFirstObjectByType<EnemySwitchingManager>();
            }
        }

        private void Awake()
        {
            if (applyOnAwake && selectionData != null)
            {
                ApplySelection(selectionData);
            }
        }

        [ContextMenu("Apply Current Selection")]
        public void ApplyCurrentSelection()
        {
            ApplySelection(selectionData);
        }

        public void ApplySelection(LevelEnemySelectionData data)
        {
            if (data == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: selection data is null", this);
                return;
            }

            if (enemySwitchingManager == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: enemySwitchingManager is null", this);
                return;
            }

            if (data.roster == null)
            {
                Debug.LogWarning("LevelEnemySelectionController: roster is null", this);
                return;
            }

            if (data.selectedEnemies == null || data.selectedEnemies.Length == 0)
            {
                Debug.LogWarning("LevelEnemySelectionController: selectedEnemies is empty", this);
                return;
            }

            if (data.clearUnassignedSlotPresets)
            {
                enemySwitchingManager.ClearAllSlotDefaultPresets();
            }

            int startupEntryIndex = Mathf.Clamp(
                data.startupSelectionIndex,
                0,
                data.selectedEnemies.Length - 1);

            int resolvedStartupSlotIndex = -1;

            for (int i = 0; i < data.selectedEnemies.Length; i++)
            {
                LevelEnemySelectionEntry selectionEntry = data.selectedEnemies[i];
                if (selectionEntry == null)
                {
                    continue;
                }

                EnemyRosterEntry rosterEntry = data.roster.GetEntryById(selectionEntry.rosterEntryId);
                if (rosterEntry == null)
                {
                    Debug.LogWarning(
                        "LevelEnemySelectionController: roster entry not found: " + selectionEntry.rosterEntryId,
                        this);
                    continue;
                }

                string targetSlotId = selectionEntry.useRosterRecommendedSlot
                    ? rosterEntry.recommendedSlotId
                    : selectionEntry.overrideSlotId;

                if (string.IsNullOrWhiteSpace(targetSlotId))
                {
                    Debug.LogWarning(
                        "LevelEnemySelectionController: targetSlotId is empty for roster entry: " + selectionEntry.rosterEntryId,
                        this);
                    continue;
                }

                bool configured = enemySwitchingManager.ConfigureSlotDefaultPreset(
                    targetSlotId,
                    rosterEntry.preset);

                if (!configured)
                {
                    continue;
                }

                if (i == startupEntryIndex)
                {
                    resolvedStartupSlotIndex = enemySwitchingManager.FindSlotIndexBySlotId(targetSlotId);
                }
            }

            if (resolvedStartupSlotIndex < 0)
            {
                Debug.LogWarning(
                    "LevelEnemySelectionController: could not resolve startup slot index for level: " + data.levelId,
                    this);
                return;
            }

            enemySwitchingManager.ConfigureStartupSlot(
                resolvedStartupSlotIndex,
                data.autoApplyDefaultPresetOnStart);

            enemySwitchingManager.SwitchToSlot(
                resolvedStartupSlotIndex,
                data.autoApplyDefaultPresetOnStart);

            lastAppliedLevelId = data.levelId;
            lastResolvedStartupSlotIndex = resolvedStartupSlotIndex;

            Debug.Log(
                "LevelEnemySelectionController: applied level selection: " + data.displayName,
                this);
        }
    }
}
