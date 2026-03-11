using System.Collections;
using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyHitReaction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Renderer[] targetRenderers;

        [Header("Hit Stop")]
        [SerializeField] private float defaultHitStopDuration = 0.05f;
        [SerializeField] private float heavyHitStopDuration = 0.07f;

        [Header("Scale Punch")]
        [SerializeField] private float scalePunchAmount = 0.12f;
        [SerializeField] private float scalePunchDuration = 0.12f;

        [Header("Flash Color")]
        [SerializeField] private Color flashColor = new Color(1f, 0.65f, 0.65f, 1f);
        [SerializeField] private float flashDuration = 0.10f;

        [Header("Small Knockback")]
        [SerializeField] private Vector3 knockbackOffset = new Vector3(0f, 0f, -0.18f);
        [SerializeField] private float knockbackDuration = 0.12f;

        [Header("Foam Tint")]
        [SerializeField] private Color foamTintColor = new Color(0.8f, 0.95f, 1f, 1f);
        [SerializeField] private float foamTintDuration = 0.6f;

        [Header("Wiggle")]
        [SerializeField] private float wiggleAngle = 8f;
        [SerializeField] private float wiggleDuration = 0.16f;

        private bool hitStopRunning;
        private Coroutine reactionCoroutine;

        private Vector3 originalLocalScale;
        private Vector3 originalLocalPosition;
        private Quaternion originalLocalRotation;

        private Material[] runtimeMaterials;
        private Color[] originalColors;

        private void Awake()
        {
            if (visualRoot == null)
            {
                visualRoot = transform;
            }

            originalLocalScale = visualRoot.localScale;
            originalLocalPosition = visualRoot.localPosition;
            originalLocalRotation = visualRoot.localRotation;

            CacheMaterials();
        }

        public void PlayHitFeedback(HitFeedbackType feedbackType)
        {
            float hitStopDuration = feedbackType == HitFeedbackType.ScalePunch
                ? heavyHitStopDuration
                : defaultHitStopDuration;

            PlayHitStop(hitStopDuration);

            if (reactionCoroutine != null)
            {
                StopCoroutine(reactionCoroutine);
                ResetVisualState();
            }

            reactionCoroutine = StartCoroutine(PlayReactionCoroutine(feedbackType));
        }

        private void PlayHitStop(float duration)
        {
            if (!hitStopRunning)
            {
                StartCoroutine(HitStopCoroutine(duration));
            }
        }

        private IEnumerator HitStopCoroutine(float duration)
        {
            hitStopRunning = true;

            float originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = originalTimeScale;
            hitStopRunning = false;
        }

        private IEnumerator PlayReactionCoroutine(HitFeedbackType feedbackType)
        {
            switch (feedbackType)
            {
                case HitFeedbackType.ScalePunch:
                    yield return StartCoroutine(ScalePunchCoroutine());
                    break;

                case HitFeedbackType.FlashColor:
                    yield return StartCoroutine(FlashColorCoroutine());
                    break;

                case HitFeedbackType.SmallKnockback:
                    yield return StartCoroutine(SmallKnockbackCoroutine());
                    break;

                case HitFeedbackType.FoamTint:
                    yield return StartCoroutine(FoamTintCoroutine());
                    break;

                case HitFeedbackType.Wiggle:
                    yield return StartCoroutine(WiggleCoroutine());
                    break;

                default:
                    yield return StartCoroutine(WiggleCoroutine());
                    break;
            }

            ResetVisualState();
            reactionCoroutine = null;
        }

        private IEnumerator ScalePunchCoroutine()
        {
            float timer = 0f;
            Vector3 punchScale = originalLocalScale * (1f + scalePunchAmount);

            while (timer < scalePunchDuration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / scalePunchDuration;

                if (t < 0.5f)
                {
                    float lerp = t / 0.5f;
                    visualRoot.localScale = Vector3.Lerp(originalLocalScale, punchScale, lerp);
                }
                else
                {
                    float lerp = (t - 0.5f) / 0.5f;
                    visualRoot.localScale = Vector3.Lerp(punchScale, originalLocalScale, lerp);
                }

                yield return null;
            }

            visualRoot.localScale = originalLocalScale;
        }

        private IEnumerator FlashColorCoroutine()
        {
            if (runtimeMaterials == null || runtimeMaterials.Length == 0)
            {
                yield break;
            }

            SetAllMaterialColors(flashColor);

            float timer = 0f;
            while (timer < flashDuration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / flashDuration;

                for (int i = 0; i < runtimeMaterials.Length; i++)
                {
                    if (runtimeMaterials[i] != null)
                    {
                        Color color = Color.Lerp(flashColor, originalColors[i], t);
                        runtimeMaterials[i].color = color;
                    }
                }

                yield return null;
            }

            RestoreOriginalColors();
        }

        private IEnumerator SmallKnockbackCoroutine()
        {
            float timer = 0f;
            Vector3 hitPosition = originalLocalPosition + knockbackOffset;

            while (timer < knockbackDuration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / knockbackDuration;

                if (t < 0.5f)
                {
                    float lerp = t / 0.5f;
                    visualRoot.localPosition = Vector3.Lerp(originalLocalPosition, hitPosition, lerp);
                }
                else
                {
                    float lerp = (t - 0.5f) / 0.5f;
                    visualRoot.localPosition = Vector3.Lerp(hitPosition, originalLocalPosition, lerp);
                }

                yield return null;
            }

            visualRoot.localPosition = originalLocalPosition;
        }

        private IEnumerator FoamTintCoroutine()
        {
            if (runtimeMaterials == null || runtimeMaterials.Length == 0)
            {
                yield break;
            }

            SetAllMaterialColors(foamTintColor);

            float timer = 0f;
            while (timer < foamTintDuration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / foamTintDuration;

                for (int i = 0; i < runtimeMaterials.Length; i++)
                {
                    if (runtimeMaterials[i] != null)
                    {
                        Color color = Color.Lerp(foamTintColor, originalColors[i], t);
                        runtimeMaterials[i].color = color;
                    }
                }

                yield return null;
            }

            RestoreOriginalColors();
        }

        private IEnumerator WiggleCoroutine()
        {
            float timer = 0f;

            while (timer < wiggleDuration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / wiggleDuration;

                float angle = Mathf.Sin(t * Mathf.PI * 4f) * wiggleAngle * (1f - t);
                visualRoot.localRotation = originalLocalRotation * Quaternion.Euler(0f, angle, 0f);

                yield return null;
            }

            visualRoot.localRotation = originalLocalRotation;
        }

        private void CacheMaterials()
        {
            if (targetRenderers == null || targetRenderers.Length == 0)
            {
                runtimeMaterials = new Material[0];
                originalColors = new Color[0];
                return;
            }

            runtimeMaterials = new Material[targetRenderers.Length];
            originalColors = new Color[targetRenderers.Length];

            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null)
                {
                    continue;
                }

                runtimeMaterials[i] = targetRenderers[i].material;
                originalColors[i] = runtimeMaterials[i].color;
            }
        }

        private void SetAllMaterialColors(Color color)
        {
            if (runtimeMaterials == null)
            {
                return;
            }

            for (int i = 0; i < runtimeMaterials.Length; i++)
            {
                if (runtimeMaterials[i] != null)
                {
                    runtimeMaterials[i].color = color;
                }
            }
        }

        private void RestoreOriginalColors()
        {
            if (runtimeMaterials == null || originalColors == null)
            {
                return;
            }

            for (int i = 0; i < runtimeMaterials.Length; i++)
            {
                if (runtimeMaterials[i] != null)
                {
                    runtimeMaterials[i].color = originalColors[i];
                }
            }
        }

        private void ResetVisualState()
        {
            if (visualRoot != null)
            {
                visualRoot.localScale = originalLocalScale;
                visualRoot.localPosition = originalLocalPosition;
                visualRoot.localRotation = originalLocalRotation;
            }

            RestoreOriginalColors();
        }
    }
}
