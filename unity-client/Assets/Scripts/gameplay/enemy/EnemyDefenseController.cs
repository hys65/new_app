using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyDefenseController : MonoBehaviour
    {
        [Header("Defense Pattern")]
        [SerializeField] private EnemyDefensePatternData defensePattern;
        [SerializeField] private bool useDefensePattern = true;

        [Header("Runtime Debug")]
        [SerializeField] private bool defenseActive;
        [SerializeField] private float defenseTimer;
        [SerializeField] private float defenseCooldownTimer;
        [SerializeField] private int recentHitCount;
        [SerializeField] private float repeatedHitWindowTimer;
        [SerializeField] private EnemyDefenseStateWindowController defenseStateWindow;

        [SerializeField] private EnemyAiLayerController enemyAiLayer;

        public EnemyDefensePatternData DefensePattern => defensePattern;
        public bool IsDefenseActive => useDefensePattern && defensePattern != null && defenseActive;

        private void Awake()
        {
            if (defenseStateWindow == null)
            {
                defenseStateWindow = GetComponent<EnemyDefenseStateWindowController>();
            }

            if (enemyAiLayer == null)
            {
                enemyAiLayer = GetComponent<EnemyAiLayerController>();
            }
        }

        private DefenseHitResult FinalizeResult(GameplayItemData itemData, bool isHeadHit, DefenseHitResult result)
        {
            enemyAiLayer?.NotifyHitEvaluated(itemData, isHeadHit, result);
            return result;
        }

        private bool HasPattern()
        {
            return useDefensePattern && defensePattern != null;
        }

        private void Update()
        {
            if (defenseTimer > 0f)
            {
                defenseTimer -= Time.deltaTime;
                if (defenseTimer <= 0f)
                {
                    defenseTimer = 0f;
                    defenseActive = false;
                }
            }

            if (defenseCooldownTimer > 0f)
            {
                defenseCooldownTimer -= Time.deltaTime;
                if (defenseCooldownTimer < 0f)
                {
                    defenseCooldownTimer = 0f;
                }
            }

            if (repeatedHitWindowTimer > 0f)
            {
                repeatedHitWindowTimer -= Time.deltaTime;
                if (repeatedHitWindowTimer <= 0f)
                {
                    repeatedHitWindowTimer = 0f;
                    recentHitCount = 0;
                }
            }
        }

        public DefenseHitResult EvaluateHit(GameplayItemData itemData, bool isHeadHit)
        {
            DefenseHitResult result = DefenseHitResult.Default();

            if (defenseStateWindow != null && !defenseStateWindow.CanUseDefenseLogic())
            {
                return FinalizeResult(itemData, isHeadHit, result);
            }

            if (!HasPattern())
            {
                return FinalizeResult(itemData, isHeadHit, result);
            }

            CountRepeatedHit();

            if (defenseActive)
            {

                if (CanBreakDefense(itemData))
                {
                    defenseActive = false;
                    defenseTimer = 0f;
                    defenseCooldownTimer = defensePattern.blockCooldown;

                    result.brokeDefense = true;
                    result.breakdownMultiplier = 1.15f;
                    result.reactionMultiplier = 1.25f;
                    result.popupText = "BREAK";
                    return FinalizeResult(itemData, isHeadHit, result);
                }

                if (ShouldBlockHit(isHeadHit))
                {
                    result.wasBlocked = true;
                    result.breakdownMultiplier = defensePattern.blockedBreakdownMultiplier;
                    result.reactionMultiplier = defensePattern.blockedReactionMultiplier;
                    result.popupText = "BLOCK";
                    return FinalizeResult(itemData, isHeadHit, result);
                }
            }

            bool activatedNow = TryActivateDefense(itemData, isHeadHit);
            if (activatedNow)
            {
                result.activatedDefense = true;
                result.popupText = "GUARD";
            }

            if (defensePattern.weakToHeadHits &&
                isHeadHit &&
                (defenseStateWindow == null || defenseStateWindow.CanExposeWeakness()))
            {
                result.weaknessApplied = true;
                result.breakdownMultiplier *= defensePattern.headWeaknessMultiplier;
                result.reactionMultiplier *= 1.15f;
                result.wasBlocked = false;

                if (string.IsNullOrEmpty(result.popupText) || result.popupText == "GUARD")
                {
                    result.popupText = "WEAK";
                }
                Debug.Log("[DefenseEval] WEAK triggered");
            }

            if (defenseActive && defensePattern.patternType == EnemyDefensePatternType.FaceGuard)
            {
                if (isHeadHit && defensePattern.reducePaintOnFaceGuard && IsPaintBall(itemData))
                {
                    result.wasBlocked = true;
                    result.breakdownMultiplier *= defensePattern.blockedBreakdownMultiplier;
                    result.reactionMultiplier *= defensePattern.blockedReactionMultiplier;
                    result.popupText = "FACE GUARD";
                    return FinalizeResult(itemData, isHeadHit, result);
                }
            }

            return FinalizeResult(itemData, isHeadHit, result);
        }

        private void CountRepeatedHit()
        {
            if (!HasPattern())
            {
                return;
            }

            recentHitCount++;
            repeatedHitWindowTimer = defensePattern.repeatedHitWindow;
        }

        private bool TryActivateDefense(GameplayItemData itemData, bool isHeadHit)
        {
            if (!HasPattern())
            {
                return false;
            }

            if (defenseActive || defenseCooldownTimer > 0f)
            {
                return false;
            }

            if (defensePattern.autoActivateOnHeadHit && isHeadHit)
            {
                ActivateDefense();
                return true;
            }

            if (defensePattern.autoActivateOnBodyHit && !isHeadHit)
            {
                ActivateDefense();
                return true;
            }

            if (defensePattern.triggeredByRepeatedHits &&
                recentHitCount >= defensePattern.repeatedHitCountThreshold)
            {
                ActivateDefense();
                recentHitCount = 0;
                repeatedHitWindowTimer = 0f;
                return true;
            }

            if (Random.value < defensePattern.randomBlockChance)
            {
                ActivateDefense();
                return true;
            }

            return false;
        }

        private void ActivateDefense()
        {
            defenseActive = true;
            defenseTimer = Mathf.Max(0.05f, defensePattern.blockDuration);
        }

        private bool ShouldBlockHit(bool isHeadHit)
        {
            if (!HasPattern())
            {
                return false;
            }

            if (isHeadHit)
            {
                return defensePattern.canBlockHead;
            }

            return defensePattern.canBlockBody;
        }

        private bool CanBreakDefense(GameplayItemData itemData)
        {
            if (!HasPattern() || itemData == null)
            {
                return false;
            }

            string id = itemData.itemId == null ? string.Empty : itemData.itemId.ToLowerInvariant();

            if (defensePattern.breakByHammer && id.Contains("hammer"))
            {
                return true;
            }

            if (defensePattern.breakByFoam && (id.Contains("foam") || id.Contains("sprayer")))
            {
                return true;
            }

            if (defensePattern.breakByPaint && (id.Contains("paint") || id.Contains("paintball")))
            {
                return true;
            }

            if (defensePattern.breakByEgg && id.Contains("egg"))
            {
                return true;
            }

            if (defensePattern.breakByTomato && id.Contains("tomato"))
            {
                return true;
            }

            return false;
        }

        private bool IsPaintBall(GameplayItemData itemData)
        {
            if (itemData == null || string.IsNullOrEmpty(itemData.itemId))
            {
                return false;
            }

            string id = itemData.itemId.ToLowerInvariant();
            return id.Contains("paint");
        }
    }
}
