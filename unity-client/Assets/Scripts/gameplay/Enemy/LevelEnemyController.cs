using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class LevelEnemyController : MonoBehaviour
    {
        [Header("Level Config")]
        public EnemyLevelConfig levelConfig;

        [Header("References")]
        public EnemyPresetApplicator presetApplicator;

        void Start()
        {
            if (levelConfig == null)
                return;

            if (presetApplicator == null)
                return;

            if (levelConfig.enemyPreset == null)
                return;

            presetApplicator.preset = levelConfig.enemyPreset;
            presetApplicator.ApplyPreset();
        }
    }
}
