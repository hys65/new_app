using System.Collections;
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

        private Coroutine deferredApplyRoutine;

        private void Reset()
        {
            if (enemySwitchingManager == null)
            {
                enemySwitchingManager = FindFirstObjectByType<EnemySwitchingManager>();
            }
        }

        private void Awake()
        {
            if (enemySwitchingManager == null)
            {
                enemySwitchingManager = FindFirstObjectByType<EnemySwitchingManager>();
            }
        }

        private void OnDisable()
        {
            if (deferredApplyRoutine != null)
            {
                StopCoroutine(deferredApplyRoutine);
                deferredApplyRoutine = null;
            }
        }

        private void Start()
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

        public void SetSelectionData(LevelEnemySelectionData data)
        {
            selectionData = data;
        }

        public void ApplySelection(LevelEnemySelectionData data)
        {
            selectionData = data;

            if (deferredApplyRoutine != null)
            {
                StopCoroutine(deferredApplyRoutine);
            }

            deferredApplyRoutine = StartCoroutine(ApplySelectionDeferred(selectionData));
        }

        private IEnumerator ApplySelectionDeferred(LevelEnemySelectionData data)
        {
            // 等一帧，让 EnemySwitchingManager.Start() 和其他启动逻辑先跑完
            yield return null;

            bool applied = TryApplySelection(data, logWarnings: true);
            if (!applied)
            {
                deferredApplyRoutine = null;
                yield break;
            }

            deferredApplyRoutine = null;
        }

        private bool TryApplySelection(LevelEnemySelectionData data, bool logWarnings)
        {
            if (data == null)
            {
                if (logWarnings)
                {
                    Debug.LogWarning("LevelEnemySelectionController: selection data is null", this);
                }

                return false;
            }

            if (enemySwitchingManager == null)
            {
                if (logWarnings)
                {
                    Debug.LogWarning("LevelEnemySelectionController: enemySwitchingManager is null", this);
                }

                return false;
            }

            if (data.roster == null)
            {
                if (logWarnings)
                {
                    Debug.LogWarning("LevelEnemySelectionController: roster is null", this);
                }

                return false;
            }

            if (data.selectedEnemies == null || data.selectedEnemies.Length == 0)
            {
                if (logWarnings)
                {
                    Debug.LogWarning("LevelEnemySelectionController: selectedEnemies is empty", this);
                }

                return false;
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
                    if (logWarnings)
                    {
                        Debug.LogWarning(
                            "LevelEnemySelectionController: roster entry not found: " +
                            selectionEntry.rosterEntryId,
                            this);
                    }

                    continue;
                }

                string targetSlotId = selectionEntry.useRosterRecommendedSlot
                    ? rosterEntry.recommendedSlotId
                    : selectionEntry.overrideSlotId;

                if (string.IsNullOrWhiteSpace(targetSlotId))
                {
                    if (logWarnings)
                    {
                        Debug.LogWarning(
                            "LevelEnemySelectionController: targetSlotId is empty for roster entry: " +
                            selectionEntry.rosterEntryId,
                            this);
                    }

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
                if (logWarnings)
                {
                    Debug.LogWarning(
                        "LevelEnemySelectionController: could not resolve startup slot index for level: " +
                        data.levelId,
                        this);
                }

                return false;
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
                "LevelEnemySelectionController: applied level selection: " + data.displayName +
                " startupSlotIndex=" + resolvedStartupSlotIndex,
                this);

            return true;
        }
    }
}
