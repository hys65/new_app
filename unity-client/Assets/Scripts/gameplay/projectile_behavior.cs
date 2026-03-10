using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ProjectileBehavior : MonoBehaviour
    {
        [SerializeField] private float autoDestroySeconds = 5f;

        private GameplayItemData itemData;
        private GameplayManager gameplayManager;
        private bool hasHit;

        public void Initialize(GameplayItemData data, GameplayManager manager)
        {
            itemData = data;
            gameplayManager = manager;
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
                    gameplayManager.AddBreakdown(itemData);
                }
            }

            Destroy(gameObject);
        }
    }
}
