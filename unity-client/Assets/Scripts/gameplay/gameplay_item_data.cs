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

        [Header("Projectile")]
        public GameObject projectilePrefab;

        [Header("Core Values")]
        public int baseBreakdownScore = 10;
        public float throwForce = 12f;
        public HitFeedbackType feedbackType = HitFeedbackType.ScalePunch;

        [Header("Impact VFX")]
        public GameObject impactVfxPrefab;
    }
}
