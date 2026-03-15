using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class EnemyPresetApplicator : MonoBehaviour
    {
        [Header("Preset")]
        public EnemyPresetData preset;

        [Header("Controllers")]
        public EnemyReactionLayerController reactionLayer;
        public EnemyDefenseController defenseController;
        public EnemyAiLayerController aiLayer;
        public EnemyDefenseStateWindowController defenseWindow;

        [ContextMenu("Apply Preset")]
        public void ApplyPreset()
        {
            if (preset == null)
            {
                Debug.LogWarning("EnemyPresetApplicator: preset is null");
                return;
            }

            if (reactionLayer != null)
            {
                reactionLayer.enemyArchetype = preset.archetype;
            }

            if (defenseController != null)
            {
                defenseController.defensePattern = preset.defensePattern;
            }

            if (aiLayer != null)
            {
                aiLayer.aiProfile = preset.aiProfile;
            }

            if (defenseWindow != null)
            {
                defenseWindow.stateProfile = preset.defenseStateWindowProfile;
            }

            Debug.Log("Enemy preset applied: " + preset.displayName);
        }

        private void Awake()
        {
            ApplyPreset();
        }
    }
}
