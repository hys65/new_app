using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class HitPopupSpawner : MonoBehaviour
    {
        [SerializeField] private Canvas hudCanvas;
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private GameObject hitPopupPrefab;

        public void SpawnPopup(Vector3 worldPosition, string message)
        {
            if (hudCanvas == null || gameplayCamera == null || hitPopupPrefab == null)
            {
                return;
            }

            GameObject popupObject = Instantiate(hitPopupPrefab, hudCanvas.transform);
            RectTransform popupRect = popupObject.GetComponent<RectTransform>();

            Vector3 screenPosition = gameplayCamera.WorldToScreenPoint(worldPosition);

            popupRect.position = screenPosition;

            HitPopupController popupController = popupObject.GetComponent<HitPopupController>();
            if (popupController != null)
            {
                popupController.SetText(message);
            }
        }
    }
}
