using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyDefenseController : MonoBehaviour
    {
        [Header("Defense Pattern")]
        public EnemyDefensePatternData defensePattern;
        [SerializeField] private bool useDefensePattern = true;

        [Header("Runtime Debug")]
        [SerializeField] private bool defenseActive;
        [SerializeField] private float defenseTimer;
        [SerializeField] private float defenseCooldownTimer;
        [SerializeField] private int recentHitCount;
        [SerializeField] private float repeatedHitWindowTimer;
        [SerializeField] private float runtimeElapsed;
        [SerializeField] private float nextTimedActivationAt;
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

            ResetTimedActivationSchedule();
        }

        private void OnEnable()
        {
            ResetTimedActivationSchedule();
        }

        private void ResetTimedActivationSchedule()
        {
            runtimeElapsed = 0f;
            nextTimedActivationAt = defensePattern != null
                ? Mathf.Max(0f, defensePattern.firstActivationDelay)
                : 0f;
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
            if (!HasPattern())
            {
                return;
            }

            runtimeElapsed += Time.deltaTime;

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

            TryActivateTimedDefense();
        }

        public DefenseHitResult EvaluateHit(GameplayItemData itemData, bool isHeadHit)
        {
            DefenseHitResult result = DefenseHitResult.Default();

            if (!HasPattern())
            {
                return FinalizeResult(itemData, isHeadHit, result);
            }

            // --------------------------------------------------
            // Level 04 hard-rule path:
            // BriefcaseBlock must remain deterministic.
            // --------------------------------------------------
            if (defensePattern.patternType == EnemyDefensePatternType.BriefcaseBlock && defenseActive)
            {
                if (CanBreakDefense(itemData))
                {
                    defenseActive = false;
                    defenseTimer = 0f;
                    defenseCooldownTimer = Mathf.Max(0f, defensePattern.blockCooldown);

                    result.brokeDefense = true;
                    result.breakdownMultiplier = 1.15f;
                    result.reactionMultiplier = 1.25f;
                    result.popupText = "BREAK";

                    Debug.Log("[DefenseEval] BRIEFCASE BREAK by item=" + SafeItemId(itemData));
                    return FinalizeResult(itemData, isHeadHit, result);
                }

                result.wasBlocked = true;
                result.breakdownMultiplier = 0f;
                result.reactionMultiplier = Mathf.Max(0f, defensePattern.blockedReactionMultiplier);
                result.popupText = "BLOCK";

                Debug.Log("[DefenseEval] BRIEFCASE BLOCK item=" + SafeItemId(itemData) + " head=" + isHeadHit);
                return FinalizeResult(itemData, isHeadHit, result);
            }

            CountRepeatedHit();

            bool weakWindowHeadBypass = false;

            // --------------------------------------------------
            // Core rule for Level 06:
            // If defense is active, blocking should be enforced by default.
            // State window does NOT decide whether defense exists.
            // State window only decides whether the weak head bypass opens.
            // --------------------------------------------------
            if (defenseActive)
            {
                if (CanBreakDefense(itemData))
                {
                    defenseActive = false;
                    defenseTimer = 0f;
                    defenseCooldownTimer = Mathf.Max(0f, defensePattern.blockCooldown);

                    result.brokeDefense = true;
                    result.breakdownMultiplier = 1.15f;
                    result.reactionMultiplier = 1.25f;
                    result.popupText = "BREAK";

                    Debug.Log("[DefenseEval] BREAK item=" + SafeItemId(itemData));
                    return FinalizeResult(itemData, isHeadHit, result);
                }

                weakWindowHeadBypass = CanUseWeakWindowHeadBypass(isHeadHit);

                if (ShouldBlockHit(isHeadHit) && !weakWindowHeadBypass)
                {
                    result.wasBlocked = true;
                    result.breakdownMultiplier = defensePattern.blockedBreakdownMultiplier;
                    result.reactionMultiplier = defensePattern.blockedReactionMultiplier;
                    result.popupText = "BLOCK";

                    Debug.Log("[DefenseEval] BLOCK item=" + SafeItemId(itemData) + " head=" + isHeadHit);
                    return FinalizeResult(itemData, isHeadHit, result);
                }
            }

            bool activatedNow = TryActivateDefense(itemData, isHeadHit);
            if (activatedNow)
            {
                result.activatedDefense = true;
                result.popupText = "GUARD";
                Debug.Log("[DefenseEval] GUARD activated");
            }

            bool canApplyWeakness =
                defenseActive &&
                defensePattern.weakToHeadHits &&
                isHeadHit &&
                weakWindowHeadBypass;

            if (canApplyWeakness)
            {
                result.weaknessApplied = true;
                result.breakdownMultiplier *= defensePattern.headWeaknessMultiplier;
                result.reactionMultiplier *= 1.15f;
                result.wasBlocked = false;
                result.popupText = "WEAK";

                Debug.Log("[DefenseEval] WEAK triggered");
            }

            // Level 05 rule:
            // FaceGuard active + paint should remain invalid until defense is broken.
            if (defenseActive && defensePattern.patternType == EnemyDefensePatternType.FaceGuard)
            {
                if (defensePattern.reducePaintOnFaceGuard && IsPaintBall(itemData))
                {
                    result.wasBlocked = true;
                    result.breakdownMultiplier = 0f;
                    result.reactionMultiplier = Mathf.Max(0f, defensePattern.blockedReactionMultiplier);
                    result.popupText = "FACE GUARD";

                    Debug.Log("[DefenseEval] FACE GUARD block item=" + SafeItemId(itemData) + " head=" + isHeadHit);
                    return FinalizeResult(itemData, isHeadHit, result);
                }
            }

            return FinalizeResult(itemData, isHeadHit, result);
        }

        private void TryActivateTimedDefense()
        {
            if (!HasPattern())
            {
                return;
            }

            if (!defensePattern.useTimedActivation)
            {
                return;
            }

            if (defenseActive || defenseCooldownTimer > 0f)
            {
                return;
            }

            if (runtimeElapsed < nextTimedActivationAt)
            {
                return;
            }

            ActivateDefense();
            ScheduleNextTimedActivation();

            Debug.Log("[DefenseEval] Timed defense activated");
        }

        private void ScheduleNextTimedActivation()
        {
            if (!HasPattern())
            {
                return;
            }

            float interval = Mathf.Max(0.1f, defensePattern.timedActivationInterval);
            nextTimedActivationAt = runtimeElapsed + interval;
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

            return isHeadHit ? defensePattern.canBlockHead : defensePattern.canBlockBody;
        }

        private bool CanUseWeakWindowHeadBypass(bool isHeadHit)
        {
            if (!defenseActive || !isHeadHit || !HasPattern())
            {
                return false;
            }

            if (!defensePattern.weakToHeadHits)
            {
                return false;
            }

            if (!ShouldBlockHit(true))
            {
                return false;
            }

            if (defenseStateWindow == null)
            {
                return false;
            }

            return defenseStateWindow.CanExposeWeakness();
        }

        private bool CanBreakDefense(GameplayItemData itemData)
        {
            if (!HasPattern() || itemData == null)
            {
                return false;
            }

            string id = SafeItemId(itemData);

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
            string id = SafeItemId(itemData);
            return id.Contains("paint");
        }

        private string SafeItemId(GameplayItemData itemData)
        {
            return itemData == null || string.IsNullOrEmpty(itemData.itemId)
                ? string.Empty
                : itemData.itemId.ToLowerInvariant();
        }
    }
}
