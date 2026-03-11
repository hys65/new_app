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
                Destroy(gameObject);
                return;
            }

            // 非敌人、非地面：先不销毁，方便继续观察问题
        }
    }
}
