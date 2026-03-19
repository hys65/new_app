using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelEncounterController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelEncounterConfigData encounterConfig;

        [Header("References")]
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private LevelEnemySelectionController levelEnemySelectionController;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = true;
        [SerializeField] private bool startRoundOnStart = true;

        [Header("Runtime Debug")]
        [SerializeField] private bool encounterApplied;

        public LevelEncounterConfigData EncounterConfig => encounterConfig;
        public bool EncounterApplied => encounterApplied;

        private void Reset()
        {
            if (gameplayManager == null)
            {
                gameplayManager = FindFirstObjectByType<GameplayManager>();
            }

            if (levelEnemySelectionController == null)
            {
                levelEnemySelectionController = FindFirstObjectByType<LevelEnemySelectionController>();
            }
        }

        private void Awake()
        {
            if (applyOnAwake)
            {
                ApplyEncounter();
            }
        }

        private void Start()
        {
            if (!encounterApplied)
            {
                return;
            }

            bool shouldStartRound =
                startRoundOnStart &&
                encounterConfig != null &&
                encounterConfig.autoStartRound &&
                gameplayManager != null &&
                !gameplayManager.IsRoundRunning;

            if (shouldStartRound)
            {
                gameplayManager.StartRound();
            }
        }

        public void SetEncounterConfig(LevelEncounterConfigData config)
        {
            encounterConfig = config;
            encounterApplied = false;
        }

        [ContextMenu("Apply Encounter")]
        public void ApplyEncounter()
        {
            if (encounterConfig == null)
            {
                Debug.LogWarning("LevelEncounterController: encounterConfig is null", this);
                return;
            }

            if (gameplayManager == null)
            {
                Debug.LogWarning("LevelEncounterController: gameplayManager is null", this);
                return;
            }

            gameplayManager.SetAutoStartOnStart(false);
            gameplayManager.ApplyEncounterSettings(
                encounterConfig.targetBreakdownValue,
                encounterConfig.roundDurationSeconds);

            if (levelEnemySelectionController != null && encounterConfig.enemySelection != null)
            {
                levelEnemySelectionController.ApplySelection(encounterConfig.enemySelection);
            }

            encounterApplied = true;

            Debug.Log(
                "LevelEncounterController: applied encounter config: " + encounterConfig.displayName,
                this);
        }
    }
}
