using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyDefenseVisualLayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform bodyVisualPivot;
        [SerializeField] private Transform headVisualPivot;
        [SerializeField] private EnemyDefenseVisualProfileData visualProfile;

        [Header("Optional")]
        [SerializeField] private EnemyReactionLayerController reactionLayerController;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLog = false;

        private Vector3 bodyBaseLocalPos;
        private Quaternion bodyBaseLocalRot;

        private Vector3 headBaseLocalPos;
        private Quaternion headBaseLocalRot;

        private float guardWeight;
        private float blockWeight;
        private float breakWeight;
        private float weakWeight;

        private float guardTimer;
        private float breakTimer;
        private float weakTimer;

        private Vector3 blockBodyPosOffset;
        private Vector3 blockBodyRotOffset;
        private Vector3 blockHeadRotOffset;

        private EnemyDefenseWindowState defenseWindowState = EnemyDefenseWindowState.None;
        private float defenseWindowNormalized;
        private bool defenseWeakWindowOpen;

        private float telegraphPoseWeight;
        private float activePoseWeight;
        private float recoverPoseWeight;

        private float noiseSeed;

        private void Awake()
        {
            if (bodyVisualPivot == null)
            {
                Debug.LogError("[EnemyDefenseVisualLayerController] bodyVisualPivot is missing.", this);
                enabled = false;
                return;
            }

            bodyBaseLocalPos = bodyVisualPivot.localPosition;
            bodyBaseLocalRot = bodyVisualPivot.localRotation;

            if (headVisualPivot != null)
            {
                headBaseLocalPos = headVisualPivot.localPosition;
                headBaseLocalRot = headVisualPivot.localRotation;
            }

            noiseSeed = Random.Range(0f, 1000f);
        }

        private void LateUpdate()
        {
            if (visualProfile == null) return;

            UpdateTimers();
            UpdateWeights();
            UpdateStateWindowWeights();
            ApplyVisualPose();
        }

        private void UpdateStateWindowWeights()
        {
            if (visualProfile == null)
            {
                return;
            }

            float targetTelegraph = defenseWindowState == EnemyDefenseWindowState.Telegraph ? 1f : 0f;
            float targetActive = defenseWindowState == EnemyDefenseWindowState.Active ? 1f : 0f;
            float targetRecover = defenseWindowState == EnemyDefenseWindowState.Recover ? 1f : 0f;

            telegraphPoseWeight = Mathf.MoveTowards(
                telegraphPoseWeight,
                targetTelegraph,
                Time.deltaTime * visualProfile.stateWindowBlendSpeed
            );

            activePoseWeight = Mathf.MoveTowards(
                activePoseWeight,
                targetActive,
                Time.deltaTime * visualProfile.stateWindowBlendSpeed
            );

            recoverPoseWeight = Mathf.MoveTowards(
                recoverPoseWeight,
                targetRecover,
                Time.deltaTime * visualProfile.stateWindowBlendSpeed
            );
        }

        private void UpdateTimers()
        {
            if (guardTimer > 0f) guardTimer -= Time.deltaTime;
            if (breakTimer > 0f) breakTimer -= Time.deltaTime;
            if (weakTimer > 0f) weakTimer -= Time.deltaTime;
        }

        private void UpdateWeights()
        {
            float targetGuard = guardTimer > 0f ? 1f : 0f;
            float targetBreak = breakTimer > 0f ? 1f : 0f;
            float targetWeak = weakTimer > 0f ? 1f : 0f;

            guardWeight = Mathf.MoveTowards(
                guardWeight,
                targetGuard,
                Time.deltaTime * (targetGuard > guardWeight ? visualProfile.guardBlendIn : visualProfile.guardBlendOut)
            );

            blockWeight = Mathf.MoveTowards(
                blockWeight,
                0f,
                Time.deltaTime * visualProfile.blockRecoverSpeed
            );

            breakWeight = Mathf.MoveTowards(
                breakWeight,
                targetBreak,
                Time.deltaTime * visualProfile.breakRecoverSpeed
            );

            weakWeight = Mathf.MoveTowards(
                weakWeight,
                targetWeak,
                Time.deltaTime * visualProfile.weakRecoverSpeed
            );

            blockBodyPosOffset = Vector3.Lerp(blockBodyPosOffset, Vector3.zero, Time.deltaTime * visualProfile.blockRecoverSpeed);
            blockBodyRotOffset = Vector3.Lerp(blockBodyRotOffset, Vector3.zero, Time.deltaTime * visualProfile.blockRecoverSpeed);
            blockHeadRotOffset = Vector3.Lerp(blockHeadRotOffset, Vector3.zero, Time.deltaTime * visualProfile.blockRecoverSpeed);
        }

        private void ApplyVisualPose()
        {
            float bodyMul = visualProfile.bodyMotionMultiplier;
            float headMul = visualProfile.headMotionMultiplier;

            Vector3 finalBodyPos = bodyBaseLocalPos;
            Vector3 finalBodyEuler = Vector3.zero;

            Vector3 finalHeadEuler = Vector3.zero;

            // TELEGRAPH
            finalBodyPos += visualProfile.telegraphBodyPosition * telegraphPoseWeight * bodyMul;
            finalBodyEuler += visualProfile.telegraphBodyRotation * telegraphPoseWeight * bodyMul;
            finalHeadEuler += visualProfile.telegraphHeadRotation * telegraphPoseWeight * headMul;

            // ACTIVE
            finalBodyPos += visualProfile.activeBodyPosition * activePoseWeight * bodyMul;
            finalBodyEuler += visualProfile.activeBodyRotation * activePoseWeight * bodyMul;
            finalHeadEuler += visualProfile.activeHeadRotation * activePoseWeight * headMul;

            // RECOVER
            finalBodyPos += visualProfile.recoverBodyPosition * recoverPoseWeight * bodyMul;
            finalBodyEuler += visualProfile.recoverBodyRotation * recoverPoseWeight * bodyMul;
            finalHeadEuler += visualProfile.recoverHeadRotation * recoverPoseWeight * headMul;

            // GUARD
            finalBodyPos += visualProfile.guardBodyPosition * guardWeight * bodyMul;
            finalBodyEuler += visualProfile.guardBodyRotation * guardWeight * bodyMul;
            finalHeadEuler += visualProfile.guardHeadRotation * guardWeight * headMul;

            // BLOCK
            finalBodyPos += blockBodyPosOffset * blockWeight * bodyMul;
            finalBodyEuler += blockBodyRotOffset * blockWeight * bodyMul;
            finalHeadEuler += blockHeadRotOffset * blockWeight * headMul;

            // BREAK
            finalBodyPos += visualProfile.breakBodyPosition * breakWeight * bodyMul;
            finalBodyEuler += visualProfile.breakBodyRotation * breakWeight * bodyMul;
            finalHeadEuler += visualProfile.breakHeadRotation * breakWeight * headMul;

            if (breakWeight > 0.01f)
            {
                float t = Time.time * visualProfile.breakNoiseSpeed;
                float nx = (Mathf.PerlinNoise(noiseSeed, t) - 0.5f) * 2f;
                float ny = (Mathf.PerlinNoise(noiseSeed + 10f, t) - 0.5f) * 2f;

                Vector3 breakNoise = new Vector3(nx, ny, 0f) * visualProfile.breakNoiseAmplitude * breakWeight;
                finalBodyEuler += breakNoise;
            }

            // WEAK
            finalBodyEuler += visualProfile.weakBodyRotation * weakWeight * bodyMul;
            finalHeadEuler += visualProfile.weakHeadRotation * weakWeight * headMul;

            bodyVisualPivot.localPosition = finalBodyPos;
            bodyVisualPivot.localRotation = bodyBaseLocalRot * Quaternion.Euler(finalBodyEuler);

            if (headVisualPivot != null)
            {
                headVisualPivot.localPosition = headBaseLocalPos;
                headVisualPivot.localRotation = headBaseLocalRot * Quaternion.Euler(finalHeadEuler);
            }
        }

        public void SetDefenseWindowState(EnemyDefenseWindowState state, float normalized, bool weakWindowOpen)
        {
            defenseWindowState = state;
            defenseWindowNormalized = normalized;
            defenseWeakWindowOpen = weakWindowOpen;
        }

        public void ApplyDefenseVisual(string popupText, bool isHeadHit, float reactionMultiplier = 1f)
        {
            if (visualProfile == null) return;
            if (string.IsNullOrEmpty(popupText)) return;

            string key = popupText.Trim().ToUpperInvariant();

            if (enableDebugLog)
            {
                Debug.Log($"[EnemyDefenseVisualLayer] ApplyDefenseVisual -> {key}, headHit={isHeadHit}, reaction={reactionMultiplier}", this);
            }

            switch (key)
            {
                case "GUARD":
                    TriggerGuard();
                    break;

                case "BLOCK":
                    TriggerBlock(reactionMultiplier);
                    break;

                case "BREAK":
                    TriggerBreak(reactionMultiplier);
                    break;

                case "WEAK":
                    TriggerWeak(isHeadHit, reactionMultiplier);
                    break;
            }
        }

        private void TriggerGuard()
        {
            guardTimer = visualProfile.guardHoldTime;
        }

        private void TriggerBlock(float reactionMultiplier)
        {
            blockWeight = 1f;

            float m = Mathf.Max(0.35f, reactionMultiplier) * visualProfile.blockImpactStrength;

            blockBodyPosOffset = visualProfile.blockBodyKickPosition * m;
            blockBodyRotOffset = visualProfile.blockBodyKickRotation * m;
            blockHeadRotOffset = visualProfile.blockHeadKickRotation * m;

            guardTimer = Mathf.Max(guardTimer, visualProfile.guardHoldTime * 0.75f);
        }

        private void TriggerBreak(float reactionMultiplier)
        {
            float m = Mathf.Max(0.5f, reactionMultiplier);

            breakWeight = 1f;
            breakTimer = visualProfile.breakHoldTime * Mathf.Lerp(0.9f, 1.3f, Mathf.Clamp01(m - 0.5f));

            guardTimer = 0f;
        }

        private void TriggerWeak(bool isHeadHit, float reactionMultiplier)
        {
            weakWeight = isHeadHit ? 1f : 0.6f;
            weakTimer = visualProfile.weakHoldTime * Mathf.Clamp(reactionMultiplier, 0.8f, 1.4f);
        }

        [ContextMenu("Test GUARD")]
        private void TestGuard()
        {
            ApplyDefenseVisual("GUARD", false, 1f);
        }

        [ContextMenu("Test BLOCK")]
        private void TestBlock()
        {
            ApplyDefenseVisual("BLOCK", false, 1f);
        }

        [ContextMenu("Test BREAK")]
        private void TestBreak()
        {
            ApplyDefenseVisual("BREAK", false, 1f);
        }

        [ContextMenu("Test WEAK")]
        private void TestWeak()
        {
            ApplyDefenseVisual("WEAK", true, 1f);
        }
    }
}
