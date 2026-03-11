using TMPro;
using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    public class HitPopupController : MonoBehaviour
    {
        public float moveSpeed = 80f;
        public float lifetime = 0.8f;

        private TextMeshProUGUI text;
        private float timer;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        void Update()
        {
            timer += Time.deltaTime;

            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            if (text != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
                Color c = text.color;
                c.a = alpha;
                text.color = c;
            }

            if (timer >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}
