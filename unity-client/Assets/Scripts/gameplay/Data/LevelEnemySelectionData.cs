using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [System.Serializable]
    public class LevelEnemySelectionEntry
    {
        public string rosterEntryId = "meeting_tyrant";

        [Tooltip("If true, use the roster entry's recommended slot id.")]
        public bool useRosterRecommendedSlot = true;

        [Tooltip("Used only when useRosterRecommendedSlot is false.")]
        public string overrideSlotId = "enemy_slot_01";
    }

    [CreateAssetMenu(
        fileName = "level_enemy_selection_data",
        menuName = "PowerPrank3D/Enemy/Level Enemy Selection Data")]
    public class LevelEnemySelectionData : ScriptableObject
    {
        [Header("Identity")]
        public string levelId = "level_01";
        public string displayName = "Level 01 Enemy Selection";

        [Header("Roster Source")]
        public EnemyRosterData roster;

        [Header("Level Selection")]
        public LevelEnemySelectionEntry[] selectedEnemies;

        [Tooltip("Index inside selectedEnemies, not slot index.")]
        public int startupSelectionIndex = 0;

        [Header("Startup Apply Rules")]
        public bool clearUnassignedSlotPresets = true;
        public bool autoApplyDefaultPresetOnStart = true;
    }
}
