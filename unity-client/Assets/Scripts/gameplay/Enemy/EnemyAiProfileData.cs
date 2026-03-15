using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "enemy_ai_profile",
        menuName = "PowerPrank3D/Enemy/Enemy AI Profile")]
    public class EnemyAiProfileData : ScriptableObject
    {
        [Header("Identity")]
        public string aiId = "enemy_ai_default";
        public string displayName = "Default Enemy AI";

        [Header("Observe")]
        [Min(1)]
        public int requiredHitSamples = 2;

        [Min(0.05f)]
        public float memoryDuration = 4.0f;

        [Min(0.05f)]
        public float minAcceptedHitInterval = 0.20f;

        public Vector2 predictedIntervalClamp = new Vector2(0.45f, 1.80f);

        [Range(0.05f, 1f)]
        public float intervalBlend = 0.50f;

        [Header("Defense Timing")]
        [Min(0.01f)]
        public float baseLeadTime = 0.22f;

        [Min(0f)]
        public float agitatedLeadBonus = 0.04f;

        [Min(0f)]
        public float furiousLeadBonus = 0.08f;

        [Min(0f)]
        public float meltdownLeadBonus = 0.12f;

        [Min(0.01f)]
        public float decisionCooldown = 0.18f;

        [Header("Threat")]
        [Range(0f, 1.5f)] public float calmThreat = 0.18f;
        [Range(0f, 1.5f)] public float annoyedThreat = 0.30f;
        [Range(0f, 1.5f)] public float agitatedThreat = 0.48f;
        [Range(0f, 1.5f)] public float furiousThreat = 0.70f;
        [Range(0f, 1.5f)] public float meltdownThreat = 0.85f;

        [Range(0f, 1f)]
        public float headHitThreatBonus = 0.12f;

        [Range(0f, 1f)]
        public float blockedConfidenceBonus = 0.10f;

        [Range(0f, 1f)]
        public float weaknessPenalty = 0.14f;

        [Range(0f, 1f)]
        public float breakPenalty = 0.30f;

        [Range(0f, 1.5f)]
        public float defenseTriggerThreshold = 0.55f;

        [Header("Break / Recover")]
        [Min(0.05f)]
        public float brokenLockDuration = 0.90f;

        [Min(0.05f)]
        public float rearmDelayAfterRecover = 0.35f;

        [Header("Debug")]
        public bool debugLog;
    }
}
