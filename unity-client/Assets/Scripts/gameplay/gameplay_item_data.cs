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

        [Header("Impact SFX")]
        public AudioClip impactSfx;
        [Range(0f, 1f)] public float impactVolume = 1f;
        public Vector2 impactPitchRange = new Vector2(0.95f, 1.05f);

        [Header("Impact Stain")]
        public GameObject impactStainPrefab;
        [Range(0.05f, 1.5f)] public float impactStainScale = 0.35f;
        public bool spawnStainOnEnemy = true;
        public bool spawnStainOnGround = false;
    }
}
