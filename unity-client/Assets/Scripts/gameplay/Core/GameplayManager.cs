using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PowerPrank3D.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Goal")]
        [SerializeField] private int targetBreakdownValue = 100;
        [SerializeField] private float roundDurationSeconds = 45f;

        [Header("Startup")]
        [SerializeField] private bool autoStartOnStart = true;

        [Header("Items")]
        [SerializeField] private GameplayItemData[] itemList;
        [SerializeField] private int defaultItemIndex;

        [Header("Combo")]
        [SerializeField] private float comboWindowSeconds = 2f;

        [Header("Enemy Reaction")]
        [SerializeField] private EnemyReactionLayerController enemyReactionLayer;

        public event Action OnStateChanged;
        public event Action<bool> OnRoundFinished;

        public int CurrentBreakdownValue { get; private set; }
        public int TargetBreakdownValue => targetBreakdownValue;
        public float RoundDurationSeconds => roundDurationSeconds;
        public float RemainingTimeSeconds { get; private set; }
        public bool IsRoundRunning { get; private set; }

        public GameplayItemData CurrentItem =>
            itemList != null && itemList.Length > 0
                ? itemList[currentItemIndex]
                : null;

        public int ComboCount { get; private set; }

        public float CurrentComboMultiplier => GetComboMultiplier(ComboCount);

        public float ComboTimeRemaining
        {
            get
            {
                if (!IsRoundRunning || ComboCount <= 0)
                {
                    return 0f;
                }

                float elapsed = Time.time - lastHitTime;
                return Mathf.Clamp(comboWindowSeconds - elapsed, 0f, comboWindowSeconds);
            }
        }

        private int currentItemIndex;
        private float lastHitTime = -999f;

        private void Start()
        {
            if (autoStartOnStart)
            {
                StartRound();
            }
        }

        private void Update()
        {
            if (!IsRoundRunning)
            {
                return;
            }

            RemainingTimeSeconds -= Time.deltaTime;

            if (RemainingTimeSeconds <= 0f)
            {
                RemainingTimeSeconds = 0f;
                ResetCombo();
                FinishRound(false);
                return;
            }

            if (ComboCount > 0 && Time.time - lastHitTime > comboWindowSeconds)
            {
                ResetCombo();
            }

            OnStateChanged?.Invoke();
        }

        public void ApplyEncounterSettings(int targetBreakdown, float roundDuration)
        {
            targetBreakdownValue = Mathf.Max(1, targetBreakdown);
            roundDurationSeconds = Mathf.Max(1f, roundDuration);

            if (enemyReactionLayer != null)
            {
                enemyReactionLayer.RefreshStage(CurrentBreakdownValue, TargetBreakdownValue);
            }

            OnStateChanged?.Invoke();
        }

        public void SetAutoStartOnStart(bool shouldAutoStart)
        {
            autoStartOnStart = shouldAutoStart;
        }

        public void SetActiveEnemyReactionLayer(EnemyReactionLayerController reactionLayer)
        {
            enemyReactionLayer = reactionLayer;

            if (enemyReactionLayer != null)
            {
                enemyReactionLayer.RefreshStage(CurrentBreakdownValue, TargetBreakdownValue);
            }

            OnStateChanged?.Invoke();
        }

        public void StartRound()
        {
            CurrentBreakdownValue = 0;
            RemainingTimeSeconds = roundDurationSeconds;
            IsRoundRunning = true;
            ComboCount = 0;
            lastHitTime = -999f;

            currentItemIndex = Mathf.Clamp(
                defaultItemIndex,
                0,
                Mathf.Max(0, itemList != null ? itemList.Length - 1 : 0));

            if (enemyReactionLayer != null)
            {
                enemyReactionLayer.RefreshStage(CurrentBreakdownValue, TargetBreakdownValue);
            }

            OnStateChanged?.Invoke();
        }

        public int AddBreakdown(GameplayItemData itemData)
        {
            return AddBreakdown(itemData, 1);
        }

        public int AddBreakdown(GameplayItemData itemData, int scoreUnits)
        {
            if (!IsRoundRunning || itemData == null)
            {
                return 0;
            }

            if (scoreUnits < 1)
            {
                scoreUnits = 1;
            }

            AdvanceCombo();

            int baseScore = itemData.baseBreakdownScore * scoreUnits;
            float comboMultiplier = GetComboMultiplier(ComboCount);
            int finalScore = Mathf.RoundToInt(baseScore * comboMultiplier);

            CurrentBreakdownValue += finalScore;

            if (CurrentBreakdownValue >= TargetBreakdownValue)
            {
                CurrentBreakdownValue = TargetBreakdownValue;
            }

            if (enemyReactionLayer != null)
            {
                enemyReactionLayer.RefreshStage(CurrentBreakdownValue, TargetBreakdownValue);
            }

            if (CurrentBreakdownValue >= TargetBreakdownValue)
            {
                ResetCombo();
                FinishRound(true);
                return finalScore;
            }

            OnStateChanged?.Invoke();
            return finalScore;
        }

        public void SelectItemByIndex(int index)
        {
            if (!IsRoundRunning || itemList == null || itemList.Length == 0)
            {
                return;
            }

            currentItemIndex = Mathf.Clamp(index, 0, itemList.Length - 1);
            OnStateChanged?.Invoke();
        }

        public void RetryRound()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void AdvanceCombo()
        {
            if (Time.time - lastHitTime <= comboWindowSeconds)
            {
                ComboCount++;
            }
            else
            {
                ComboCount = 1;
            }

            lastHitTime = Time.time;
        }

        private void ResetCombo()
        {
            ComboCount = 0;
            lastHitTime = -999f;
        }

        private float GetComboMultiplier(int comboCount)
        {
            if (comboCount >= 5) return 2.0f;
            if (comboCount >= 3) return 1.5f;
            if (comboCount >= 2) return 1.2f;
            return 1.0f;
        }

        private void FinishRound(bool isWin)
        {
            IsRoundRunning = false;
            OnStateChanged?.Invoke();
            OnRoundFinished?.Invoke(isWin);
        }
    }
}
