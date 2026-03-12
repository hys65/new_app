using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ImpactStain : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 15f;
        [SerializeField] private bool randomRotation = true;
        [SerializeField] private Vector2 randomScaleMultiplier = new Vector2(0.9f, 1.15f);

        private void Start()
        {
            if (randomRotation)
            {
                transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
            }

            float mul = Random.Range(randomScaleMultiplier.x, randomScaleMultiplier.y);
            transform.localScale *= mul;

            if (lifeTime > 0f)
            {
                Destroy(gameObject, lifeTime);
            }
        }
    }
}
