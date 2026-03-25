using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ProjectileBehavior : MonoBehaviour
    {
        [SerializeField] private float autoDestroySeconds = 20f;

        private GameplayItemData itemData;
        private GameplayManager gameplayManager;
        private HitPopupSpawner popupSpawner;
        private LevelGoalController levelGoalController;
        private Rigidbody rb;
        private bool hasHit;

        public void Initialize(GameplayItemData data, GameplayManager manager)
        {
            itemData = data;
            gameplayManager = manager;

            if (popupSpawner == null)
            {
                popupSpawner = FindFirstObjectByType<HitPopupSpawner>();
            }

            if (levelGoalController == null)
            {
                levelGoalController = FindFirstObjectByType<LevelGoalController>();
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            if (popupSpawner == null)
            {
                popupSpawner = FindFirstObjectByType<HitPopupSpawner>();
            }

            if (levelGoalController == null)
            {
                levelGoalController = FindFirstObjectByType<LevelGoalController>();
            }
        }

        private void Start()
        {
            Destroy(gameObject, autoDestroySeconds);
        }

        private void Update()
        {
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

            var receiver = collision.collider.GetComponentInParent<EnemyHitReaction>();
            bool hitEnemy = receiver != null;
            bool hitGround = collision.collider.CompareTag("Ground");

            if (hitEnemy)
            {
                hasHit = true;

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

                Debug.Log(
                    "[HeadCheck] collider=" + collision.collider.name +
                    " | tag=" + collision.collider.tag +
                    " | isHeadHit=" + isHeadHit
                );

                int scoreUnits = isHeadHit ? 2 : 1;

                EnemyDefenseController defenseController =
                    collision.collider.GetComponentInParent<EnemyDefenseController>();

                DefenseHitResult defenseResult = DefenseHitResult.Default();

                if (defenseController != null)
                {
                    defenseResult = defenseController.EvaluateHit(itemData, isHeadHit);
                }

                int finalUnits = scoreUnits;

                if (defenseResult.wasBlocked)
                {
                    finalUnits = 0;
                }
                else if (defenseResult.weaknessApplied)
                {
                    finalUnits = Mathf.Max(1, Mathf.RoundToInt(scoreUnits * defenseResult.breakdownMultiplier));
                }

                int gainedScore = 0;

                if (gameplayManager != null && finalUnits > 0)
                {
                    gainedScore = gameplayManager.AddBreakdown(itemData, finalUnits);
                }

                if (levelGoalController != null)
                {
                    levelGoalController.NotifyHitResolved(new CombatHitInfo
                    {
                        isHeadHit = isHeadHit,
                        wasBlocked = defenseResult.wasBlocked,
                        itemId = itemData != null ? itemData.itemId : string.Empty,
                        gainedScore = gainedScore
                    });
                }

                EnemyReactionLayerController reactionLayer =
                    collision.collider.GetComponentInParent<EnemyReactionLayerController>();

                EnemyDefenseVisualLayerController defenseVisual =
                    collision.collider.GetComponentInParent<EnemyDefenseVisualLayerController>();

                Vector3 incomingDirection = Vector3.zero;

                if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
                {
                    incomingDirection = rb.linearVelocity.normalized;
                }

                if (reactionLayer != null)
                {
                    reactionLayer.ReactToHit(itemData, isHeadHit, incomingDirection, defenseResult.reactionMultiplier);
                }

                if (defenseVisual != null)
                {
                    defenseVisual.ApplyDefenseVisual(
                        defenseResult.popupText,
                        isHeadHit,
                        defenseResult.reactionMultiplier
                    );
                }

                if (popupSpawner != null)
                {
                    Vector3 hitPoint = collision.contacts.Length > 0
                        ? collision.contacts[0].point
                        : collision.collider.bounds.center;

                    if (!string.IsNullOrEmpty(defenseResult.popupText))
                    {
                        popupSpawner.SpawnPopup(hitPoint, defenseResult.popupText);
                    }
                    else if (gainedScore > 0)
                    {
                        popupSpawner.SpawnPopup(hitPoint, "+" + gainedScore);
                    }
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
                    SpawnImpactStain(contact.point, contact.normal, null, false, true);
                }

                Destroy(gameObject);
                return;
            }
        }

        private void SpawnImpactVfx(Vector3 position, Vector3 normal)
        {
            if (itemData == null || itemData.impactVfxPrefab == null)
            {
                return;
            }

            Quaternion rotation = Quaternion.LookRotation(-normal);
            rotation *= Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

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
            if (itemData == null || itemData.impactSfx == null)
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
            if (itemData == null || itemData.impactStainPrefab == null)
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

            Vector3 spawnPos = position + normal * 0.02f;
            Quaternion rotation = Quaternion.LookRotation(-normal);
            rotation *= Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            Transform parentToUse = null;

            if (isEnemyHit && hitParent != null)
            {
                parentToUse = hitParent;
            }
            else
            {
                parentToUse = GameObject.Find("Stains")?.transform;
            }

            GameObject stain = Instantiate(itemData.impactStainPrefab, spawnPos, rotation, parentToUse);

            float stainScale = Mathf.Max(0.01f, itemData.impactStainScale);
            stain.transform.localScale = Vector3.one * stainScale;

            Collider stainCollider = stain.GetComponent<Collider>();
            if (stainCollider != null)
            {
                stainCollider.enabled = false;
            }

            Rigidbody stainRb = stain.GetComponent<Rigidbody>();
            if (stainRb != null)
            {
                stainRb.linearVelocity = Vector3.zero;
                stainRb.angularVelocity = Vector3.zero;
                stainRb.useGravity = false;
                stainRb.isKinematic = true;
            }
        }
    }
}
