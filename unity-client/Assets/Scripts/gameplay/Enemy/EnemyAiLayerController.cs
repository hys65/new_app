using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public enum EnemyAiBrainState
    {
        Observe = 0,
        Predict = 1,
        BrokenRecover = 2
    }

    public class EnemyAiLayerController : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private EnemyAiProfileData aiProfile;

        [Header("References")]
        [SerializeField] private EnemyReactionLayerController reactionLayer;
        [SerializeField] private EnemyDefenseController defenseController;
        [SerializeField] private EnemyDefenseVisualLayerController defenseVisualLayer;
        [SerializeField] private EnemyDefenseStateWindowController defenseStateWindow;

        [Header("Runtime Debug")]
        [SerializeField] private EnemyAiBrainState brainState = EnemyAiBrainState.Observe;
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

            if (brainState == EnemyAiBrainState.BrokenRecover)
            {
                return;
            }

            currentThreat = EvaluateThreat();

            if (sampledHitCount >= aiProfile.requiredHitSamples)
            {
                brainState = EnemyAiBrainState.Predict;
            }
            else
            {
                brainState = EnemyAiBrainState.Observe;
            }

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
                    float clamped = Mathf.Clamp(
                        interval,
                        aiProfile.predictedIntervalClamp.x,
                        aiProfile.predictedIntervalClamp.y);

                    if (sampledHitCount <= 0)
                    {
                        predictedInterval = clamped;
                    }
                    else
                    {
                        predictedInterval = Mathf.Lerp(
                            predictedInterval,
                            clamped,
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
                EnterBrokenRecover();
                currentThreat -= aiProfile.breakPenalty;
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
            if (brainState != EnemyAiBrainState.BrokenRecover)
            {
                return;
            }

            brokenRecoverTimer -= Time.deltaTime;
            if (brokenRecoverTimer > 0f)
            {
                return;
            }

            brokenRecoverTimer = 0f;
            brainState = EnemyAiBrainState.Observe;
            nextDecisionAllowedTime = Time.time + aiProfile.rearmDelayAfterRecover;

            if (aiProfile.debugLog)
            {
                Debug.Log("[EnemyAI] BrokenRecover -> Observe", this);
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

            sampledHitCount = 0;
            predictedNextHitTime = -1f;
            lastObservedItemId = string.Empty;
            lastObservedHeadHit = false;

            if (brainState != EnemyAiBrainState.BrokenRecover)
            {
                brainState = EnemyAiBrainState.Observe;
            }
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

            float headThreat = lastObservedHeadHit ? aiProfile.headHitThreatBonus * 0.5f : 0f;
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
            if (brainState != EnemyAiBrainState.Predict)
            {
                return;
            }

            if (Time.time < nextDecisionAllowedTime)
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

            if (defenseStateWindow.CurrentState != EnemyDefenseWindowState.None)
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

            if (aiProfile.debugLog)
            {
                Debug.Log(
                    $"[EnemyAI] StartDefenseCycle stage={reactionLayer.CurrentStage}, " +
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

        private void EnterBrokenRecover()
        {
            brainState = EnemyAiBrainState.BrokenRecover;
            brokenRecoverTimer = aiProfile.brokenLockDuration;
            predictedNextHitTime = -1f;
            sampledHitCount = 0;

            if (defenseStateWindow != null &&
                defenseStateWindow.CurrentState != EnemyDefenseWindowState.None)
            {
                defenseStateWindow.ForceEndDefenseCycle();
            }

            if (aiProfile.debugLog)
            {
                Debug.Log("[EnemyAI] EnterBrokenRecover", this);
            }
        }
    }
}
