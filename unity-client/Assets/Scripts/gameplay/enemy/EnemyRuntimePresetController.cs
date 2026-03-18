using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyRuntimePresetController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyPresetApplicator presetApplicator;

        [Header("Runtime")]
        [SerializeField] private EnemyPresetData currentPreset;

        public EnemyPresetApplicator PresetApplicator => presetApplicator;
        public EnemyPresetData CurrentPreset => currentPreset;

        private void Reset()
        {
            if (presetApplicator == null)
            {
                presetApplicator = GetComponent<EnemyPresetApplicator>();
            }
        }

        private void Awake()
        {
            if (presetApplicator == null)
            {
                presetApplicator = GetComponent<EnemyPresetApplicator>();
            }

            if (presetApplicator != null)
            {
                currentPreset = presetApplicator.preset;
            }
        }

        [ContextMenu("Reapply Current Preset")]
        public void ReapplyCurrentPreset()
        {
            if (presetApplicator == null)
            {
                Debug.LogWarning("EnemyRuntimePresetController: presetApplicator is null", this);
                return;
            }

            if (currentPreset == null)
            {
                currentPreset = presetApplicator.preset;
            }

            if (currentPreset == null)
            {
                Debug.LogWarning("EnemyRuntimePresetController: currentPreset is null", this);
                return;
            }

            ApplyPreset(currentPreset);
        }

        public void ApplyPreset(EnemyPresetData preset)
        {
            if (presetApplicator == null)
            {
                Debug.LogWarning("EnemyRuntimePresetController: presetApplicator is null", this);
                return;
            }

            if (preset == null)
            {
                Debug.LogWarning("EnemyRuntimePresetController: preset is null", this);
                return;
            }

            currentPreset = preset;
            presetApplicator.preset = preset;
            presetApplicator.ApplyPreset();
        }
    }
}
