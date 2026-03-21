using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(
        fileName = "level_encounter_config_data",
        menuName = "PowerPrank3D/Gameplay/Level Encounter Config Data")]
    public class LevelEncounterConfigData : ScriptableObject
    {
        [Header("Identity")]
        public string levelId = "level_01";
        public string displayName = "Level 01 Encounter";

        [Header("Enemy")]
        public LevelEnemySelectionData enemySelection;

        [Header("Goal")]
        public LevelGoalDefinition primaryGoal = new LevelGoalDefinition
        {
            goalType = LevelGoalType.BreakdownTarget,
            targetCount = 100
        };

        [Header("Round")]
        [Min(1)]
        public int targetBreakdownValue = 100;

        [Min(1f)]
        public float roundDurationSeconds = 45f;

        [Tooltip("If true, controller will start the round automatically after applying encounter config.")]
        public bool autoStartRound = true;
    }
}
