using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public enum EnemyAiBrainState
    {
        Idle = 0,
        Observe = 1,
        PrepareDefense = 2,
        Guard = 3,
        Recover = 4
    }

    public class EnemyAiLayerController : MonoBehaviour
    {
        [Header("Profile")]
        public EnemyAiProfileData aiProfile;

        [Header("References")]
        [SerializeField] private EnemyReactionLayerController reactionLayer;
        [SerializeField] private EnemyDefenseController defenseController;
        [SerializeField] private EnemyDefenseVisualLayerController defenseVisualLayer;
        [SerializeField] private EnemyDefenseStateWindowController defenseStateWindow;

        [Header("Runtime Debug")]
        [SerializeField] private EnemyAiBrainState brainState = EnemyAiBrainState.Idle;
        [SerializeField] private float currentThreat;
        [SerializeField] private float predictedInterval = 0.9f;
        [SerializeField] private float predictedNextHitTime = -1f;
        [SerializeField] private int sampledHitCount;
        [SerializeField] private bool lastObservedHeadHit;
        [SerializeField] private string lastObservedItemId;
        [SerializeField] private float lastObservedHitTime = -999f;
        [SerializeField] private float nextDecisionAllowedTime;
        [SerializeField] private float brokenRecoverTimer;

        public EnemyAiBrainState BrainState => brainState;

        private void Awake()
        {
            if (reactionLayer == null)
            {
                reactionLayer = GetComponent<EnemyReactionLayerController>();
            }

            if (defenseController == null)
            {
                defenseController = GetComponent<EnemyDefenseController>();
            }

            if (defenseVisualLayer == null)
            {
                defenseVisualLayer = GetComponent<EnemyDefenseVisualLayerController>();
            }

            if (defenseStateWindow == null)
            {
                defenseStateWindow = GetComponent<EnemyDefenseStateWindowController>();
            }
        }

        private void Update()
        {
            if (aiProfile == null || defenseStateWindow == null)
            {
                return;
            }

            UpdateBrokenRecover();
            UpdateObservationDecay();

            currentThreat = EvaluateThreat();
            RefreshBrainState();
            TryStartDefenseCycle();
        }

        public void NotifyHitEvaluated(GameplayItemData itemData, bool isHeadHit, DefenseHitResult result)
        {
            if (aiProfile == null)
            {
                return;
            }

            float now = Time.time;

            if (lastObservedHitTime > -100f)
            {
                float interval = now - lastObservedHitTime;

                if (interval >= aiProfile.minAcceptedHitInterval)
                {
                    float clampedInterval = Mathf.Clamp(
                        interval,
                        aiProfile.predictedIntervalClamp.x,
                        aiProfile.predictedIntervalClamp.y);

                    if (sampledHitCount <= 0)
                    {
                        predictedInterval = clampedInterval;
                    }
                    else
                    {
                        predictedInterval = Mathf.Lerp(
                            predictedInterval,
                            clampedInterval,
                            aiProfile.intervalBlend);
                    }

                    sampledHitCount++;
                }
            }
            else
            {
                sampledHitCount = 1;
            }

            lastObservedHitTime = now;
            lastObservedHeadHit = isHeadHit;
            lastObservedItemId = itemData != null ? itemData.itemId : string.Empty;
            predictedNextHitTime = now + predictedInterval;

            if (isHeadHit)
            {
                currentThreat += aiProfile.headHitThreatBonus;
            }

            if (result.wasBlocked)
            {
                currentThreat += aiProfile.blockedConfidenceBonus;
            }

            if (result.weaknessApplied)
            {
                currentThreat -= aiProfile.weaknessPenalty;
            }

            if (result.brokeDefense)
            {
                currentThreat -= aiProfile.breakPenalty;
                EnterRecoverLock();
            }

            currentThreat = Mathf.Clamp01(currentThreat);

            if (aiProfile.debugLog)
            {
                Debug.Log(
                    $"[EnemyAI] HitObserved item={lastObservedItemId}, head={isHeadHit}, " +
                    $"blocked={result.wasBlocked}, broke={result.brokeDefense}, " +
                    $"samples={sampledHitCount}, next={predictedNextHitTime:F2}, threat={currentThreat:F2}",
                    this);
            }
        }

        private void UpdateBrokenRecover()
        {
            if (brokenRecoverTimer <= 0f)
            {
                return;
            }

            brokenRecoverTimer -= Time.deltaTime;

            if (brokenRecoverTimer > 0f)
            {
                return;
            }

            brokenRecoverTimer = 0f;
            nextDecisionAllowedTime = Time.time + aiProfile.rearmDelayAfterRecover;

            if (aiProfile.debugLog)
            {
                Debug.Log("[EnemyAI] Recover lock ended", this);
            }
        }

        private void UpdateObservationDecay()
        {
            if (lastObservedHitTime < -100f)
            {
                return;
            }

            float timeSinceLastHit = Time.time - lastObservedHitTime;

            if (timeSinceLastHit < aiProfile.memoryDuration)
            {
                return;
            }

            ClearObservationMemory();
        }

        private void RefreshBrainState()
        {
            EnemyDefenseWindowState windowState = defenseStateWindow.CurrentState;

            if (windowState == EnemyDefenseWindowState.Telegraph)
            {
                brainState = EnemyAiBrainState.PrepareDefense;
                return;
            }

            if (windowState == EnemyDefenseWindowState.Active)
            {
                brainState = EnemyAiBrainState.Guard;
                return;
            }

            if (windowState == EnemyDefenseWindowState.Recover || IsRecoverLocked())
            {
                brainState = EnemyAiBrainState.Recover;
                return;
            }

            if (!HasObservationMemory())
            {
                brainState = EnemyAiBrainState.Idle;
                return;
            }

            brainState = EnemyAiBrainState.Observe;
        }

        private float EvaluateThreat()
        {
            float baseThreat = GetStageThreat();
            float cadenceThreat = 0f;

            if (sampledHitCount >= aiProfile.requiredHitSamples && predictedInterval > 0.001f)
            {
                float fastFactor = Mathf.InverseLerp(
                    aiProfile.predictedIntervalClamp.y,
                    aiProfile.predictedIntervalClamp.x,
                    predictedInterval);

                cadenceThreat = fastFactor * 0.35f;
            }

            float headThreat = lastObservedHeadHit
                ? aiProfile.headHitThreatBonus * 0.5f
                : 0f;

            return Mathf.Clamp01(baseThreat + cadenceThreat + headThreat);
        }

        private float GetStageThreat()
        {
            if (reactionLayer == null)
            {
                return aiProfile.annoyedThreat;
            }

            switch (reactionLayer.CurrentStage)
            {
                case EnemyReactionStage.Calm:
                    return aiProfile.calmThreat;

                case EnemyReactionStage.Annoyed:
                    return aiProfile.annoyedThreat;

                case EnemyReactionStage.Agitated:
                    return aiProfile.agitatedThreat;

                case EnemyReactionStage.Furious:
                    return aiProfile.furiousThreat;

                case EnemyReactionStage.Meltdown:
                    return aiProfile.meltdownThreat;

                default:
                    return aiProfile.annoyedThreat;
            }
        }

        private void TryStartDefenseCycle()
        {
            if (IsRecoverLocked())
            {
                return;
            }

            if (Time.time < nextDecisionAllowedTime)
            {
                return;
            }

            if (defenseStateWindow.CurrentState != EnemyDefenseWindowState.None)
            {
                return;
            }

            if (sampledHitCount < aiProfile.requiredHitSamples)
            {
                return;
            }

            if (currentThreat < aiProfile.defenseTriggerThreshold)
            {
                return;
            }

            if (predictedNextHitTime < 0f)
            {
                return;
            }

            float leadTime = GetLeadTime();
            float triggerTime = predictedNextHitTime - leadTime;

            if (Time.time < triggerTime)
            {
                return;
            }

            defenseStateWindow.ForceStartDefenseCycle();
            nextDecisionAllowedTime = Time.time + aiProfile.decisionCooldown;
            brainState = EnemyAiBrainState.PrepareDefense;

            if (aiProfile.debugLog)
            {
                Debug.Log(
                    $"[EnemyAI] StartDefenseCycle stage={GetReactionStageName()}, " +
                    $"threat={currentThreat:F2}, predictedInterval={predictedInterval:F2}, lead={leadTime:F2}",
                    this);
            }
        }

        private float GetLeadTime()
        {
            if (reactionLayer == null)
            {
                return aiProfile.baseLeadTime;
            }

            switch (reactionLayer.CurrentStage)
            {
                case EnemyReactionStage.Agitated:
                    return aiProfile.baseLeadTime + aiProfile.agitatedLeadBonus;

                case EnemyReactionStage.Furious:
                    return aiProfile.baseLeadTime + aiProfile.furiousLeadBonus;

                case EnemyReactionStage.Meltdown:
                    return aiProfile.baseLeadTime + aiProfile.meltdownLeadBonus;

                default:
                    return aiProfile.baseLeadTime;
            }
        }

        private void EnterRecoverLock()
        {
            brokenRecoverTimer = aiProfile.brokenLockDuration;
            predictedNextHitTime = -1f;
            sampledHitCount = 0;
            lastObservedHeadHit = false;
            lastObservedItemId = string.Empty;
            lastObservedHitTime = -999f;

            if (defenseStateWindow != null &&
                defenseStateWindow.CurrentState != EnemyDefenseWindowState.None)
            {
                defenseStateWindow.ForceEndDefenseCycle();
            }

            brainState = EnemyAiBrainState.Recover;

            if (aiProfile.debugLog)
            {
                Debug.Log("[EnemyAI] EnterRecoverLock", this);
            }
        }

        private bool HasObservationMemory()
        {
            return sampledHitCount > 0 && predictedNextHitTime >= 0f;
        }

        private bool IsRecoverLocked()
        {
            return brokenRecoverTimer > 0f;
        }

        private void ClearObservationMemory()
        {
            sampledHitCount = 0;
            predictedNextHitTime = -1f;
            lastObservedHeadHit = false;
            lastObservedItemId = string.Empty;
            lastObservedHitTime = -999f;
        }

        private string GetReactionStageName()
        {
            if (reactionLayer == null)
            {
                return "Unknown";
            }

            return reactionLayer.CurrentStage.ToString();
        }
    }
}
