using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ProjectileBehavior : MonoBehaviour
    {
        [SerializeField] private float autoDestroySeconds = 20f;

        private GameplayItemData itemData;
        private GameplayManager gameplayManager;
        private HitPopupSpawner popupSpawner;
        private bool hasHit;

        public void Initialize(GameplayItemData data, GameplayManager manager)
        {
            itemData = data;
            gameplayManager = manager;

            if (popupSpawner == null)
            {
                popupSpawner = FindFirstObjectByType<HitPopupSpawner>();
            }
        }

        private void Awake()
        {
            if (popupSpawner == null)
            {
                popupSpawner = FindFirstObjectByType<HitPopupSpawner>();
            }
        }

        private void Start()
        {
            Destroy(gameObject, autoDestroySeconds);
        }

        private void Update()
        {
            // 掉出世界底部再强制清理
            if (transform.position.y < -10f)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (hasHit)
            {
                return;
            }

            Debug.Log(
                "Projectile hit -> name: " + collision.collider.name +
                " | tag: " + collision.collider.tag +
                " | layer: " + LayerMask.LayerToName(collision.collider.gameObject.layer)
            );

            var receiver = collision.collider.GetComponentInParent<EnemyHitReaction>();
            bool hitEnemy = receiver != null;
            bool hitGround = collision.collider.CompareTag("Ground");

            if (hitEnemy)
            {
                hasHit = true;

                // ★新增：命中粒子
                if (collision.contacts.Length > 0)
                {
                    ContactPoint contact = collision.contacts[0];
                    SpawnImpactVfx(contact.point, contact.normal);
                    PlayImpactSfx(contact.point);
                    SpawnImpactStain(contact.point, contact.normal, receiver.transform, true, false);
                    CameraShake.Shake(0.12f, 0.06f);
                }

                receiver.PlayHitFeedback(itemData != null ? itemData.feedbackType : HitFeedbackType.ScalePunch);

                bool isHeadHit = collision.collider.CompareTag("Head");
                int scoreUnits = isHeadHit ? 2 : 1;

                int gainedScore = gameplayManager != null
                    ? gameplayManager.AddBreakdown(itemData, scoreUnits)
                    : 0;

                if (popupSpawner != null && gainedScore > 0)
                {
                    Vector3 hitPoint = collision.contacts.Length > 0
                        ? collision.contacts[0].point
                        : collision.collider.bounds.center;

                    popupSpawner.SpawnPopup(hitPoint, "+" + gainedScore);
                }

                Destroy(gameObject);
                return;
            }

            if (hitGround)
            {
                hasHit = true;

                if (collision.contacts.Length > 0)
                {
                    ContactPoint contact = collision.contacts[0];
                    SpawnImpactVfx(contact.point, contact.normal);
                    PlayImpactSfx(contact.point);
                }

                Destroy(gameObject);
                return;
            }

            // 非敌人、非地面：先不销毁，方便继续观察问题
        }
        private void SpawnImpactVfx(Vector3 position, Vector3 normal)
        {
            if (itemData == null)
            {
                return;
            }

            if (itemData.impactVfxPrefab == null)
            {
                return;
            }

            Quaternion rotation = Quaternion.LookRotation(normal);
            GameObject vfx = Instantiate(itemData.impactVfxPrefab, position + normal * 0.02f, rotation);

            ParticleSystem[] particleSystems = vfx.GetComponentsInChildren<ParticleSystem>();
            float maxDuration = 0f;

            foreach (ParticleSystem ps in particleSystems)
            {
                var main = ps.main;
                float duration = main.duration;

                if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
                {
                    duration += main.startLifetime.constantMax;
                }
                else if (main.startLifetime.mode == ParticleSystemCurveMode.Constant)
                {
                    duration += main.startLifetime.constant;
                }
                else
                {
                    duration += 2f;
                }

                if (duration > maxDuration)
                {
                    maxDuration = duration;
                }
            }

            if (maxDuration <= 0f)
            {
                maxDuration = 2f;
            }

            Destroy(vfx, maxDuration + 0.5f);
        }

        private void PlayImpactSfx(Vector3 position)
        {
            if (itemData == null)
            {
                return;
            }

            if (itemData.impactSfx == null)
            {
                return;
            }

            ImpactSfxPlayer.PlayAtPoint(
                itemData.impactSfx,
                position,
                itemData.impactVolume,
                itemData.impactPitchRange
            );
        }

        private void SpawnImpactStain(Vector3 position, Vector3 normal, Transform hitParent, bool isEnemyHit, bool isGroundHit)
        {
            if (itemData == null)
            {
                return;
            }

            if (itemData.impactStainPrefab == null)
            {
                return;
            }

            if (isEnemyHit && !itemData.spawnStainOnEnemy)
            {
                return;
            }

            if (isGroundHit && !itemData.spawnStainOnGround)
            {
                return;
            }

            Transform stainsRoot = GameObject.Find("Stains")?.transform;

            Vector3 spawnPos = position + normal * 0.06f;
            Quaternion rotation = Quaternion.LookRotation(-normal);

            Transform parentToUse = stainsRoot != null ? stainsRoot : null;

            GameObject stain = Instantiate(itemData.impactStainPrefab, spawnPos, rotation, parentToUse);

            float stainScale = Mathf.Max(0.01f, itemData.impactStainScale);
            stain.transform.localScale = Vector3.one * stainScale;

            Collider stainCollider = stain.GetComponent<Collider>();
            if (stainCollider != null)
            {
                stainCollider.enabled = false;
            }
        }
    }
}
