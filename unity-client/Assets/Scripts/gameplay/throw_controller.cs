using UnityEngine;
using UnityEngine.EventSystems;

namespace PowerPrank3D.Gameplay
{
    public class ThrowController : MonoBehaviour
    {
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private Transform throwSpawnPoint;

        [Header("Aim")]
        [SerializeField] private float upwardFactor = 0.25f;

        private Vector2 dragStartPos;
        private bool isDragging;

        private void Update()
        {
            if (gameplayManager == null || !gameplayManager.IsRoundRunning)
            {
                return;
            }

            HandleItemShortcuts();

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUi())
                {
                    isDragging = false;
                    return;
                }

                dragStartPos = Input.mousePosition;
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                var dragEndPos = (Vector2)Input.mousePosition;
                var dragDirection = dragEndPos - dragStartPos;

                var throwDirection = BuildThrowDirection(dragDirection);
                SpawnAndThrow(throwDirection);
                isDragging = false;
            }
        }

        private void HandleItemShortcuts()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) gameplayManager.SelectItemByIndex(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) gameplayManager.SelectItemByIndex(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) gameplayManager.SelectItemByIndex(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) gameplayManager.SelectItemByIndex(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) gameplayManager.SelectItemByIndex(4);
        }

        private bool IsPointerOverUi()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            if (Input.touchCount > 0)
            {
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }

            return EventSystem.current.IsPointerOverGameObject();
        }

        private Vector3 BuildThrowDirection(Vector2 dragDirection)
        {
            if (gameplayCamera == null)
            {
                return (transform.forward + Vector3.up * upwardFactor).normalized;
            }

            if (dragDirection.sqrMagnitude < 4f)
            {
                return (gameplayCamera.transform.forward + Vector3.up * upwardFactor).normalized;
            }

            var screenDir = new Vector3(dragDirection.x, dragDirection.y, gameplayCamera.pixelHeight * 0.2f);
            var worldDir = gameplayCamera.transform.TransformDirection(screenDir.normalized);
            worldDir.y = Mathf.Abs(worldDir.y) + upwardFactor;
            return worldDir.normalized;
        }

        private void SpawnAndThrow(Vector3 throwDirection)
        {
            var itemData = gameplayManager.CurrentItem;
            if (itemData == null || itemData.projectilePrefab == null || throwSpawnPoint == null)
            {
                return;
            }

            var spawned = Instantiate(itemData.projectilePrefab, throwSpawnPoint.position, Quaternion.identity);
            var projectile = spawned.GetComponent<ProjectileBehavior>();
            if (projectile == null)
            {
                projectile = spawned.AddComponent<ProjectileBehavior>();
            }

            projectile.Initialize(itemData, gameplayManager);

            if (spawned.TryGetComponent<Rigidbody>(out var body))
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                body.AddForce(throwDirection * itemData.throwForce, ForceMode.VelocityChange);
            }
        }
    }
}
