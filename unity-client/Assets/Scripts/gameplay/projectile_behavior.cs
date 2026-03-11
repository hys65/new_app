using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ProjectileBehavior : MonoBehaviour
    {
        [SerializeField] private float autoDestroySeconds = 5f;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (hasHit)
            {
                return;
            }

            hasHit = true;

            var receiver = collision.collider.GetComponentInParent<EnemyHitReaction>();
            if (receiver != null)
            {
                receiver.PlayHitFeedback(HitFeedbackType.ScalePunch);

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
            }

            Destroy(gameObject);
        }
    }
}
