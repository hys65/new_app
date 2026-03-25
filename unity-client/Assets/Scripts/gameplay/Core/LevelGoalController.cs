using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelGoalController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameplayManager gameplayManager;

        [Header("Runtime Debug")]
        [SerializeField] private LevelGoalDefinition currentGoal;
        [SerializeField] private int currentProgress;
        [SerializeField] private bool goalCompleted;

        public LevelGoalDefinition CurrentGoal => currentGoal;
        public int CurrentProgress => currentProgress;
        public bool GoalCompleted => goalCompleted;

        private void Reset()
        {
            if (gameplayManager == null)
            {
                gameplayManager = FindFirstObjectByType<GameplayManager>();
            }
        }

        public void ApplyGoal(LevelEncounterConfigData encounterConfig)
        {
            if (encounterConfig == null)
            {
                currentGoal = null;
                currentProgress = 0;
                goalCompleted = false;
                return;
            }

            if (encounterConfig.primaryGoal == null)
            {
                currentGoal = new LevelGoalDefinition
                {
                    goalType = LevelGoalType.BreakdownTarget,
                    targetCount = Mathf.Max(1, encounterConfig.targetBreakdownValue),
                    requiredItemId = string.Empty
                };
            }
            else
            {
                currentGoal = new LevelGoalDefinition
                {
                    goalType = encounterConfig.primaryGoal.goalType,
                    targetCount = Mathf.Max(1, encounterConfig.primaryGoal.targetCount),
                    requiredItemId = encounterConfig.primaryGoal.requiredItemId
                };

                if (currentGoal.goalType == LevelGoalType.BreakdownTarget)
                {
                    currentGoal.targetCount = Mathf.Max(1, encounterConfig.targetBreakdownValue);
                }
            }

            ResetProgress();
        }

        public void ResetProgress()
        {
            currentProgress = 0;
            goalCompleted = false;
        }

        public void NotifyHitResolved(CombatHitInfo hitInfo)
        {
            if (currentGoal == null || goalCompleted)
            {
                return;
            }

            switch (currentGoal.goalType)
            {
                case LevelGoalType.BreakdownTarget:
                    if (gameplayManager != null)
                    {
                        currentProgress = gameplayManager.CurrentBreakdownValue;
                        TryCompleteGoal();
                    }
                    break;

                case LevelGoalType.HeadHitCount:
                    if (hitInfo.gainedScore > 0 && hitInfo.isHeadHit)
                    {
                        currentProgress++;
                        TryCompleteGoal();
                    }
                    break;

                case LevelGoalType.SpecificItemHitCount:
                    if (hitInfo.gainedScore > 0 &&
                        !string.IsNullOrWhiteSpace(currentGoal.requiredItemId) &&
                        string.Equals(hitInfo.itemId, currentGoal.requiredItemId, System.StringComparison.Ordinal))
                    {
                        currentProgress++;
                        TryCompleteGoal();
                    }
                    break;

                case LevelGoalType.UnblockedHitStreak:
                    if (hitInfo.wasBlocked)
                    {
                        currentProgress = 0;
                        gameplayManager?.RefreshState();
                        return;
                    }

                    if (hitInfo.gainedScore > 0)
                    {
                        currentProgress++;
                        TryCompleteGoal();
                    }
                    break;
            }
        }

        public string GetGoalSummaryText()
        {
            if (currentGoal == null)
            {
                return string.Empty;
            }

            switch (currentGoal.goalType)
            {
                case LevelGoalType.HeadHitCount:
                    return $"Goal: Head Hits {currentProgress} / {currentGoal.targetCount}";

                case LevelGoalType.SpecificItemHitCount:
                    return $"Goal: {currentGoal.requiredItemId} Hits {currentProgress} / {currentGoal.targetCount}";

                case LevelGoalType.UnblockedHitStreak:
                    return $"Goal: Clean Hits {currentProgress} / {currentGoal.targetCount}";

                case LevelGoalType.BreakdownTarget:
                default:
                    int breakdownValue = gameplayManager != null ? gameplayManager.CurrentBreakdownValue : currentProgress;
                    return $"Goal: Breakdown {breakdownValue} / {currentGoal.targetCount}";
            }
        }

        private void TryCompleteGoal()
        {
            if (currentGoal == null)
            {
                return;
            }

            if (currentProgress < currentGoal.targetCount)
            {
                return;
            }

            currentProgress = currentGoal.targetCount;
            goalCompleted = true;

            if (currentGoal.goalType != LevelGoalType.BreakdownTarget && gameplayManager != null)
            {
                gameplayManager.ForceFinishRound(true);
            }
        }
    }
}
