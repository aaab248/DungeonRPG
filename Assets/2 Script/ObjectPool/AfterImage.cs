using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float alpha; // 스프라이트 투명도
    [SerializeField]
    private float alphaDecay = 3f; // 투명도 감소값

/// <summary>
/// 잔상 생성
/// </summary>
/// <param name="sprite"></param>
/// <param name="position"></param>
/// <param name="rotation"></param>
/// <param name="scale"></param>
/// <param name="color"></param>
/// <param name="dir"></param>
    public void Init(Sprite sprite, Vector3 position, Vector3 rotation, Vector3 scale, Color color)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        transform.position = position;
        transform.eulerAngles = rotation;
        transform.localScale = scale;

        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        alpha = color.a * 0.5f;
    }

    private void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;
        if(alpha <= 0)
        {
            // 알파값 0이하 => pool return
            AfterImagePool.Instance.ReturnAfterImage(this);
        }
        else
        {
            // 알파값 감소
            var color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}
