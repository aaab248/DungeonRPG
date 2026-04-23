using System.Collections;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine currentFade;

    [SerializeField]
    private float fadeDuration = 0.1f;
    [SerializeField]
    private float originAlpha;
    [SerializeField]
    private float fadeAlpha = 0.6f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originAlpha = spriteRenderer.color.a;
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-100 * transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFade(fadeAlpha, fadeDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFade(originAlpha, fadeDuration);
        }
    }

    void StartFade(float targetAlpha, float duration)
    {
        if(currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(Fade(targetAlpha,duration));
    }

    IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            Color c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;

            yield return null;
        }

        Color final = spriteRenderer.color;
        final.a = targetAlpha;
        spriteRenderer.color = final;

        currentFade = null;
    }
}
