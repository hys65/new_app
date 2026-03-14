using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public static class ImpactSfxPlayer
    {
        public static void PlayAtPoint(AudioClip clip, Vector3 position, float volume = 1f, Vector2? pitchRange = null)
        {
            if (clip == null)
            {
                return;
            }

            GameObject audioObject = new GameObject("impact_sfx_temp");
            audioObject.transform.position = position;

            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1.5f;
            audioSource.maxDistance = 15f;
            audioSource.playOnAwake = false;
            audioSource.volume = Mathf.Clamp01(volume);

            Vector2 finalPitchRange = pitchRange ?? new Vector2(1f, 1f);
            float minPitch = Mathf.Min(finalPitchRange.x, finalPitchRange.y);
            float maxPitch = Mathf.Max(finalPitchRange.x, finalPitchRange.y);
            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.Play();

            float destroyDelay = Mathf.Max(clip.length / Mathf.Max(0.01f, Mathf.Abs(audioSource.pitch)), 0.1f);
            Object.Destroy(audioObject, destroyDelay + 0.1f);
        }
    }
}
