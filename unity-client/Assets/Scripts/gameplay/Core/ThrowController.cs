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
        [SerializeField] private float minDragDistance = 40f;
        [SerializeField] private float maxDragDistance = 300f;

        [Header("Throw Force Multiplier")]
        [SerializeField] private float minForceMultiplier = 0.6f;
        [SerializeField] private float maxForceMultiplier = 1.4f;

        [Header("Aim Preview")]
        [SerializeField] private LineRenderer aimLine;
        [SerializeField] private float aimLineMinLength = 0.8f;
        [SerializeField] private float aimLineMaxLength = 2.5f;

        [Header("Trajectory Preview")]
        [SerializeField] private LineRenderer trajectoryLine;
        [SerializeField] private int trajectoryPointCount = 20;
        [SerializeField] private float trajectoryTimeStep = 0.08f;

        private Vector2 dragStartPos;
        private bool isDragging;

        private void Awake()
        {
            SetAimLineVisible(false);
            SetTrajectoryVisible(false);
        }

        private void Update()
        {
            if (gameplayManager == null || !gameplayManager.IsRoundRunning)
            {
                SetAimLineVisible(false);
                SetTrajectoryVisible(false);
                return;
            }

            HandleItemShortcuts();
            HandleThrowInput();
        }

        private void HandleThrowInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUi())
                {
                    isDragging = false;
                    SetAimLineVisible(false);
                    SetTrajectoryVisible(false);
                    return;
                }

                dragStartPos = Input.mousePosition;
                isDragging = true;
                UpdateAimPreview(Vector2.zero);
                UpdateTrajectoryPreview(Vector2.zero);
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 currentPos = Input.mousePosition;
                Vector2 dragDelta = currentPos - dragStartPos;

                UpdateAimPreview(dragDelta);
                UpdateTrajectoryPreview(dragDelta);
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                Vector2 dragEndPos = Input.mousePosition;
                Vector2 dragDelta = dragEndPos - dragStartPos;
                float dragDistance = dragDelta.magnitude;

                isDragging = false;
                SetAimLineVisible(false);
                SetTrajectoryVisible(false);

                if (dragDistance < minDragDistance)
                {
                    return;
                }

                Vector3 throwDirection = BuildThrowDirection(dragDelta);
                float forceMultiplier = BuildForceMultiplier(dragDistance);

                SpawnAndThrow(throwDirection, forceMultiplier);
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

        private Vector3 BuildThrowDirection(Vector2 dragDelta)
        {
            if (gameplayCamera == null)
            {
                return (transform.forward + Vector3.up * upwardFactor).normalized;
            }

            Vector3 screenDir = new Vector3(
                -dragDelta.x,
                -dragDelta.y,
                gameplayCamera.pixelHeight * 0.35f
            );

            Vector3 worldDir = gameplayCamera.transform.TransformDirection(screenDir.normalized);
            worldDir.y = Mathf.Abs(worldDir.y) + upwardFactor;

            return worldDir.normalized;
        }

        private float BuildForceMultiplier(float dragDistance)
        {
            float normalized = Mathf.InverseLerp(minDragDistance, maxDragDistance, dragDistance);
            return Mathf.Lerp(minForceMultiplier, maxForceMultiplier, normalized);
        }

        private void SpawnAndThrow(Vector3 throwDirection, float forceMultiplier)
        {
            GameplayItemData itemData = gameplayManager.CurrentItem;
            if (itemData == null || itemData.projectilePrefab == null || throwSpawnPoint == null)
            {
                return;
            }

            GameObject spawned = Instantiate(
                itemData.projectilePrefab,
                throwSpawnPoint.position,
                Quaternion.identity
            );

            ProjectileBehavior projectile = spawned.GetComponent<ProjectileBehavior>();
            if (projectile == null)
            {
                projectile = spawned.AddComponent<ProjectileBehavior>();
            }

            projectile.Initialize(itemData, gameplayManager);

            if (spawned.TryGetComponent<Rigidbody>(out var body))
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;

                float finalForce = itemData.throwForce * forceMultiplier;
                body.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }
        }

        private void UpdateAimPreview(Vector2 dragDelta)
        {
            if (aimLine == null || throwSpawnPoint == null || gameplayCamera == null)
            {
                return;
            }

            Vector3 start = gameplayCamera.transform.position
                            + gameplayCamera.transform.forward * 0.8f
                            + gameplayCamera.transform.up * -0.15f;

            float dragDistance = dragDelta.magnitude;

            if (dragDistance < 1f)
            {
                SetAimLineVisible(true);
                aimLine.SetPosition(0, start);
                aimLine.SetPosition(1, start + gameplayCamera.transform.forward * aimLineMinLength);
                return;
            }

            Vector3 direction = BuildThrowDirection(dragDelta);

            float normalized = Mathf.InverseLerp(minDragDistance, maxDragDistance, dragDistance);
            float lineLength = Mathf.Lerp(aimLineMinLength, aimLineMaxLength, normalized);

            Vector3 end = start + direction * lineLength;

            SetAimLineVisible(true);
            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, end);
        }

        private void UpdateTrajectoryPreview(Vector2 dragDelta)
        {
            if (trajectoryLine == null || throwSpawnPoint == null || gameplayManager == null)
            {
                return;
            }

            float dragDistance = dragDelta.magnitude;
            if (dragDistance < minDragDistance)
            {
                SetTrajectoryVisible(false);
                return;
            }

            GameplayItemData itemData = gameplayManager.CurrentItem;
            if (itemData == null)
            {
                SetTrajectoryVisible(false);
                return;
            }

            Vector3 startPosition = throwSpawnPoint.position;
            Vector3 direction = BuildThrowDirection(dragDelta);
            float forceMultiplier = BuildForceMultiplier(dragDistance);
            float finalForce = itemData.throwForce * forceMultiplier;
            Vector3 startVelocity = direction * finalForce;

            trajectoryLine.positionCount = trajectoryPointCount;

            for (int i = 0; i < trajectoryPointCount; i++)
            {
                float t = i * trajectoryTimeStep;
                Vector3 point = startPosition
                                + startVelocity * t
                                + 0.5f * Physics.gravity * t * t;

                trajectoryLine.SetPosition(i, point);
            }

            SetTrajectoryVisible(true);
        }

        private void SetAimLineVisible(bool visible)
        {
            if (aimLine != null)
            {
                aimLine.enabled = visible;
            }
        }

        private void SetTrajectoryVisible(bool visible)
        {
            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = visible;
            }
        }
    }
}
