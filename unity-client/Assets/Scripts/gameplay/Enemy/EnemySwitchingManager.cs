using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemySwitchingManager : MonoBehaviour
    {
        [System.Serializable]
        public class EnemySlot
        {
            public string slotId = "enemy_slot_01";
            public string displayName = "Enemy 01";

            [Header("Scene References")]
            public GameObject enemyRoot;
            public EnemyRuntimePresetController runtimePresetController;
            public EnemyReactionLayerController reactionLayer;

            [Header("Default Data")]
            public EnemyPresetData defaultPreset;
        }

        [Header("References")]
        [SerializeField] private GameplayManager gameplayManager;

        [Header("Slots")]
        [SerializeField] private EnemySlot[] enemySlots;

        [Header("Startup")]
        [SerializeField] private int startingSlotIndex = 0;
        [SerializeField] private bool autoApplyDefaultPresetOnStart = true;
        [SerializeField] private bool deactivateNonActiveEnemiesOnStart = true;

        [Header("Runtime Debug")]
        [SerializeField] private int currentSlotIndex = -1;
        [SerializeField] private string currentSlotId;
        [SerializeField] private bool startupApplied;

        public int CurrentSlotIndex => currentSlotIndex;

        public EnemySlot CurrentSlot
        {
            get
            {
                if (!IsValidIndex(currentSlotIndex))
                {
                    return null;
                }

                return enemySlots[currentSlotIndex];
            }
        }

        public int SlotCount => enemySlots != null ? enemySlots.Length : 0;

        private void Reset()
        {
            if (gameplayManager == null)
            {
                gameplayManager = FindFirstObjectByType<GameplayManager>();
            }
        }

        private void Awake()
        {
            if (gameplayManager == null)
            {
                gameplayManager = FindFirstObjectByType<GameplayManager>();
            }
        }

        private void Start()
        {
            if (enemySlots == null || enemySlots.Length == 0)
            {
                Debug.LogWarning("EnemySwitchingManager: no enemy slots configured", this);
                return;
            }

            ApplyStartupSlotIfNeeded();
        }

        [ContextMenu("Apply Startup Slot")]
        public void ApplyStartupSlotIfNeeded()
        {
            if (startupApplied)
            {
                return;
            }

            startupApplied = true;
            SwitchToSlot(startingSlotIndex, autoApplyDefaultPresetOnStart);
        }

        [ContextMenu("Switch To Next Slot")]
        public void SwitchToNextSlot()
        {
            if (enemySlots == null || enemySlots.Length == 0)
            {
                Debug.LogWarning("EnemySwitchingManager: no enemy slots configured", this);
                return;
            }

            int nextIndex = currentSlotIndex + 1;
            if (nextIndex >= enemySlots.Length)
            {
                nextIndex = 0;
            }

            SwitchToSlot(nextIndex, false);
        }

        [ContextMenu("Apply Current Slot Default Preset")]
        public void ApplyCurrentSlotDefaultPreset()
        {
            if (!IsValidIndex(currentSlotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: current slot index is invalid", this);
                return;
            }

            ApplyDefaultPresetToSlot(currentSlotIndex);
        }

        public void ConfigureStartupSlot(int slotIndex, bool autoApplyDefaultPreset)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: startup slot index is invalid: " + slotIndex, this);
                return;
            }

            startingSlotIndex = slotIndex;
            autoApplyDefaultPresetOnStart = autoApplyDefaultPreset;
        }

        public int FindSlotIndexBySlotId(string slotId)
        {
            if (enemySlots == null || enemySlots.Length == 0)
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(slotId))
            {
                return -1;
            }

            for (int i = 0; i < enemySlots.Length; i++)
            {
                EnemySlot slot = enemySlots[i];
                if (slot == null)
                {
                    continue;
                }

                if (string.Equals(slot.slotId, slotId, System.StringComparison.Ordinal))
                {
                    return i;
                }
            }

            return -1;
        }

        public void ClearAllSlotDefaultPresets()
        {
            if (enemySlots == null)
            {
                return;
            }

            for (int i = 0; i < enemySlots.Length; i++)
            {
                EnemySlot slot = enemySlots[i];
                if (slot == null)
                {
                    continue;
                }

                slot.defaultPreset = null;
            }
        }

        public bool ConfigureSlotDefaultPreset(string slotId, EnemyPresetData preset)
        {
            int slotIndex = FindSlotIndexBySlotId(slotId);
            if (slotIndex < 0)
            {
                Debug.LogWarning("EnemySwitchingManager: slotId not found: " + slotId, this);
                return false;
            }

            return ConfigureSlotDefaultPreset(slotIndex, preset);
        }

        public bool ConfigureSlotDefaultPreset(int slotIndex, EnemyPresetData preset)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: slot index is invalid: " + slotIndex, this);
                return false;
            }

            EnemySlot slot = enemySlots[slotIndex];
            if (slot == null)
            {
                Debug.LogWarning("EnemySwitchingManager: slot is null at index: " + slotIndex, this);
                return false;
            }

            slot.defaultPreset = preset;
            return true;
        }

        public void SwitchToSlot(int slotIndex, bool applyDefaultPreset)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: slot index is invalid: " + slotIndex, this);
                return;
            }

            currentSlotIndex = slotIndex;

            EnemySlot currentSlot = enemySlots[currentSlotIndex];
            currentSlotId = currentSlot != null ? currentSlot.slotId : string.Empty;

            RefreshSceneActivation();

            if (applyDefaultPreset)
            {
                ApplyDefaultPresetToSlot(currentSlotIndex);
            }

            // 强制兜底：当前槽位对象必须处于激活状态
            if (currentSlot != null && currentSlot.enemyRoot != null)
            {
                currentSlot.enemyRoot.SetActive(true);
            }

            if (gameplayManager != null && currentSlot != null && currentSlot.reactionLayer != null)
            {
                gameplayManager.SetActiveEnemyReactionLayer(currentSlot.reactionLayer);
            }

            Debug.Log(
                "EnemySwitchingManager: switched to slot index=" + currentSlotIndex +
                " slotId=" + currentSlotId,
                this);
        }

        private void RefreshSceneActivation()
        {
            if (enemySlots == null || enemySlots.Length == 0)
            {
                return;
            }

            for (int i = 0; i < enemySlots.Length; i++)
            {
                EnemySlot slot = enemySlots[i];
                if (slot == null || slot.enemyRoot == null)
                {
                    continue;
                }

                if (deactivateNonActiveEnemiesOnStart)
                {
                    bool shouldBeActive = i == currentSlotIndex;
                    slot.enemyRoot.SetActive(shouldBeActive);
                }
                else if (i == currentSlotIndex)
                {
                    // 即使不关闭其他敌人，也要确保当前敌人被显式打开
                    slot.enemyRoot.SetActive(true);
                }
            }
        }

        public void ApplyDefaultPresetToSlot(int slotIndex)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: slot index is invalid: " + slotIndex, this);
                return;
            }

            EnemySlot slot = enemySlots[slotIndex];
            if (slot == null)
            {
                Debug.LogWarning("EnemySwitchingManager: slot is null", this);
                return;
            }

            if (slot.runtimePresetController == null)
            {
                Debug.LogWarning("EnemySwitchingManager: runtimePresetController is null on slot: " + slotIndex, this);
                return;
            }

            if (slot.defaultPreset == null)
            {
                Debug.LogWarning("EnemySwitchingManager: defaultPreset is null on slot: " + slotIndex, this);
                return;
            }

            slot.runtimePresetController.ApplyPreset(slot.defaultPreset);
        }

        public void ApplyPresetToCurrentSlot(EnemyPresetData preset)
        {
            if (!IsValidIndex(currentSlotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: current slot index is invalid", this);
                return;
            }

            ApplyPresetToSlot(currentSlotIndex, preset);
        }

        public void ApplyPresetToSlot(int slotIndex, EnemyPresetData preset)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("EnemySwitchingManager: slot index is invalid: " + slotIndex, this);
                return;
            }

            EnemySlot slot = enemySlots[slotIndex];
            if (slot == null || slot.runtimePresetController == null)
            {
                Debug.LogWarning("EnemySwitchingManager: runtimePresetController is missing on slot: " + slotIndex, this);
                return;
            }

            slot.runtimePresetController.ApplyPreset(preset);

            if (slotIndex == currentSlotIndex)
            {
                if (slot.enemyRoot != null)
                {
                    slot.enemyRoot.SetActive(true);
                }

                if (gameplayManager != null && slot.reactionLayer != null)
                {
                    gameplayManager.SetActiveEnemyReactionLayer(slot.reactionLayer);
                }
            }
        }

        public EnemyRuntimePresetController GetCurrentRuntimePresetController()
        {
            EnemySlot slot = CurrentSlot;
            return slot != null ? slot.runtimePresetController : null;
        }

        private bool IsValidIndex(int index)
        {
            return enemySlots != null && index >= 0 && index < enemySlots.Length;
        }
    }
}
