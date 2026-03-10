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

        [Header("Items")]
        [SerializeField] private GameplayItemData[] itemList;
        [SerializeField] private int defaultItemIndex;

        public event Action OnStateChanged;
        public event Action<bool> OnRoundFinished;

        public int CurrentBreakdownValue { get; private set; }
        public int TargetBreakdownValue => targetBreakdownValue;
        public float RemainingTimeSeconds { get; private set; }
        public bool IsRoundRunning { get; private set; }
        public GameplayItemData CurrentItem => itemList != null && itemList.Length > 0 ? itemList[currentItemIndex] : null;
        public int ItemCount => itemList != null ? itemList.Length : 0;

        private int currentItemIndex;

        private void Start()
        {
            StartRound();
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
                FinishRound(false);
            }

            OnStateChanged?.Invoke();
        }

        public void StartRound()
        {
            CurrentBreakdownValue = 0;
            RemainingTimeSeconds = roundDurationSeconds;
            IsRoundRunning = true;
            currentItemIndex = Mathf.Clamp(defaultItemIndex, 0, Mathf.Max(0, ItemCount - 1));
            OnStateChanged?.Invoke();
        }

        public void AddBreakdown(GameplayItemData itemData)
        {
            if (!IsRoundRunning || itemData == null)
            {
                return;
            }

            CurrentBreakdownValue += Mathf.Max(0, itemData.baseBreakdownScore);
            if (CurrentBreakdownValue >= TargetBreakdownValue)
            {
                CurrentBreakdownValue = TargetBreakdownValue;
                FinishRound(true);
                return;
            }

            OnStateChanged?.Invoke();
        }

        public void SelectItemByIndex(int index)
        {
            if (!IsRoundRunning || ItemCount == 0)
            {
                return;
            }

            currentItemIndex = Mathf.Clamp(index, 0, ItemCount - 1);
            OnStateChanged?.Invoke();
        }

        public void RetryRound()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void FinishRound(bool isWin)
        {
            if (!IsRoundRunning)
            {
                return;
            }

            IsRoundRunning = false;
            OnStateChanged?.Invoke();
            OnRoundFinished?.Invoke(isWin);
        }
    }
}
