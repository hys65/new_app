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
        [SerializeField] private Button retryButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private TextMeshProUGUI nextLevelButtonText;
        [SerializeField] private TextMeshProUGUI retryButtonText;

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
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultTitleText != null)
            {
                string key = isWin ? "result_victory" : "result_failed";
                resultTitleText.text = GetText(key);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true);
            }

            if (retryButtonText != null)
            {
                retryButtonText.text = GetText("ui_retry");
            }

            if (isWin)
            {
                bool hasNextLevel = levelProgressionController != null &&
                                    levelProgressionController.HasNextLevel();

                if (nextLevelButton != null)
                {
                    nextLevelButton.gameObject.SetActive(hasNextLevel);
                }

                if (nextLevelButtonText != null)
                {
                    nextLevelButtonText.text = GetText("ui_next_level");
                }
            }
            else
            {
                if (nextLevelButton != null)
                {
                    nextLevelButton.gameObject.SetActive(false);
                }
            }

            SetComboVisible(false);
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
