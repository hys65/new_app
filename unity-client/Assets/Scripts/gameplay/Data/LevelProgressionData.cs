using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "level_progression_data",
        menuName = "PowerPrank3D/Gameplay/Level Progression Data")]
    public class LevelProgressionData : ScriptableObject
    {
        [Header("Identity")]
        public string progressionId = "main_progression";
        public string displayName = "Main Progression";

        [Header("Levels")]
        public LevelEncounterConfigData[] levels;

        [Min(0)]
        public int startupLevelIndex = 0;

        public LevelEncounterConfigData GetLevelAt(int index)
        {
            if (levels == null || levels.Length == 0)
            {
                return null;
            }

            if (index < 0 || index >= levels.Length)
            {
                return null;
            }

            return levels[index];
        }

        public int GetSafeStartupLevelIndex()
        {
            if (levels == null || levels.Length == 0)
            {
                return -1;
            }

            return Mathf.Clamp(startupLevelIndex, 0, levels.Length - 1);
        }
    }
}
