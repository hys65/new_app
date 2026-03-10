using System.Collections;
using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyHitReaction : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float reactionDuration = 0.2f;

        private Vector3 originLocalScale;
        private Vector3 originLocalPosition;
        private Color originColor = Color.white;
        private Material runtimeMaterial;

        private void Awake()
        {
            if (visualRoot == null)
            {
                visualRoot = transform;
            }

            originLocalScale = visualRoot.localScale;
            originLocalPosition = visualRoot.localPosition;

            if (targetRenderer != null)
            {
                runtimeMaterial = targetRenderer.material;
                originColor = runtimeMaterial.color;
            }
        }

        public void PlayHitFeedback(HitFeedbackType feedbackType)
        {
            StopAllCoroutines();
            StartCoroutine(DoFeedback(feedbackType));
        }

        private IEnumerator DoFeedback(HitFeedbackType feedbackType)
        {
            var elapsed = 0f;
            while (elapsed < reactionDuration)
            {
                elapsed += Time.deltaTime;
                var progress = Mathf.Clamp01(elapsed / reactionDuration);

                ApplyVisual(feedbackType, progress);
                yield return null;
            }

            visualRoot.localScale = originLocalScale;
            visualRoot.localPosition = originLocalPosition;
            if (runtimeMaterial != null)
            {
                runtimeMaterial.color = originColor;
            }
        }

        private void ApplyVisual(HitFeedbackType feedbackType, float progress)
        {
            var pingPong = Mathf.Sin(progress * Mathf.PI);
            switch (feedbackType)
            {
                case HitFeedbackType.ScalePunch:
                    visualRoot.localScale = originLocalScale * (1f + pingPong * 0.2f);
                    break;
                case HitFeedbackType.FlashColor:
                    if (runtimeMaterial != null)
                    {
                        runtimeMaterial.color = Color.Lerp(originColor, Color.red, pingPong);
                    }
                    break;
                case HitFeedbackType.SmallKnockback:
                    visualRoot.localPosition = originLocalPosition + new Vector3(0f, 0f, -0.1f * pingPong);
                    break;
                case HitFeedbackType.FoamTint:
                    if (runtimeMaterial != null)
                    {
                        runtimeMaterial.color = Color.Lerp(originColor, new Color(0.85f, 0.95f, 1f), pingPong);
                    }
                    break;
                case HitFeedbackType.Wiggle:
                    var wiggleX = Mathf.Sin(progress * 20f) * 0.03f;
                    visualRoot.localPosition = originLocalPosition + new Vector3(wiggleX, 0f, 0f);
                    break;
            }
        }
    }
}
