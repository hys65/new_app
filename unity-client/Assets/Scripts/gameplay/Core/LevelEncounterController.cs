using System.Collections;
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
        [SerializeField] private LevelGoalController levelGoalController;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = false;

        [Header("Runtime Debug")]
        [SerializeField] private bool encounterApplied;

        public LevelEncounterConfigData EncounterConfig => encounterConfig;
        public bool EncounterApplied => encounterApplied;

        private Coroutine deferredEncounterRoutine;

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

            if (levelGoalController == null)
            {
                levelGoalController = FindFirstObjectByType<LevelGoalController>();
            }
        }

        private void OnDisable()
        {
            if (deferredEncounterRoutine != null)
            {
                StopCoroutine(deferredEncounterRoutine);
                deferredEncounterRoutine = null;
            }
        }

        private void Start()
        {
            if (applyOnAwake)
            {
                ApplyEncounter();
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

            bool useBreakdownWinCondition =
                encounterConfig.primaryGoal == null ||
                encounterConfig.primaryGoal.goalType == LevelGoalType.BreakdownTarget;

            gameplayManager.ConfigureBreakdownWinCondition(useBreakdownWinCondition);

            if (levelEnemySelectionController != null && encounterConfig.enemySelection != null)
            {
                levelEnemySelectionController.SetSelectionData(encounterConfig.enemySelection);
                levelEnemySelectionController.ApplySelection(encounterConfig.enemySelection);
            }

            if (levelGoalController != null)
            {
                levelGoalController.ApplyGoal(encounterConfig);
            }

            if (deferredEncounterRoutine != null)
            {
                StopCoroutine(deferredEncounterRoutine);
            }

            deferredEncounterRoutine = StartCoroutine(ApplyEncounterNextFrame());

            encounterApplied = true;

            Debug.Log(
                "LevelEncounterController: applied encounter config: " + encounterConfig.displayName,
                this);
        }

        private IEnumerator ApplyEncounterNextFrame()
        {
            yield return null;

            if (encounterConfig == null)
            {
                deferredEncounterRoutine = null;
                yield break;
            }

            if (levelEnemySelectionController != null && encounterConfig.enemySelection != null)
            {
                levelEnemySelectionController.SetSelectionData(encounterConfig.enemySelection);
                levelEnemySelectionController.ApplySelection(encounterConfig.enemySelection);
            }

            deferredEncounterRoutine = null;
        }
    }
}
