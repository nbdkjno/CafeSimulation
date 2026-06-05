using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 0.5f;  // время исчезновения (секунды)

    private SpriteRenderer spriteRenderer;
    private CanvasGroup canvasGroup;
    private float startAlpha = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvasGroup = GetComponent<CanvasGroup>();
        startAlpha = GetCurrentAlpha();
    }

    float GetCurrentAlpha()
    {
        if (spriteRenderer != null) return spriteRenderer.color.a;
        if (canvasGroup != null) return canvasGroup.alpha;
        return 1f;
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);

            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}