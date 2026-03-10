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

        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI currentBreakdownText;
        [SerializeField] private TextMeshProUGUI targetBreakdownText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI selectedItemText;

        [Header("Result")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private Button retryButton;

        private void Awake()
        {
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }

            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
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
            if (currentBreakdownText != null)
            {
                currentBreakdownText.text = "CURRENT TEST 123";
                currentBreakdownText.color = Color.red;
                currentBreakdownText.fontSize = 48;
            }

            if (targetBreakdownText != null)
            {
                targetBreakdownText.text = "TARGET TEST 456";
                targetBreakdownText.color = Color.green;
                targetBreakdownText.fontSize = 48;
            }

            if (timerText != null)
            {
                timerText.text = "TIME TEST 789";
                timerText.color = Color.yellow;
                timerText.fontSize = 48;
            }

            if (selectedItemText != null)
            {
                selectedItemText.text = "ITEM TEST XYZ";
                selectedItemText.color = Color.cyan;
                selectedItemText.fontSize = 48;
            }
        }

        private void Update()
        {
            RefreshHud();
        }

        private void RefreshHud()
        {
            if (gameplayManager == null)
            {
                return;
            }

            SetText(currentBreakdownText, "ui_breakdown_current", gameplayManager.CurrentBreakdownValue.ToString());
            SetText(targetBreakdownText, "ui_breakdown_target", gameplayManager.TargetBreakdownValue.ToString());
            SetText(timerText, "ui_time_left", Mathf.CeilToInt(gameplayManager.RemainingTimeSeconds).ToString());

            var itemLabel = gameplayManager.CurrentItem != null
                ? GetText(gameplayManager.CurrentItem.displayKey)
                : "-";

            SetText(selectedItemText, "ui_selected_item", itemLabel);
        }

        private void OnRoundFinished(bool isWin)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultTitleText != null)
            {
                var key = isWin ? "result_victory" : "result_failed";
                resultTitleText.text = GetText(key);
            }
        }

        private void OnLocaleChanged(string _)
        {
            RefreshHud();
        }

        private void OnRetryClicked()
        {
            gameplayManager?.RetryRound();
        }

        private void SetText(TextMeshProUGUI textComponent, string labelKey, string value)
        {
            if (textComponent == null)
            {
                return;
            }

            var label = GetText(labelKey);

            if (string.IsNullOrWhiteSpace(label) || label == labelKey)
            {
                label = GetFallbackLabel(labelKey);
            }

            textComponent.text = $"{label}: {value}";
        }

        private string GetText(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var text = localizationManager != null ? localizationManager.GetText(key) : key;

            if (string.IsNullOrWhiteSpace(text))
            {
                return GetFallbackLabel(key);
            }

            return text;
        }

        private string GetFallbackLabel(string key)
        {
            switch (key)
            {
                case "ui_breakdown_current":
                    return "Current Breakdown";
                case "ui_breakdown_target":
                    return "Target Breakdown";
                case "ui_time_left":
                    return "Time Left";
                case "ui_selected_item":
                    return "Selected Item";
                case "result_victory":
                    return "Victory";
                case "result_failed":
                    return "Failed";
                default:
                    return key;
            }
        }
    }
}