using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [CreateAssetMenu(fileName = "enemy_defense_visual_profile", menuName = "PowerPrank3D/Enemy/Defense Visual Profile")]
    public class EnemyDefenseVisualProfileData : ScriptableObject
    {
        [Header("Guard Pose")]
        public Vector3 guardBodyRotation = new Vector3(0f, 12f, -8f);
        public Vector3 guardBodyPosition = new Vector3(0f, 0f, -0.08f);
        public Vector3 guardHeadRotation = new Vector3(8f, -10f, 0f);
        public float guardBlendIn = 10f;
        public float guardBlendOut = 8f;
        public float guardHoldTime = 0.45f;

        [Header("Block Impulse")]
        public Vector3 blockBodyKickRotation = new Vector3(-10f, 16f, 0f);
        public Vector3 blockBodyKickPosition = new Vector3(0f, 0.02f, -0.10f);
        public Vector3 blockHeadKickRotation = new Vector3(6f, 8f, 0f);
        public float blockRecoverSpeed = 14f;
        public float blockImpactStrength = 1f;

        [Header("Break Pose")]
        public Vector3 breakBodyRotation = new Vector3(16f, -18f, 10f);
        public Vector3 breakBodyPosition = new Vector3(0f, 0.03f, -0.12f);
        public Vector3 breakHeadRotation = new Vector3(-12f, 18f, 0f);
        public float breakHoldTime = 0.55f;
        public float breakRecoverSpeed = 6f;
        public float breakNoiseAmplitude = 2.5f;
        public float breakNoiseSpeed = 10f;

        [Header("Weak Pose")]
        public Vector3 weakBodyRotation = new Vector3(-3f, 6f, 0f);
        public Vector3 weakHeadRotation = new Vector3(-10f, 12f, 0f);
        public float weakHoldTime = 0.35f;
        public float weakRecoverSpeed = 10f;

        [Header("Global Multipliers")]
        public float bodyMotionMultiplier = 1f;
        public float headMotionMultiplier = 1f;

        [Header("State Window Pose")]
        public Vector3 telegraphBodyRotation = new Vector3(0f, 8f, -4f);
        public Vector3 telegraphBodyPosition = new Vector3(0f, 0f, -0.03f);
        public Vector3 telegraphHeadRotation = new Vector3(6f, -4f, 0f);

        public Vector3 activeBodyRotation = new Vector3(2f, 16f, -8f);
        public Vector3 activeBodyPosition = new Vector3(0f, 0f, -0.08f);
        public Vector3 activeHeadRotation = new Vector3(10f, -10f, 0f);

        public Vector3 recoverBodyRotation = new Vector3(-3f, 4f, 2f);
        public Vector3 recoverBodyPosition = new Vector3(0f, 0f, -0.02f);
        public Vector3 recoverHeadRotation = new Vector3(-4f, 3f, 0f);

        public float stateWindowBlendSpeed = 8f;

        [Header("Optional Left Arm Guard Pose")]
        public bool useLeftArmGuardPose = false;

        [Tooltip("防御时左手臂目标 Local Position。基于被驱动的 Transform 本地坐标。")]
        public Vector3 leftArmGuardLocalPosition = Vector3.zero;

        [Tooltip("防御时左手臂目标 Local Rotation（Euler）。基于被驱动的 Transform 本地旋转。")]
        public Vector3 leftArmGuardLocalRotation = Vector3.zero;

        [Tooltip("防御时左手臂目标 Local Scale。通常保持默认即可；只有你确实需要时才改。")]
        public Vector3 leftArmGuardLocalScale = Vector3.one;

        [Tooltip("进入护脸姿态的速度。")]
        public float leftArmGuardBlendIn = 10f;

        [Tooltip("退出护脸姿态、回到默认姿态的速度。")]
        public float leftArmGuardBlendOut = 8f;
    }
}
