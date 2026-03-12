using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class ImpactStain : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 20f;
        [SerializeField] private bool randomYawOnStart = true;
        [SerializeField] private Vector2 randomScaleMultiplier = new Vector2(0.9f, 1.15f);

        private void Start()
        {
            if (randomYawOnStart)
            {
                transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
            }

            float scaleMul = Random.Range(randomScaleMultiplier.x, randomScaleMultiplier.y);
            transform.localScale *= scaleMul;

            if (lifeTime > 0f)
            {
                Destroy(gameObject, lifeTime);
            }
        }
    }
}
