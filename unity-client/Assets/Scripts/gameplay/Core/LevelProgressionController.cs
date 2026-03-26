using System.Collections;
using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelProgressionController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelProgressionData progressionData;

        [Header("References")]
        [SerializeField] private LevelEncounterController levelEncounterController;
        [SerializeField] private GameplayManager gameplayManager;

        [Header("Startup")]
        [SerializeField] private bool applyOnAwake = true;

        [Header("Runtime Flow")]
        [SerializeField] private bool autoAdvanceOnWin = false;
        [SerializeField] private bool restartCurrentLevelOnLoss = false;
        [SerializeField] private float advanceDelaySeconds = 0.15f;

        [Header("Runtime Debug")]
        [SerializeField] private int currentLevelIndex = -1;
        [SerializeField] private LevelEncounterConfigData currentEncounterConfig;
        [SerializeField] private bool isTransitioning;

        public int CurrentLevelIndex => currentLevelIndex;
        public LevelEncounterConfigData CurrentEncounterConfig => currentEncounterConfig;

        private Coroutine transitionRoutine;
        private Coroutine startupRoutine;

        private void Reset()
        {
            if (levelEncounterController == null)
            {
                levelEncounterController = FindFirstObjectByType<LevelEncounterController>();
            }

            if (gameplayManager == null)
            {
                gameplayManager = FindFirstObjectByType<GameplayManager>();
            }
        }

        private void OnEnable()
        {
            if (gameplayManager != null)
            {
                gameplayManager.OnRoundFinished += HandleRoundFinished;
            }
        }

        private void OnDisable()
        {
            if (gameplayManager != null)
            {
                gameplayManager.OnRoundFinished -= HandleRoundFinished;
            }

            if (startupRoutine != null)
            {
                StopCoroutine(startupRoutine);
                startupRoutine = null;
            }

            if (transitionRoutine != null)
            {
                StopCoroutine(transitionRoutine);
                transitionRoutine = null;
            }
        }

        private void Start()
        {
            if (!applyOnAwake)
            {
                return;
            }

            if (startupRoutine != null)
            {
                StopCoroutine(startupRoutine);
            }

            startupRoutine = StartCoroutine(ApplyStartupLevelNextFrame());
        }

        private IEnumerator ApplyStartupLevelNextFrame()
        {
            // Let all scene startup logic finish first, especially EnemySwitchingManager.
            yield return null;

            ApplyStartupLevel();
            startupRoutine = null;
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

            if (gameplayManager == null)
            {
                Debug.LogWarning("LevelProgressionController: gameplayManager is null", this);
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

            if (encounterConfig.autoStartRound)
            {
                gameplayManager.StartRound();
            }

            Debug.Log(
                "LevelProgressionController: applied level index: " + currentLevelIndex +
                " config: " + encounterConfig.displayName,
                this);
        }

        public bool HasNextLevel()
        {
            if (progressionData == null || progressionData.levels == null)
            {
                return false;
            }

            return currentLevelIndex >= 0 && currentLevelIndex < progressionData.levels.Length - 1;
        }

        [ContextMenu("Advance To Next Level")]
        public void AdvanceToNextLevel()
        {
            if (progressionData == null || progressionData.levels == null || progressionData.levels.Length == 0)
            {
                Debug.LogWarning("LevelProgressionController: no levels configured", this);
                return;
            }

            int nextLevelIndex = currentLevelIndex + 1;
            if (nextLevelIndex >= progressionData.levels.Length)
            {
                Debug.Log("LevelProgressionController: reached final level, no next level to apply", this);
                return;
            }

            ApplyLevelByIndex(nextLevelIndex);
        }

        [ContextMenu("Restart Current Level")]
        public void RestartCurrentLevel()
        {
            if (currentLevelIndex < 0)
            {
                Debug.LogWarning("LevelProgressionController: no current level to restart", this);
                return;
            }

            ApplyLevelByIndex(currentLevelIndex);
        }

        private void HandleRoundFinished(bool isWin)
        {
            if (transitionRoutine != null)
            {
                StopCoroutine(transitionRoutine);
                transitionRoutine = null;
            }

            if (isWin)
            {
                if (autoAdvanceOnWin)
                {
                    transitionRoutine = StartCoroutine(AdvanceAfterDelay());
                }

                return;
            }

            if (restartCurrentLevelOnLoss)
            {
                transitionRoutine = StartCoroutine(RestartAfterDelay());
            }
        }

        private IEnumerator AdvanceAfterDelay()
        {
            isTransitioning = true;

            yield return null;
            yield return new WaitForSeconds(advanceDelaySeconds);

            AdvanceToNextLevel();

            isTransitioning = false;
            transitionRoutine = null;
        }

        private IEnumerator RestartAfterDelay()
        {
            isTransitioning = true;

            yield return null;
            yield return new WaitForSeconds(advanceDelaySeconds);

            RestartCurrentLevel();

            isTransitioning = false;
            transitionRoutine = null;
        }
    }
}
