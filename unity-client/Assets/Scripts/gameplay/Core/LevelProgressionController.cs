using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelProgressionController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelProgressionData progressionData;

        [Header("References")]
        [SerializeField] private LevelEncounterController levelEncounterController;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = true;

        [Header("Runtime Debug")]
        [SerializeField] private int currentLevelIndex = -1;
        [SerializeField] private LevelEncounterConfigData currentEncounterConfig;

        public int CurrentLevelIndex => currentLevelIndex;
        public LevelEncounterConfigData CurrentEncounterConfig => currentEncounterConfig;

        private void Reset()
        {
            if (levelEncounterController == null)
            {
                levelEncounterController = FindFirstObjectByType<LevelEncounterController>();
            }
        }

        private void Awake()
        {
            if (applyOnAwake)
            {
                ApplyStartupLevel();
            }
        }

        [ContextMenu("Apply Startup Level")]
        public void ApplyStartupLevel()
        {
            if (progressionData == null)
            {
                Debug.LogWarning("LevelProgressionController: progressionData is null", this);
                return;
            }

            int startupIndex = progressionData.GetSafeStartupLevelIndex();
            if (startupIndex < 0)
            {
                Debug.LogWarning("LevelProgressionController: no valid startup level found", this);
                return;
            }

            ApplyLevelByIndex(startupIndex);
        }

        public void ApplyLevelByIndex(int levelIndex)
        {
            if (progressionData == null)
            {
                Debug.LogWarning("LevelProgressionController: progressionData is null", this);
                return;
            }

            if (levelEncounterController == null)
            {
                Debug.LogWarning("LevelProgressionController: levelEncounterController is null", this);
                return;
            }

            LevelEncounterConfigData encounterConfig = progressionData.GetLevelAt(levelIndex);
            if (encounterConfig == null)
            {
                Debug.LogWarning(
                    "LevelProgressionController: encounter config not found at index: " + levelIndex,
                    this);
                return;
            }

            currentLevelIndex = levelIndex;
            currentEncounterConfig = encounterConfig;

            levelEncounterController.SetEncounterConfig(encounterConfig);
            levelEncounterController.ApplyEncounter();

            Debug.Log(
                "LevelProgressionController: applied level index: " + currentLevelIndex +
                " config: " + encounterConfig.displayName,
                this);
        }
    }
}
