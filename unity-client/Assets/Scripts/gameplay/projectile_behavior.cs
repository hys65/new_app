using UnityEngine;
using Object = UnityEngine.Object;

namespace PowerPrank3D.Gameplay
{
    public class ProjectileBehavior : MonoBehaviour
    {
        [SerializeField] private float autoDestroySeconds = 5f;

        private GameplayItemData itemData;
        private GameplayManager gameplayManager;
        private bool hasHit;
        private HitPopupSpawner hitPopupSpawner;

        public void Initialize(GameplayItemData data, GameplayManager manager)
        {
            itemData = data;
            gameplayManager = manager;
            hitPopupSpawner = Object.FindFirstObjectByType<HitPopupSpawner>();
        }

        private void Start()
        {
            Destroy(gameObject, autoDestroySeconds);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleHit(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleHit(other);
        }

        private void HandleHit(Collider hitCollider)
        {
            if (hasHit || hitCollider == null)
            {
                return;
            }

            hasHit = true;

            var receiver = hitCollider.GetComponentInParent<EnemyHitReaction>();
            if (receiver != null)
            {
                receiver.PlayHitFeedback(itemData != null ? itemData.feedbackType : HitFeedbackType.ScalePunch);

                if (gameplayManager != null && gameplayManager.IsRoundRunning && itemData != null)
                {
                    bool isHeadshot = hitCollider.CompareTag("Head");
                    int popupScore = itemData.baseBreakdownScore;

                    gameplayManager.AddBreakdown(itemData);

                    if (isHeadshot)
                    {
                        gameplayManager.AddBreakdown(itemData);
                        popupScore *= 2;
                    }

                    if (hitPopupSpawner != null)
                    {
                        Vector3 popupWorldPosition = hitCollider.bounds.center + Vector3.up * 0.5f;
                        hitPopupSpawner.SpawnPopup(popupWorldPosition, "+" + popupScore);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}