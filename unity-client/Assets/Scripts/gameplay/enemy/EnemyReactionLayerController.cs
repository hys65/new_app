using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyReactionLayerController : MonoBehaviour
    {
        private enum StageEntryActionType
        {
            None = 0,
            AnnoyedSnap = 1,
            AgitatedLean = 2,
            FuriousGlare = 3,
            MeltdownBreak = 4
        }

        [Header("Archetype")]
        [SerializeField] private EnemyArchetypeData enemyArchetype;
        [SerializeField] private bool useArchetype = true;

        [Header("References")]
        [SerializeField] private Transform bodyVisual;
        [SerializeField] private Transform headVisual;

        [Header("Stage Thresholds")]
        [SerializeField] [Range(0f, 1f)] private float annoyedThreshold = 0.30f;
        [SerializeField] [Range(0f, 1f)] private float agitatedThreshold = 0.60f;
        [SerializeField] [Range(0f, 1f)] private float furiousThreshold = 0.90f;

        [Header("Idle Motion")]
        [SerializeField] private float calmBodyYaw = 0.8f;
        [SerializeField] private float annoyedBodyYaw = 2.0f;
        [SerializeField] private float agitatedBodyYaw = 4.0f;
        [SerializeField] private float furiousBodyYaw = 7.0f;
        [SerializeField] private float meltdownBodyYaw = 10.0f;

        [SerializeField] private float calmHeadYaw = 1.5f;
        [SerializeField] private float annoyedHeadYaw = 4.0f;
        [SerializeField] private float agitatedHeadYaw = 7.0f;
        [SerializeField] private float furiousHeadYaw = 11.0f;
        [SerializeField] private float meltdownHeadYaw = 14.0f;

        [Header("Hit Reaction")]
        [SerializeField] private float hitBodyKickAngle = 14f;
        [SerializeField] private float hitBodySideAngle = 8f;
        [SerializeField] private float headHitKickAngle = 32f;
        [SerializeField] private float headHitSideAngle = 18f;
        [SerializeField] private float bodyHitRecoverSpeed = 14f;
        [SerializeField] private float headHitRecoverSpeed = 18f;

        [Header("Stage Layer")]
        [SerializeField] private float stageBlendSpeed = 6f;
        [SerializeField] private float angerRecoverSpeed = 5f;
        [SerializeField] private float disgustRecoverSpeed = 4f;
        [SerializeField] private float panicRecoverSpeed = 4f;
        [SerializeField] private float collapseRecoverSpeed = 2.5f;

        [Header("Meltdown Pose")]
        [SerializeField] private float meltdownBodyForward = 18f;
        [SerializeField] private float meltdownHeadDown = 26f;
        [SerializeField] private float meltdownBodyDrop = 0.06f;

        [Header("Hit Freeze")]
        [SerializeField] private float hitFreezeDuration = 0.05f;
        [SerializeField] private float headHitFreezeBonus = 0.04f;

        [Header("Head Jerk")]
        [SerializeField] private float headJerkDuration = 0.14f;
        [SerializeField] private float headJerkYawAngle = 36f;
        [SerializeField] private float headJerkPitchAngle = 14f;

        [Header("Furious / Meltdown Inserts")]
        [SerializeField] private float furiousStaggerChancePerSecond = 0.35f;
        [SerializeField] private float meltdownBreakChancePerSecond = 0.28f;
        [SerializeField] private float staggerDuration = 0.10f;
        [SerializeField] private float staggerBodyAngle = 10f;
        [SerializeField] private float staggerHeadAngle = 8f;

        [Header("Stage Entry Actions")]
        [SerializeField] private float annoyedEntryDuration = 0.22f;
        [SerializeField] private float agitatedEntryDuration = 0.26f;
        [SerializeField] private float furiousEntryDuration = 0.28f;
        [SerializeField] private float meltdownEntryDuration = 0.34f;

        [SerializeField] private float annoyedEntryHeadYaw = 18f;
        [SerializeField] private float annoyedEntryHeadPitch = 6f;

        [SerializeField] private float agitatedEntryBodyRoll = 12f;
        [SerializeField] private float agitatedEntryHeadYaw = 12f;

        [SerializeField] private float furiousEntryBodyYaw = 10f;
        [SerializeField] private float furiousEntryHeadYaw = 20f;
        [SerializeField] private float furiousEntryHeadPitch = -8f;

        [SerializeField] private float meltdownEntryBodyPitch = 18f;
        [SerializeField] private float meltdownEntryHeadYaw = 22f;
        [SerializeField] private float meltdownEntryHeadPitch = 18f;

        [Header("Meltdown Break")]
        [SerializeField] private float meltdownBreakDuration = 0.22f;
        [SerializeField] private float meltdownBreakBodyPitch = 16f;
        [SerializeField] private float meltdownBreakHeadYaw = 22f;
        [SerializeField] private float meltdownBreakHeadPitch = 16f;

        private EnemyReactionStage currentStage = EnemyReactionStage.Calm;
        private float progress01;

        private Vector3 bodyLocalPosDefault;
        private Quaternion bodyLocalRotDefault;
        private Vector3 headLocalPosDefault;
        private Quaternion headLocalRotDefault;

        private float stageWeightCurrent;
        private float stageWeightTarget;

        private float bodyHitBack;
        private float bodyHitSide;
        private float headHitBack;
        private float headHitSide;

        private float angerShake;
        private float disgustWobble;
        private float blindPanic;
        private float collapsePulse;

        private float hitFreezeTimer;

        private float staggerTimer;
        private float staggerSideSign = 1f;

        private float headJerkTimer;
        private float headJerkSideSign = 1f;

        private float meltdownBreakTimer;
        private float meltdownBreakSideSign = 1f;

        private StageEntryActionType stageEntryAction = StageEntryActionType.None;
        private float stageEntryTimer;
        private float stageEntryDuration;
        private float stageEntrySideSign = 1f;

        public EnemyReactionStage CurrentStage => currentStage;
        public float Progress01 => progress01;

        private void Awake()
        {
            if (bodyVisual == null)
            {
                bodyVisual = transform;
            }

            if (headVisual == null)
            {
                headVisual = bodyVisual;
            }

            bodyLocalPosDefault = bodyVisual.localPosition;
            bodyLocalRotDefault = bodyVisual.localRotation;

            headLocalPosDefault = headVisual.localPosition;
            headLocalRotDefault = headVisual.localRotation;
        }

        private void Update()
        {
            stageWeightCurrent = Mathf.Lerp(stageWeightCurrent, stageWeightTarget, Time.deltaTime * stageBlendSpeed);

            UpdateTimers();
            UpdateAutomaticInserts();

            if (hitFreezeTimer > 0f)
            {
                hitFreezeTimer -= Time.deltaTime;
                ApplyBodyLayer(true);
                ApplyHeadLayer(true);
                return;
            }

            bodyHitBack = Mathf.Lerp(bodyHitBack, 0f, Time.deltaTime * bodyHitRecoverSpeed);
            bodyHitSide = Mathf.Lerp(bodyHitSide, 0f, Time.deltaTime * bodyHitRecoverSpeed);

            headHitBack = Mathf.Lerp(headHitBack, 0f, Time.deltaTime * headHitRecoverSpeed);
            headHitSide = Mathf.Lerp(headHitSide, 0f, Time.deltaTime * headHitRecoverSpeed);

            angerShake = Mathf.Lerp(angerShake, 0f, Time.deltaTime * angerRecoverSpeed);
            disgustWobble = Mathf.Lerp(disgustWobble, 0f, Time.deltaTime * disgustRecoverSpeed);
            blindPanic = Mathf.Lerp(blindPanic, 0f, Time.deltaTime * panicRecoverSpeed);
            collapsePulse = Mathf.Lerp(collapsePulse, 0f, Time.deltaTime * collapseRecoverSpeed);

            ApplyBodyLayer(false);
            ApplyHeadLayer(false);
        }

        public void RefreshStage(int currentBreakdown, int targetBreakdown)
        {
            if (targetBreakdown <= 0)
            {
                progress01 = 0f;
                SetStage(EnemyReactionStage.Calm);
                return;
            }

            progress01 = Mathf.Clamp01((float)currentBreakdown / targetBreakdown);

            EnemyReactionStage nextStage;

            if (progress01 < annoyedThreshold)
            {
                nextStage = EnemyReactionStage.Calm;
            }
            else if (progress01 < agitatedThreshold)
            {
                nextStage = EnemyReactionStage.Annoyed;
            }
            else if (progress01 < furiousThreshold)
            {
                nextStage = EnemyReactionStage.Agitated;
            }
            else if (progress01 < 0.99f)
            {
                nextStage = EnemyReactionStage.Furious;
            }
            else
            {
                nextStage = EnemyReactionStage.Meltdown;
            }

            SetStage(nextStage);
        }

        public void ReactToHit(GameplayItemData itemData, bool isHeadHit, Vector3 incomingDirection, float reactionMultiplier = 1f)
        {
            float itemStrength = GetItemStrength(itemData) * Mathf.Max(0f, reactionMultiplier);
            float stageBoost = 1f + stageWeightTarget * 0.5f;

            float bodyHitMul = EvalBodyHitMultiplier();
            float headHitMul = EvalHeadHitMultiplier();
            float knockbackMul = EvalKnockbackMultiplier();
            float freezeMul = EvalFreezeMultiplier();

            float sideSign = 1f;
            if (incomingDirection.sqrMagnitude > 0.001f)
            {
                float localX = transform.InverseTransformDirection(incomingDirection).x;
                sideSign = localX >= 0f ? -1f : 1f;
            }

            bodyHitBack += itemStrength * stageBoost * bodyHitMul;
            bodyHitSide += sideSign * itemStrength * 0.9f * knockbackMul;

            if (isHeadHit)
            {
                headHitBack += 1.45f * itemStrength * headHitMul;
                headHitSide += sideSign * 1.35f * itemStrength * headHitMul;
                angerShake += 0.45f * itemStrength * bodyHitMul;
                hitFreezeTimer = (hitFreezeDuration + headHitFreezeBonus) * freezeMul;

                TriggerHeadJerk(sideSign, 1.0f * headHitMul);
            }
            else
            {
                headHitBack += 0.40f * itemStrength * 0.85f;
                headHitSide += sideSign * 0.28f * itemStrength * 0.85f;
                hitFreezeTimer = hitFreezeDuration * freezeMul;
            }

            ApplyItemMood(itemData, itemStrength);

            if (currentStage == EnemyReactionStage.Furious)
            {
                if (Random.value < 0.28f)
                {
                    TriggerStagger(sideSign);
                }
            }

            if (currentStage == EnemyReactionStage.Meltdown)
            {
                collapsePulse += 0.50f * itemStrength * EvalMeltdownMultiplier();

                if (Random.value < 0.55f)
                {
                    TriggerMeltdownBreak(sideSign);
                }
            }
        }

        private void UpdateTimers()
        {
            if (staggerTimer > 0f)
            {
                staggerTimer -= Time.deltaTime;
            }

            if (headJerkTimer > 0f)
            {
                headJerkTimer -= Time.deltaTime;
            }

            if (meltdownBreakTimer > 0f)
            {
                meltdownBreakTimer -= Time.deltaTime;
            }

            if (stageEntryTimer > 0f)
            {
                stageEntryTimer -= Time.deltaTime;
                if (stageEntryTimer <= 0f)
                {
                    stageEntryAction = StageEntryActionType.None;
                }
            }
        }

        private void UpdateAutomaticInserts()
        {
            if (currentStage == EnemyReactionStage.Furious && staggerTimer <= 0f)
            {
                float chance = furiousStaggerChancePerSecond * Time.deltaTime;
                if (Random.value < chance)
                {
                    TriggerStagger(Random.value < 0.5f ? -1f : 1f);
                }
            }

            if (currentStage == EnemyReactionStage.Meltdown && meltdownBreakTimer <= 0f)
            {
                float chance = meltdownBreakChancePerSecond * Time.deltaTime;
                if (Random.value < chance)
                {
                    TriggerMeltdownBreak(Random.value < 0.5f ? -1f : 1f);
                }
            }
        }

        private void TriggerHeadJerk(float sideSign, float intensity)
        {
            headJerkSideSign = Mathf.Sign(Mathf.Approximately(sideSign, 0f) ? 1f : sideSign);

            float finalIntensity = intensity * EvalHeadHitMultiplier();
            float vanityMul = EvalVanityHeadRecoverMultiplier();

            headJerkTimer = headJerkDuration * Mathf.Clamp(finalIntensity, 0.9f, 1.6f) * vanityMul;
        }

        private void TriggerStagger(float sideSign)
        {
            staggerSideSign = Mathf.Sign(Mathf.Approximately(sideSign, 0f) ? 1f : sideSign);
            staggerTimer = staggerDuration * EvalStaggerMultiplier();
        }

        private void TriggerMeltdownBreak(float sideSign)
        {
            meltdownBreakSideSign = Mathf.Sign(Mathf.Approximately(sideSign, 0f) ? 1f : sideSign);
            meltdownBreakTimer = meltdownBreakDuration * EvalMeltdownMultiplier();
        }

        private void TriggerStageEntryAction(EnemyReactionStage stage)
        {
            float bodyMul = EvalStageEntryBodyMultiplier();
            float headMul = EvalStageEntryHeadMultiplier();
            float shakeMul = EvalStageEntryShakeMultiplier();

            stageEntrySideSign = Random.value < 0.5f ? -1f : 1f;

            switch (stage)
            {
                case EnemyReactionStage.Annoyed:
                    stageEntryAction = StageEntryActionType.AnnoyedSnap;
                    stageEntryDuration = annoyedEntryDuration;
                    stageEntryTimer = annoyedEntryDuration;
                    TriggerHeadJerk(stageEntrySideSign, 1.1f * headMul);
                    break;

                case EnemyReactionStage.Agitated:
                    stageEntryAction = StageEntryActionType.AgitatedLean;
                    stageEntryDuration = agitatedEntryDuration;
                    stageEntryTimer = agitatedEntryDuration;
                    TriggerStagger(stageEntrySideSign);
                    break;

                case EnemyReactionStage.Furious:
                    stageEntryAction = StageEntryActionType.FuriousGlare;
                    stageEntryDuration = furiousEntryDuration;
                    stageEntryTimer = furiousEntryDuration;
                    TriggerHeadJerk(stageEntrySideSign, 1.15f * headMul);
                    hitFreezeTimer = Mathf.Max(hitFreezeTimer, 0.05f * shakeMul);
                    break;

                case EnemyReactionStage.Meltdown:
                    stageEntryAction = StageEntryActionType.MeltdownBreak;
                    stageEntryDuration = meltdownEntryDuration;
                    stageEntryTimer = meltdownEntryDuration;
                    TriggerMeltdownBreak(stageEntrySideSign);
                    TriggerHeadJerk(stageEntrySideSign, 1.2f * headMul);
                    hitFreezeTimer = Mathf.Max(hitFreezeTimer, 0.06f * shakeMul);
                    break;

                default:
                    stageEntryAction = StageEntryActionType.None;
                    stageEntryDuration = 0f;
                    stageEntryTimer = 0f;
                    break;
            }
        }

        private void SetStage(EnemyReactionStage nextStage)
        {
            if (currentStage == nextStage)
            {
                stageWeightTarget = GetStageWeight(nextStage);
                return;
            }

            currentStage = nextStage;
            stageWeightTarget = GetStageWeight(nextStage);

            switch (currentStage)
            {
                case EnemyReactionStage.Calm:
                    stageEntryAction = StageEntryActionType.None;
                    stageEntryTimer = 0f;
                    break;

                case EnemyReactionStage.Annoyed:
                    angerShake += 0.20f;
                    TriggerStageEntryAction(currentStage);
                    break;

                case EnemyReactionStage.Agitated:
                    angerShake += 0.35f;
                    disgustWobble += 0.22f;
                    TriggerStageEntryAction(currentStage);
                    break;

                case EnemyReactionStage.Furious:
                    angerShake += 0.60f;
                    disgustWobble += 0.35f;
                    blindPanic += 0.28f;
                    TriggerStageEntryAction(currentStage);
                    break;

                case EnemyReactionStage.Meltdown:
                    angerShake += 0.80f;
                    disgustWobble += 0.45f;
                    blindPanic += 0.55f;
                    collapsePulse += 0.80f;
                    TriggerStageEntryAction(currentStage);
                    break;
            }
        }

        private float GetStageWeight(EnemyReactionStage stage)
        {
            switch (stage)
            {
                case EnemyReactionStage.Annoyed: return 0.25f;
                case EnemyReactionStage.Agitated: return 0.52f;
                case EnemyReactionStage.Furious: return 0.82f;
                case EnemyReactionStage.Meltdown: return 1.10f;
                default: return 0f;
            }
        }

        private float GetItemStrength(GameplayItemData itemData)
        {
            if (itemData == null)
            {
                return 1f;
            }

            float forceFactor = Mathf.Clamp(itemData.throwForce / 18f, 0.7f, 1.8f);
            float scoreFactor = Mathf.Clamp(itemData.baseBreakdownScore / 20f, 0.7f, 1.8f);

            return (forceFactor * 0.55f) + (scoreFactor * 0.45f);
        }

        private void ApplyItemMood(GameplayItemData itemData, float itemStrength)
        {
            if (itemData == null)
            {
                angerShake += 0.18f * itemStrength;
                return;
            }

            switch (itemData.feedbackType)
            {
                case HitFeedbackType.ScalePunch:
                    bodyHitBack += 0.25f * itemStrength;
                    angerShake += 0.15f * itemStrength;
                    break;

                case HitFeedbackType.FlashColor:
                    disgustWobble += 0.45f * itemStrength;
                    angerShake += 0.20f * itemStrength;
                    break;

                case HitFeedbackType.SmallKnockback:
                    bodyHitBack += 0.70f * itemStrength;
                    bodyHitSide += 0.24f * Mathf.Sign(bodyHitSide == 0f ? 1f : bodyHitSide) * itemStrength;
                    angerShake += 0.28f * itemStrength;
                    break;

                case HitFeedbackType.FoamTint:
                    blindPanic += 0.75f * itemStrength;
                    disgustWobble += 0.20f * itemStrength;
                    break;

                case HitFeedbackType.Wiggle:
                    disgustWobble += 0.55f * itemStrength;
                    angerShake += 0.12f * itemStrength;
                    break;

                default:
                    angerShake += 0.18f * itemStrength;
                    break;
            }
        }

        private void ApplyBodyLayer(bool frozen)
        {
            float time = Time.time;

            float stageBodyYaw = GetStageBodyYaw();
            float baseYaw = Mathf.Sin(time * (0.8f + stageWeightCurrent * 1.9f)) * stageBodyYaw;
            float baseRoll = Mathf.Sin(time * (1.3f + stageWeightCurrent * 2.2f)) * (stageBodyYaw * 0.35f);

            float angerYaw = Mathf.Sin(time * 15f) * angerShake * 7f;
            float angerRoll = Mathf.Cos(time * 18f) * angerShake * 5f;

            float disgustRoll = Mathf.Sin(time * 9f) * disgustWobble * 7f;
            float panicYaw = Mathf.Sin(time * 21f) * blindPanic * 9f * EvalPanicShakeMultiplier();
            float meltdownForward = currentStage == EnemyReactionStage.Meltdown
                ? ((meltdownBodyForward * EvalIntimidationChestForwardMultiplier()) * stageWeightCurrent) + (collapsePulse * 7f * EvalMeltdownMultiplier())
                : 0f;

            float recoilPitch = -bodyHitBack * hitBodyKickAngle;
            float recoilRoll = bodyHitSide * hitBodySideAngle;

            float staggerRoll = 0f;
            float staggerYaw = 0f;
            if (staggerTimer > 0f)
            {
                float t = staggerTimer / staggerDuration;
                staggerRoll = staggerSideSign * staggerBodyAngle * EvalStaggerMultiplier() * t;
                staggerYaw = staggerSideSign * staggerBodyAngle * 0.45f * EvalStaggerMultiplier() * t;
            }

            float breakPitch = 0f;
            float breakRoll = 0f;
            if (meltdownBreakTimer > 0f)
            {
                float t = meltdownBreakTimer / meltdownBreakDuration;
                breakPitch = meltdownBreakBodyPitch * EvalMeltdownMultiplier() * t;
                breakRoll = meltdownBreakSideSign * 5f * Mathf.Sin((1f - t) * 18f) * t;
            }

            float entryPitch = 0f;
            float entryYaw = 0f;
            float entryRoll = 0f;
            Vector3 entryOffset = Vector3.zero;
            ApplyStageEntryBody(ref entryPitch, ref entryYaw, ref entryRoll, ref entryOffset);

            float finalPitch = recoilPitch + meltdownForward + breakPitch + entryPitch;
            float finalYaw = entryYaw + (frozen ? 0f : (baseYaw + angerYaw + panicYaw + staggerYaw));
            float finalRoll = recoilRoll + staggerRoll + breakRoll + entryRoll + (frozen ? 0f : (baseRoll + angerRoll + disgustRoll));

            Vector3 targetPos = bodyLocalPosDefault + entryOffset;

            if (currentStage == EnemyReactionStage.Meltdown)
            {
                targetPos += Vector3.down * (meltdownBodyDrop + collapsePulse * 0.02f);
            }

            if (meltdownBreakTimer > 0f)
            {
                float t = meltdownBreakTimer / meltdownBreakDuration;
                targetPos += Vector3.down * (0.02f * t);
            }

            bodyVisual.localPosition = targetPos;
            bodyVisual.localRotation = bodyLocalRotDefault * Quaternion.Euler(finalPitch, finalYaw, finalRoll);
        }

        private void ApplyHeadLayer(bool frozen)
        {
            float time = Time.time;

            float stageHeadYaw = GetStageHeadYaw();
            float baseYaw = Mathf.Sin(time * (1.5f + stageWeightCurrent * 2.5f)) * stageHeadYaw;
            float basePitch = Mathf.Cos(time * (1.1f + stageWeightCurrent * 1.8f)) * (stageHeadYaw * 0.35f);

            float panicYaw = Mathf.Sin(time * 24f) * blindPanic * 13f * EvalPanicShakeMultiplier();
            float panicPitch = Mathf.Cos(time * 19f) * blindPanic * 8f * EvalPanicShakeMultiplier();

            float headPitch = -headHitBack * headHitKickAngle * EvalHeadHitMultiplier();
            float headYaw = headHitSide * headHitSideAngle * EvalHeadHitMultiplier();

            float jerkYaw = 0f;
            float jerkPitch = 0f;
            if (headJerkTimer > 0f)
            {
                float t = headJerkTimer / headJerkDuration;
                jerkYaw = headJerkSideSign * headJerkYawAngle * EvalHeadHitMultiplier() * t;
                jerkPitch = -headJerkPitchAngle * EvalHeadHitMultiplier() * t;
            }

            float breakYaw = 0f;
            float breakPitch = 0f;
            if (meltdownBreakTimer > 0f)
            {
                float t = meltdownBreakTimer / meltdownBreakDuration;
                breakYaw = meltdownBreakSideSign * meltdownBreakHeadYaw * Mathf.Sin((1f - t) * 20f) * t;
                breakPitch = meltdownBreakHeadPitch * t;
            }

            float entryPitch = 0f;
            float entryYaw = 0f;
            ApplyStageEntryHead(ref entryPitch, ref entryYaw);

            float meltdownPitch = currentStage == EnemyReactionStage.Meltdown
                ? (meltdownHeadDown * stageWeightCurrent * EvalMeltdownMultiplier()) + (collapsePulse * 5f * EvalMeltdownMultiplier())
                : 0f;

            float finalPitch = headPitch + jerkPitch + breakPitch + entryPitch + meltdownPitch + (frozen ? 0f : (basePitch + panicPitch));
            float finalYaw = headYaw + jerkYaw + breakYaw + entryYaw + (frozen ? 0f : (baseYaw + panicYaw));

            headVisual.localPosition = headLocalPosDefault;
            headVisual.localRotation = headLocalRotDefault * Quaternion.Euler(finalPitch, finalYaw, 0f);
        }

        private void ApplyStageEntryBody(ref float pitch, ref float yaw, ref float roll, ref Vector3 offset)
        {
            if (stageEntryAction == StageEntryActionType.None || stageEntryTimer <= 0f || stageEntryDuration <= 0f)
            {
                return;
            }

            float t = stageEntryTimer / stageEntryDuration;

            switch (stageEntryAction)
            {
                case StageEntryActionType.AgitatedLean:
                    roll += stageEntrySideSign * agitatedEntryBodyRoll * EvalStageEntryBodyMultiplier() * t;
                    yaw += stageEntrySideSign * agitatedEntryBodyRoll * 0.35f * EvalStageEntryBodyMultiplier() * t;
                    break;

                case StageEntryActionType.FuriousGlare:
                    yaw += stageEntrySideSign * furiousEntryBodyYaw * EvalStageEntryBodyMultiplier() * t;
                    roll += stageEntrySideSign * 4f * EvalStageEntryShakeMultiplier() * t;
                    break;

                case StageEntryActionType.MeltdownBreak:
                    pitch += meltdownEntryBodyPitch * EvalStageEntryBodyMultiplier() * t;
                    roll += stageEntrySideSign * 5f * EvalStageEntryShakeMultiplier() * Mathf.Sin((1f - t) * 14f) * t;
                    offset += Vector3.down * (0.018f * EvalStageEntryBodyMultiplier() * t);
                    break;
                }
        }

        private void ApplyStageEntryHead(ref float pitch, ref float yaw)
        {
            if (stageEntryAction == StageEntryActionType.None || stageEntryTimer <= 0f || stageEntryDuration <= 0f)
            {
                return;
            }

            float t = stageEntryTimer / stageEntryDuration;

            switch (stageEntryAction)
            {
                case StageEntryActionType.AnnoyedSnap:
                    yaw += stageEntrySideSign * annoyedEntryHeadYaw * EvalStageEntryHeadMultiplier() * t;
                    pitch += annoyedEntryHeadPitch * EvalStageEntryHeadMultiplier() * t;
                    break;

                case StageEntryActionType.AgitatedLean:
                    yaw += stageEntrySideSign * agitatedEntryHeadYaw * EvalStageEntryHeadMultiplier() * t;
                    break;

                case StageEntryActionType.FuriousGlare:
                    yaw += stageEntrySideSign * furiousEntryHeadYaw * EvalStageEntryHeadMultiplier() * t;
                    pitch += furiousEntryHeadPitch * EvalStageEntryHeadMultiplier() * t;
                    break;

                case StageEntryActionType.MeltdownBreak:
                    yaw += stageEntrySideSign * meltdownEntryHeadYaw * EvalStageEntryHeadMultiplier() * Mathf.Sin((1f - t) * 16f) * t;
                    pitch += meltdownEntryHeadPitch * EvalStageEntryHeadMultiplier() * t;
                    break;
            }
        }

        private float GetStageBodyYaw()
        {
            float baseValue;

            switch (currentStage)
            {
                case EnemyReactionStage.Annoyed: baseValue = annoyedBodyYaw; break;
                case EnemyReactionStage.Agitated: baseValue = agitatedBodyYaw; break;
                case EnemyReactionStage.Furious: baseValue = furiousBodyYaw; break;
                case EnemyReactionStage.Meltdown: baseValue = meltdownBodyYaw; break;
                default: baseValue = calmBodyYaw; break;
            }

            return baseValue * EvalIdleBodyYawMultiplier();
        }

        // =========================================================
        // Archetype Utility
        // =========================================================

        private bool HasArchetype()
        {
            return useArchetype && enemyArchetype != null;
        }

        private float GetCurrentStageArchetypeMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            switch (currentStage)
            {
                case EnemyReactionStage.Annoyed:
                    return enemyArchetype.annoyedMultiplier;
                case EnemyReactionStage.Agitated:
                    return enemyArchetype.agitatedMultiplier;
                case EnemyReactionStage.Furious:
                    return enemyArchetype.furiousMultiplier;
                case EnemyReactionStage.Meltdown:
                    return enemyArchetype.meltdownMultiplier;
                default:
                    return enemyArchetype.calmMultiplier;
            }
        }

        private float EvalIdleBodyYawMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.idleBodySwayMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalIdleHeadYawMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.idleHeadMotionMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalBodyHitMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.bodyHitMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalHeadHitMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.headHitMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalKnockbackMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.knockbackMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalFreezeMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.hitFreezeMultiplier;
        }

        private float EvalStaggerMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.staggerMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalStageEntryBodyMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.stageEntryBodyMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalStageEntryHeadMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.stageEntryHeadMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalStageEntryShakeMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.stageEntryShakeMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalVanityHeadRecoverMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.vanityHeadRecoverMultiplier;
        }

        private float EvalIntimidationChestForwardMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.intimidationChestForwardMultiplier;
        }

        private float EvalPanicShakeMultiplier()
        {
            if (!HasArchetype())
            {
                return 1f;
            }

            return enemyArchetype.panicShakeMultiplier * GetCurrentStageArchetypeMultiplier();
        }

        private float EvalMeltdownMultiplier()
        {
            return HasArchetype() ? enemyArchetype.meltdownMultiplier : 1f;
        }

        private float GetStageHeadYaw()
        {
            float baseValue;

            switch (currentStage)
            {
                case EnemyReactionStage.Annoyed: baseValue = annoyedHeadYaw; break;
                case EnemyReactionStage.Agitated: baseValue = agitatedHeadYaw; break;
                case EnemyReactionStage.Furious: baseValue = furiousHeadYaw; break;
                case EnemyReactionStage.Meltdown: baseValue = meltdownHeadYaw; break;
                default: baseValue = calmHeadYaw; break;
            }

            return baseValue * EvalIdleHeadYawMultiplier();
        }
    }
}
