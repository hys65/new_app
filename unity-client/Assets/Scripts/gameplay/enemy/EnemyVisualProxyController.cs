using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyVisualProxyController : MonoBehaviour
    {
        private enum ProxyPose
        {
            Idle = 0,
            Telegraph = 1,
            Guard = 2,
            Break = 3,
            Weak = 4
        }

        [Header("References")]
        [SerializeField] private Transform bodyPivot;
        [SerializeField] private Transform torso;
        [SerializeField] private Transform headAnchor;
        [SerializeField] private Transform headVisual;
        [SerializeField] private Transform leftArmPivot;
        [SerializeField] private Transform leftArmVisual;
        [SerializeField] private Transform rightArmPivot;
        [SerializeField] private Transform rightArmVisual;
        [SerializeField] private Transform defenseVisualAnchor;
        [SerializeField] private Transform guardVisual;

        [Header("Blend")]
        [SerializeField] private float poseBlendSpeed = 10f;

        [Header("Idle Pose")]
        [SerializeField] private Vector3 idleTorsoLocalPos = Vector3.zero;
        [SerializeField] private Vector3 idleTorsoLocalRot = Vector3.zero;
        [SerializeField] private Vector3 idleHeadLocalPos = Vector3.zero;
        [SerializeField] private Vector3 idleHeadLocalRot = Vector3.zero;
        [SerializeField] private Vector3 idleLeftArmLocalRot = new Vector3(0f, 0f, -6f);
        [SerializeField] private Vector3 idleRightArmLocalRot = new Vector3(0f, 0f, 6f);
        [SerializeField] private Vector3 idleGuardLocalPos = new Vector3(0.28f, 0.05f, 0.18f);
        [SerializeField] private Vector3 idleGuardLocalRot = new Vector3(0f, -12f, 8f);

        [Header("Prepare Defense Pose")]
        [SerializeField] private Vector3 telegraphTorsoLocalPos = new Vector3(0f, 0f, -0.02f);
        [SerializeField] private Vector3 telegraphTorsoLocalRot = new Vector3(-4f, 0f, 0f);
        [SerializeField] private Vector3 telegraphHeadLocalRot = new Vector3(4f, 0f, 0f);
        [SerializeField] private Vector3 telegraphLeftArmLocalRot = new Vector3(-18f, 0f, -20f);
        [SerializeField] private Vector3 telegraphRightArmLocalRot = new Vector3(-18f, 0f, 20f);
        [SerializeField] private Vector3 telegraphGuardLocalPos = new Vector3(0.05f, 0.12f, 0.18f);
        [SerializeField] private Vector3 telegraphGuardLocalRot = new Vector3(-8f, -10f, 10f);

        [Header("Guard Pose")]
        [SerializeField] private Vector3 guardTorsoLocalPos = new Vector3(0f, 0f, -0.05f);
        [SerializeField] private Vector3 guardTorsoLocalRot = new Vector3(-8f, 0f, 0f);
        [SerializeField] private Vector3 guardHeadLocalRot = new Vector3(8f, 0f, 0f);
        [SerializeField] private Vector3 guardLeftArmLocalRot = new Vector3(-28f, 0f, -8f);
        [SerializeField] private Vector3 guardRightArmLocalRot = new Vector3(-28f, 0f, 8f);
        [SerializeField] private Vector3 guardGuardLocalPos = new Vector3(0.00f, 0.18f, 0.26f);
        [SerializeField] private Vector3 guardGuardLocalRot = new Vector3(-12f, 0f, 8f);

        [Header("Break Pose")]
        [SerializeField] private Vector3 breakTorsoLocalPos = new Vector3(0f, 0f, -0.01f);
        [SerializeField] private Vector3 breakTorsoLocalRot = new Vector3(8f, 0f, 0f);
        [SerializeField] private Vector3 breakHeadLocalRot = new Vector3(-10f, 0f, 0f);
        [SerializeField] private Vector3 breakLeftArmLocalRot = new Vector3(12f, 0f, -85f);
        [SerializeField] private Vector3 breakRightArmLocalRot = new Vector3(12f, 0f, 85f);
        [SerializeField] private Vector3 breakGuardLocalPos = new Vector3(0.2f, 0.1f, 0.08f);
        [SerializeField] private Vector3 breakGuardLocalRot = new Vector3(18f, 0f, 45f);

        [Header("Weak Pose")]
        [SerializeField] private Vector3 weakTorsoLocalRot = new Vector3(-4f, 0f, 0f);
        [SerializeField] private Vector3 weakHeadLocalRot = new Vector3(14f, 0f, 0f);
        [SerializeField] private Vector3 weakLeftArmLocalRot = new Vector3(-8f, 0f, -40f);
        [SerializeField] private Vector3 weakRightArmLocalRot = new Vector3(-18f, 0f, 40f);

        [Header("Runtime Debug")]
        [SerializeField] private bool guardVisible;
        [SerializeField] private string debugPoseName = "Idle";

        private ProxyPose currentPose = ProxyPose.Idle;
        private ProxyPose targetPose = ProxyPose.Idle;

        private Vector3 currentTorsoPos;
        private Vector3 currentTorsoRot;
        private Vector3 currentHeadPos;
        private Vector3 currentHeadRot;
        private Vector3 currentLeftArmRot;
        private Vector3 currentRightArmRot;
        private Vector3 currentGuardPos;
        private Vector3 currentGuardRot;

        private void Awake()
        {
            if (bodyPivot == null)
            {
                bodyPivot = transform;
            }

            currentTorsoPos = idleTorsoLocalPos;
            currentTorsoRot = idleTorsoLocalRot;
            currentHeadPos = idleHeadLocalPos;
            currentHeadRot = idleHeadLocalRot;
            currentLeftArmRot = idleLeftArmLocalRot;
            currentRightArmRot = idleRightArmLocalRot;
            currentGuardPos = idleGuardLocalPos;
            currentGuardRot = idleGuardLocalRot;

            ApplyImmediate();
            UpdateGuardVisibility(false);
        }

        private void LateUpdate()
        {
            Vector3 targetTorsoPos = idleTorsoLocalPos;
            Vector3 targetTorsoRot = idleTorsoLocalRot;
            Vector3 targetHeadPos = idleHeadLocalPos;
            Vector3 targetHeadRot = idleHeadLocalRot;
            Vector3 targetLeftArmRot = idleLeftArmLocalRot;
            Vector3 targetRightArmRot = idleRightArmLocalRot;
            Vector3 targetGuardPos = idleGuardLocalPos;
            Vector3 targetGuardRot = idleGuardLocalRot;
            bool shouldShowGuard = false;

            switch (targetPose)
            {
                case ProxyPose.Telegraph:
                    targetTorsoPos = telegraphTorsoLocalPos;
                    targetTorsoRot = telegraphTorsoLocalRot;
                    targetHeadRot = telegraphHeadLocalRot;
                    targetLeftArmRot = telegraphLeftArmLocalRot;
                    targetRightArmRot = telegraphRightArmLocalRot;
                    targetGuardPos = telegraphGuardLocalPos;
                    targetGuardRot = telegraphGuardLocalRot;
                    shouldShowGuard = true;
                    break;

                case ProxyPose.Guard:
                    targetTorsoPos = guardTorsoLocalPos;
                    targetTorsoRot = guardTorsoLocalRot;
                    targetHeadRot = guardHeadLocalRot;
                    targetLeftArmRot = guardLeftArmLocalRot;
                    targetRightArmRot = guardRightArmLocalRot;
                    targetGuardPos = guardGuardLocalPos;
                    targetGuardRot = guardGuardLocalRot;
                    shouldShowGuard = true;
                    break;

                case ProxyPose.Break:
                    targetTorsoPos = breakTorsoLocalPos;
                    targetTorsoRot = breakTorsoLocalRot;
                    targetHeadRot = breakHeadLocalRot;
                    targetLeftArmRot = breakLeftArmLocalRot;
                    targetRightArmRot = breakRightArmLocalRot;
                    targetGuardPos = breakGuardLocalPos;
                    targetGuardRot = breakGuardLocalRot;
                    shouldShowGuard = true;
                    break;

                case ProxyPose.Weak:
                    targetTorsoRot = weakTorsoLocalRot;
                    targetHeadRot = weakHeadLocalRot;
                    targetLeftArmRot = weakLeftArmLocalRot;
                    targetRightArmRot = weakRightArmLocalRot;
                    shouldShowGuard = true;
                    break;
            }

            float t = Time.deltaTime * poseBlendSpeed;

            currentTorsoPos = Vector3.Lerp(currentTorsoPos, targetTorsoPos, t);
            currentTorsoRot = Vector3.Lerp(currentTorsoRot, targetTorsoRot, t);
            currentHeadPos = Vector3.Lerp(currentHeadPos, targetHeadPos, t);
            currentHeadRot = Vector3.Lerp(currentHeadRot, targetHeadRot, t);
            currentLeftArmRot = Vector3.Lerp(currentLeftArmRot, targetLeftArmRot, t);
            currentRightArmRot = Vector3.Lerp(currentRightArmRot, targetRightArmRot, t);
            currentGuardPos = Vector3.Lerp(currentGuardPos, targetGuardPos, t);
            currentGuardRot = Vector3.Lerp(currentGuardRot, targetGuardRot, t);

            ApplyImmediate();
            UpdateGuardVisibility(shouldShowGuard);

            currentPose = targetPose;
            debugPoseName = currentPose.ToString();
        }

        public void SetIdlePose()
        {
            targetPose = ProxyPose.Idle;
        }

        public void SetPrepareDefensePose()
        {
            targetPose = ProxyPose.Telegraph;
        }

        public void SetGuardPose()
        {
            targetPose = ProxyPose.Guard;
        }

        public void SetDefenseBreakPose()
        {
            targetPose = ProxyPose.Break;
        }

        public void SetWeakPose()
        {
            targetPose = ProxyPose.Weak;
        }

        public void ApplyDefenseState(EnemyDefenseWindowState state, bool defenseActive)
        {
            if (defenseActive)
            {
                SetGuardPose();
                return;
            }

            switch (state)
            {
                case EnemyDefenseWindowState.Telegraph:
                    SetPrepareDefensePose();
                    break;

                case EnemyDefenseWindowState.Active:
                    SetGuardPose();
                    break;

                case EnemyDefenseWindowState.Recover:
                    SetIdlePose();
                    break;

                default:
                    SetIdlePose();
                    break;
            }
        }

        public void ApplyPopupState(string popupText)
        {
            if (string.IsNullOrEmpty(popupText))
            {
                return;
            }

            string key = popupText.Trim().ToUpperInvariant();

            switch (key)
            {
                case "GUARD":
                    SetPrepareDefensePose();
                    break;

                case "BLOCK":
                    SetGuardPose();
                    break;

                case "BREAK":
                    SetDefenseBreakPose();
                    break;

                case "WEAK":
                    SetWeakPose();
                    break;
            }
        }

        private void ApplyImmediate()
        {
            if (torso != null)
            {
                torso.localPosition = currentTorsoPos;
                torso.localRotation = Quaternion.Euler(currentTorsoRot);
            }

            if (headAnchor != null)
            {
                headAnchor.localPosition = currentHeadPos;
                headAnchor.localRotation = Quaternion.Euler(currentHeadRot);
            }

            if (leftArmPivot != null)
            {
                leftArmPivot.localRotation = Quaternion.Euler(currentLeftArmRot);
            }

            if (rightArmPivot != null)
            {
                rightArmPivot.localRotation = Quaternion.Euler(currentRightArmRot);
            }

            if (defenseVisualAnchor != null)
            {
                defenseVisualAnchor.localPosition = currentGuardPos;
                defenseVisualAnchor.localRotation = Quaternion.Euler(currentGuardRot);
            }
            else if (guardVisual != null)
            {
                guardVisual.localPosition = currentGuardPos;
                guardVisual.localRotation = Quaternion.Euler(currentGuardRot);
            }
        }

        private void UpdateGuardVisibility(bool visible)
        {
            guardVisible = visible;

            if (guardVisual != null)
            {
                guardVisual.gameObject.SetActive(visible);
            }
        }
    }
}
