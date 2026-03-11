using TMPro;
using UnityEngine;

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

        // 上浮
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 淡出
        if (text != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }

        // 销毁
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}