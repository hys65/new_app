using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "enemy_level_config",
        menuName = "PowerPrank3D/Enemy/Enemy Level Config")]
    public class EnemyLevelConfig : ScriptableObject
    {
        [Header("Enemy Preset")]
        public EnemyPresetData enemyPreset;

        [Header("Difficulty")]
        [Range(0.5f, 2f)]
        public float breakdownMultiplier = 1f;

        [Range(0.5f, 2f)]
        public float reactionMultiplier = 1f;
    }
}
