using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class CameraShake : MonoBehaviour
    {
        private static CameraShake instance;

        private Vector3 originalLocalPos;
        private float shakeTimer;
        private float shakeDuration;
        private float shakeMagnitude;

        private void Awake()
        {
            instance = this;
            originalLocalPos = transform.localPosition;
        }

        private void Update()
        {
            if (shakeTimer > 0f)
            {
                shakeTimer -= Time.deltaTime;

                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;

                transform.localPosition = originalLocalPos + new Vector3(x, y, 0f);

                if (shakeTimer <= 0f)
                {
                    transform.localPosition = originalLocalPos;
                }
            }
        }

        public static void Shake(float duration, float magnitude)
        {
            if (instance == null)
                return;

            instance.shakeDuration = duration;
            instance.shakeMagnitude = magnitude;
            instance.shakeTimer = duration;
        }
    }
}
