using PowerPrank3D.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PowerPrank3D.Gameplay
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private LocalizationManager localizationManager;
        [SerializeField] private LevelProgressionController levelProgressionController;

        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI currentBreakdownText;
        [SerializeField] private TextMeshProUGUI targetBreakdownText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI selectedItemText;

        [Header("Combo")]
        [SerializeField] private GameObject comboRoot;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private Image comboTimerFill;

        [Header("Result")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI resultSubtitleText;
        [SerializeField] private TextMeshProUGUI levelInfoText;
        [SerializeField] private TextMeshProUGUI goalSummaryText;
        [SerializeField] private TextMeshProUGUI finalLevelNoticeText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private TextMeshProUGUI nextLevelButtonText;
        [SerializeField] private TextMeshProUGUI retryButtonText;

        private bool lastRoundFinished;
        private bool lastRoundWasWin;

        private void Awake()
        {
            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
                retryButton.onClick.AddListener(OnRetryClicked);
            }

            if (nextLevelButton != null)
            {
                nextLevelButton.onClick.RemoveAllListeners();
                nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            }

            HideResultPanel();
            SetComboVisible(false);
        }

        private void OnEnable()
        {
            if (gameplayManager != null)
            {
                gameplayManager.OnStateChanged += RefreshHud;
                gameplayManager.OnRoundFinished += OnRoundFinished;
            }

            if (localizationManager != null)
            {
                localizationManager.OnLocaleChanged += OnLocaleChanged;
            }
        }

        private void OnDisable()
        {
            if (gameplayManager != null)
            {
                gameplayManager.OnStateChanged -= RefreshHud;
                gameplayManager.OnRoundFinished -= OnRoundFinished;
            }

            if (localizationManager != null)
            {
                localizationManager.OnLocaleChanged -= OnLocaleChanged;
            }
        }

        private void Start()
        {
            RefreshHud();
        }

        private void RefreshHud()
        {
            if (gameplayManager == null)
            {
                return;
            }

            if (gameplayManager.IsRoundRunning)
            {
                lastRoundFinished = false;
                HideResultPanel();
            }

            SetText(currentBreakdownText, "ui_breakdown_current", gameplayManager.CurrentBreakdownValue.ToString());
            SetText(targetBreakdownText, "ui_breakdown_target", gameplayManager.TargetBreakdownValue.ToString());
            SetText(timerText, "ui_time_left", Mathf.CeilToInt(gameplayManager.RemainingTimeSeconds).ToString());

            string itemLabel = gameplayManager.CurrentItem != null
                ? GetText(gameplayManager.CurrentItem.displayKey)
                : "-";

            SetText(selectedItemText, "ui_selected_item", itemLabel);

            RefreshComboHud();

            if (lastRoundFinished && resultPanel != null && resultPanel.activeSelf)
            {
                RefreshResultPanel(lastRoundWasWin);
            }
        }

        private void RefreshComboHud()
        {
            if (gameplayManager == null)
            {
                SetComboVisible(false);
                return;
            }

            int comboCount = gameplayManager.ComboCount;
            if (comboCount < 2)
            {
                SetComboVisible(false);
                return;
            }

            SetComboVisible(true);

            if (comboText != null)
            {
                comboText.text = $"COMBO x{comboCount}";
                comboText.color = GetComboColor(comboCount);
            }

            if (comboTimerFill != null)
            {
                float maxWindow = 2f;
                float remain = gameplayManager.ComboTimeRemaining;
                comboTimerFill.fillAmount = Mathf.Clamp01(remain / maxWindow);
                comboTimerFill.color = GetComboColor(comboCount);
            }
        }

        private Color GetComboColor(int comboCount)
        {
            if (comboCount >= 5)
            {
                return new Color(1f, 0.55f, 0f, 1f);
            }

            if (comboCount >= 3)
            {
                return Color.yellow;
            }

            return Color.white;
        }

        private void SetComboVisible(bool visible)
        {
            if (comboRoot != null)
            {
                comboRoot.SetActive(visible);
            }
            else
            {
                if (comboText != null)
                {
                    comboText.gameObject.SetActive(visible);
                }

                if (comboTimerFill != null && comboTimerFill.transform.parent != null)
                {
                    comboTimerFill.transform.parent.gameObject.SetActive(visible);
                }
            }
        }

        private void OnRoundFinished(bool isWin)
        {
            lastRoundFinished = true;
            lastRoundWasWin = isWin;

            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            RefreshResultPanel(isWin);
            SetComboVisible(false);
        }

        private void RefreshResultPanel(bool isWin)
        {
            bool hasNextLevel = isWin &&
                                levelProgressionController != null &&
                                levelProgressionController.HasNextLevel();

            if (resultTitleText != null)
            {
                resultTitleText.text = GetText(isWin ? "result_victory" : "result_failed");
            }

            if (resultSubtitleText != null)
            {
                string subtitleKey;

                if (isWin)
                {
                    subtitleKey = hasNextLevel
                        ? "result_ready_for_next"
                        : "result_all_levels_complete";
                }
                else
                {
                    subtitleKey = "result_try_again";
                }

                resultSubtitleText.text = GetText(subtitleKey);
            }

            if (levelInfoText != null)
            {
                levelInfoText.text = BuildLevelInfoText();
            }

            if (goalSummaryText != null)
            {
                goalSummaryText.text = BuildGoalSummaryText();
            }

            if (finalLevelNoticeText != null)
            {
                bool showFinalNotice = isWin && !hasNextLevel;
                finalLevelNoticeText.gameObject.SetActive(showFinalNotice);

                if (showFinalNotice)
                {
                    finalLevelNoticeText.text = GetText("ui_final_level_cleared");
                }
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true);
            }

            if (retryButtonText != null)
            {
                retryButtonText.text = GetText("ui_retry");
            }

            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(hasNextLevel);
            }

            if (nextLevelButtonText != null)
            {
                nextLevelButtonText.text = GetText("ui_next_level");
            }
        }

        private string BuildLevelInfoText()
        {
            if (levelProgressionController == null)
            {
                return string.Empty;
            }

            int currentLevelNumber = levelProgressionController.CurrentLevelIndex + 1;
            return $"{GetText("ui_level")} {currentLevelNumber}";
        }

        private string BuildGoalSummaryText()
        {
            if (gameplayManager == null)
            {
                return string.Empty;
            }

            return $"{GetText("ui_goal_progress")}: {gameplayManager.CurrentBreakdownValue} / {gameplayManager.TargetBreakdownValue}";
        }

        private void OnLocaleChanged(string _)
        {
            RefreshHud();
        }

        private void OnRetryClicked()
        {
            if (levelProgressionController != null)
            {
                levelProgressionController.RestartCurrentLevel();
                return;
            }

            gameplayManager?.RetryRound();
        }

        private void OnNextLevelClicked()
        {
            if (levelProgressionController == null)
            {
                return;
            }

            levelProgressionController.AdvanceToNextLevel();
        }

        private void HideResultPanel()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
        }

        private void SetText(TextMeshProUGUI textComponent, string labelKey, string value)
        {
            if (textComponent == null)
            {
                return;
            }

            textComponent.text = $"{GetText(labelKey)}: {value}";
        }

        private string GetText(string key)
        {
            return localizationManager != null ? localizationManager.GetText(key) : key;
        }
    }
}
