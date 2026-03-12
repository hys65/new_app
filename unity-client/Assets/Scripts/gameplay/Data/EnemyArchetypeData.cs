using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "enemy_archetype_data",
        menuName = "PowerPrank3D/Enemy/Enemy Archetype Data")]
    public class EnemyArchetypeData : ScriptableObject
    {
        [Header("Identity")]
        public string archetypeId = "meeting_tyrant";
        public string displayName = "Meeting Tyrant";

        [Header("Idle Style")]
        [Range(0f, 3f)] public float idleBodySwayMultiplier = 1f;
        [Range(0f, 3f)] public float idleBodyLeanMultiplier = 1f;
        [Range(0f, 3f)] public float idleHeadMotionMultiplier = 1f;
        [Range(0f, 3f)] public float idleAgitationMultiplier = 1f;

        [Header("Hit Style")]
        [Range(0f, 3f)] public float bodyHitMultiplier = 1f;
        [Range(0f, 3f)] public float headHitMultiplier = 1f;
        [Range(0f, 3f)] public float knockbackMultiplier = 1f;
        [Range(0f, 3f)] public float hitFreezeMultiplier = 1f;
        [Range(0f, 3f)] public float staggerMultiplier = 1f;

        [Header("Stage Behavior")]
        [Range(0f, 3f)] public float calmMultiplier = 1f;
        [Range(0f, 3f)] public float annoyedMultiplier = 1f;
        [Range(0f, 3f)] public float agitatedMultiplier = 1f;
        [Range(0f, 3f)] public float furiousMultiplier = 1f;
        [Range(0f, 3f)] public float meltdownMultiplier = 1f;

        [Header("Stage Entry Action")]
        [Range(0f, 3f)] public float stageEntryBodyMultiplier = 1f;
        [Range(0f, 3f)] public float stageEntryHeadMultiplier = 1f;
        [Range(0f, 3f)] public float stageEntryShakeMultiplier = 1f;

        [Header("Special Flavor")]
        [Range(0f, 3f)] public float vanityHeadRecoverMultiplier = 1f;
        [Range(0f, 3f)] public float intimidationChestForwardMultiplier = 1f;
        [Range(0f, 3f)] public float panicShakeMultiplier = 1f;

        public float GetStageMultiplier(float breakdown)
        {
            if (breakdown >= 99f) return meltdownMultiplier;
            if (breakdown >= 90f) return furiousMultiplier;
            if (breakdown >= 60f) return agitatedMultiplier;
            if (breakdown >= 30f) return annoyedMultiplier;
            return calmMultiplier;
        }
    }
}
