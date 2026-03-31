using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "enemy_preset_data",
        menuName = "PowerPrank3D/Enemy/Enemy Preset Data")]
    public class EnemyPresetData : ScriptableObject
    {
        [Header("Identity")]
        public string presetId = "enemy_preset_default";
        public string displayName = "Default Enemy Preset";

        [Header("Core References")]
        public EnemyArchetypeData archetype;
        public EnemyDefensePatternData defensePattern;
        public EnemyAiProfileData aiProfile;
        public EnemyDefenseStateWindowProfileData defenseStateWindowProfile;
        public EnemyDefenseVisualProfileData defenseVisualProfile;
    }
}
