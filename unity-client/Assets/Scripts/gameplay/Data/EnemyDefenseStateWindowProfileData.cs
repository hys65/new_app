using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(fileName = "enemy_defense_state_window_profile", menuName = "PowerPrank3D/Enemy/Defense State Window Profile")]
    public class EnemyDefenseStateWindowProfileData : ScriptableObject
    {
        [Header("Timing")]
        public float telegraphDuration = 0.28f;
        public float activeDuration = 0.60f;
        public float recoverDuration = 0.35f;

        [Header("Auto Loop")]
        public bool autoCycle = true;
        public float idleCooldownBetweenCycles = 1.25f;
        public Vector2 cycleJitter = new Vector2(0.0f, 0.35f);

        [Header("Opening Rules")]
        [Range(0f, 1f)] public float startCycleChance = 1f;
        public bool allowTelegraphRestartDuringRecover = false;

        [Header("Active Window Behavior")]
        public bool activeWindowUsesDefenseLogic = true;

        [Header("Weak Window")]
        public bool enableWeakWindowInsideActive = true;
        [Range(0f, 1f)] public float weakWindowNormalizedStart = 0.55f;
        [Range(0f, 1f)] public float weakWindowNormalizedEnd = 0.85f;

        [Header("Debug")]
        public bool debugLog;
    }
}
