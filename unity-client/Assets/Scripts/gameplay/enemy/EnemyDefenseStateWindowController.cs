using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyDefenseStateWindowController : MonoBehaviour
    {
        [Header("References")]
        public EnemyDefenseStateWindowProfileData stateProfile;
        [SerializeField] private EnemyDefenseVisualLayerController defenseVisualLayer;
        [SerializeField] private EnemyDefenseController defenseController;

        [Header("Runtime")]
        [SerializeField] private EnemyDefenseWindowState currentState = EnemyDefenseWindowState.None;
        [SerializeField] private float stateTimer;
        [SerializeField] private bool activeWindowEnabled;
        [SerializeField] private bool weakWindowEnabled;

        private float nextCycleDelayTimer;

        public EnemyDefenseWindowState CurrentState => currentState;
        public bool IsTelegraph => currentState == EnemyDefenseWindowState.Telegraph;
        public bool IsActive => currentState == EnemyDefenseWindowState.Active;
        public bool IsRecover => currentState == EnemyDefenseWindowState.Recover;
        public bool IsDefenseLogicActive => activeWindowEnabled;
        public bool IsWeakWindowOpen => weakWindowEnabled;

        private void Awake()
        {
            if (defenseVisualLayer == null)
            {
                defenseVisualLayer = GetComponent<EnemyDefenseVisualLayerController>();
            }

            if (defenseController == null)
            {
                defenseController = GetComponent<EnemyDefenseController>();
            }
        }

        private void Start()
        {
            EnterIdle();
            ResetCycleDelay();
        }

        private void Update()
        {
            if (stateProfile == null)
            {
                return;
            }

            UpdateStateTimer();
            UpdateAutoCycle();
            UpdateWeakWindow();
            PushStateToVisualLayer();
        }

        private void UpdateStateTimer()
        {
            if (currentState == EnemyDefenseWindowState.None)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            if (stateTimer > 0f)
            {
                return;
            }

            switch (currentState)
            {
                case EnemyDefenseWindowState.Telegraph:
                    EnterActive();
                    break;

                case EnemyDefenseWindowState.Active:
                    EnterRecover();
                    break;

                case EnemyDefenseWindowState.Recover:
                    EnterIdle();
                    ResetCycleDelay();
                    break;
            }
        }

        private void UpdateAutoCycle()
        {
            if (stateProfile == null || !stateProfile.autoCycle)
            {
                return;
            }

            if (currentState != EnemyDefenseWindowState.None)
            {
                return;
            }

            nextCycleDelayTimer -= Time.deltaTime;
            if (nextCycleDelayTimer > 0f)
            {
                return;
            }

            if (Random.value <= stateProfile.startCycleChance)
            {
                EnterTelegraph();
            }
            else
            {
                ResetCycleDelay();
            }
        }

        private void UpdateWeakWindow()
        {
            weakWindowEnabled = false;

            if (stateProfile == null)
            {
                return;
            }

            if (!stateProfile.enableWeakWindowInsideActive)
            {
                return;
            }

            if (currentState != EnemyDefenseWindowState.Active)
            {
                return;
            }

            float duration = Mathf.Max(0.001f, stateProfile.activeDuration);
            float elapsed = duration - Mathf.Max(0f, stateTimer);
            float normalized = Mathf.Clamp01(elapsed / duration);

            weakWindowEnabled =
                normalized >= stateProfile.weakWindowNormalizedStart &&
                normalized <= stateProfile.weakWindowNormalizedEnd;
        }

        private void PushStateToVisualLayer()
        {
            if (defenseVisualLayer == null)
            {
                return;
            }

            defenseVisualLayer.SetDefenseWindowState(
                currentState,
                GetStateNormalized(),
                weakWindowEnabled
            );
        }

        private float GetStateNormalized()
        {
            if (stateProfile == null)
            {
                return 0f;
            }

            float duration = 0f;

            switch (currentState)
            {
                case EnemyDefenseWindowState.Telegraph:
                    duration = stateProfile.telegraphDuration;
                    break;
                case EnemyDefenseWindowState.Active:
                    duration = stateProfile.activeDuration;
                    break;
                case EnemyDefenseWindowState.Recover:
                    duration = stateProfile.recoverDuration;
                    break;
            }

            if (duration <= 0.001f)
            {
                return 1f;
            }

            float elapsed = duration - Mathf.Max(0f, stateTimer);
            return Mathf.Clamp01(elapsed / duration);
        }

        private void ResetCycleDelay()
        {
            if (stateProfile == null)
            {
                nextCycleDelayTimer = 1f;
                return;
            }

            float jitter = Random.Range(stateProfile.cycleJitter.x, stateProfile.cycleJitter.y);
            nextCycleDelayTimer = Mathf.Max(0.05f, stateProfile.idleCooldownBetweenCycles + jitter);
        }

        public void EnterTelegraph()
        {
            if (stateProfile == null)
            {
                return;
            }

            if (currentState == EnemyDefenseWindowState.Recover &&
                !stateProfile.allowTelegraphRestartDuringRecover)
            {
                return;
            }

            currentState = EnemyDefenseWindowState.Telegraph;
            stateTimer = Mathf.Max(0.01f, stateProfile.telegraphDuration);
            activeWindowEnabled = false;
            weakWindowEnabled = false;

            if (stateProfile.debugLog)
            {
                Debug.Log("[DefenseStateWindow] EnterTelegraph", this);
            }
        }

        public void EnterActive()
        {
            if (stateProfile == null)
            {
                return;
            }

            currentState = EnemyDefenseWindowState.Active;
            stateTimer = Mathf.Max(0.01f, stateProfile.activeDuration);
            activeWindowEnabled = stateProfile.activeWindowUsesDefenseLogic;
            weakWindowEnabled = false;

            if (stateProfile.debugLog)
            {
                Debug.Log("[DefenseStateWindow] EnterActive", this);
            }
        }

        public void EnterRecover()
        {
            if (stateProfile == null)
            {
                return;
            }

            currentState = EnemyDefenseWindowState.Recover;
            stateTimer = Mathf.Max(0.01f, stateProfile.recoverDuration);
            activeWindowEnabled = false;
            weakWindowEnabled = false;

            if (stateProfile.debugLog)
            {
                Debug.Log("[DefenseStateWindow] EnterRecover", this);
            }
        }

        public void EnterIdle()
        {
            currentState = EnemyDefenseWindowState.None;
            stateTimer = 0f;
            activeWindowEnabled = false;
            weakWindowEnabled = false;

            if (stateProfile != null && stateProfile.debugLog)
            {
                Debug.Log("[DefenseStateWindow] EnterIdle", this);
            }
        }

        public void ForceStartDefenseCycle()
        {
            EnterTelegraph();
        }

        public void ForceEndDefenseCycle()
        {
            EnterRecover();
        }

        public bool CanUseDefenseLogic()
        {
            return currentState == EnemyDefenseWindowState.Active && activeWindowEnabled;
        }

        public bool CanExposeWeakness()
        {
            if (currentState != EnemyDefenseWindowState.Active)
            {
                return false;
            }

            if (!weakWindowEnabled)
            {
                return false;
            }

            if (stateProfile == null)
            {
                return false;
            }

            float duration = Mathf.Max(0.001f, stateProfile.activeDuration);
            float elapsed = duration - Mathf.Max(0f, stateTimer);
            float normalized = Mathf.Clamp01(elapsed / duration);

            return normalized >= stateProfile.weakWindowNormalizedStart &&
                   normalized <= stateProfile.weakWindowNormalizedEnd;
        }
    }
}
