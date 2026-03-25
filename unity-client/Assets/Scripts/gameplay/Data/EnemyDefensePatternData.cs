using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public enum EnemyDefensePatternType
    {
        None = 0,
        BriefcaseBlock = 1,
        FaceGuard = 2
    }

    [CreateAssetMenu(
        fileName = "enemy_defense_pattern_data",
        menuName = "PowerPrank3D/Enemy/Enemy Defense Pattern Data")]
    public class EnemyDefensePatternData : ScriptableObject
    {
        [Header("Identity")]
        public string defenseId = "defense_none";
        public string displayName = "No Defense";
        public EnemyDefensePatternType patternType = EnemyDefensePatternType.None;

        [Header("Core")]
        public bool canBlockBody = true;
        public bool canBlockHead = false;
        [Range(0f, 1f)] public float randomBlockChance = 0.25f;
        public float blockDuration = 1.0f;
        public float blockCooldown = 2.0f;

        [Header("Reactive Trigger")]
        public bool triggeredByRepeatedHits = true;
        public int repeatedHitCountThreshold = 2;
        public float repeatedHitWindow = 2.0f;

        [Header("Timed Trigger")]
        public bool useTimedActivation = false;
        public float firstActivationDelay = 4.0f;
        public float timedActivationInterval = 6.0f;

        [Header("Blocked Result")]
        [Range(0f, 1f)] public float blockedBreakdownMultiplier = 0.25f;
        [Range(0f, 1f)] public float blockedReactionMultiplier = 0.55f;

        [Header("Passive Hit Shaping")]
        public bool reduceHeadHitsOutsideDefense = false;
        [Range(0f, 1f)] public float passiveHeadBreakdownMultiplier = 0.25f;
        [Range(0f, 1f)] public float passiveHeadReactionMultiplier = 0.9f;
        public string passiveHeadPopupText = string.Empty;

        [Header("Weakness")]
        public bool weakToHeadHits = false;
        [Range(1f, 3f)] public float headWeaknessMultiplier = 1.5f;

        [Header("Break Defense By Item")]
        public bool breakByHammer = true;
        public bool breakByFoam = false;
        public bool breakByPaint = false;
        public bool breakByEgg = false;
        public bool breakByTomato = false;

        [Header("Optional Flavor")]
        public bool autoActivateOnHeadHit = false;
        public bool autoActivateOnBodyHit = false;
        public bool reducePaintOnFaceGuard = true;
    }
}
