using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public enum HitFeedbackType
    {
        ScalePunch,
        FlashColor,
        SmallKnockback,
        FoamTint,
        Wiggle
    }

    [CreateAssetMenu(fileName = "gameplay_item_data", menuName = "PowerPrank3D/Gameplay Item Data")]
    public class GameplayItemData : ScriptableObject
    {
        [Header("Identity")]
        public string itemId = "item_egg";
        public string displayKey = "item_egg_name";

        [Header("Core Values")]
        public int baseBreakdownScore = 10;
        public HitFeedbackType feedbackType = HitFeedbackType.ScalePunch;

        [Header("Throw")]
        public GameObject projectilePrefab;
        public float throwForce = 14f;
    }
}
