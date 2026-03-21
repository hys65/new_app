using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [System.Serializable]
    public class LevelGoalDefinition
    {
        public LevelGoalType goalType = LevelGoalType.BreakdownTarget;

        [Min(1)]
        public int targetCount = 100;

        [Tooltip("Only used by SpecificItemHitCount")]
        public string requiredItemId = "item_egg";
    }
}
