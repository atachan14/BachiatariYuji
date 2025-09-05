using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChangeNode : BaseNode
{
    [Header("Fade Params")]
    [SerializeField, Range(0f, 1f)] private float startAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float endAlpha = 1f;
    [SerializeField, Min(0f)] private float duration = 0.5f;
    [SerializeField] private bool useUnscaledTime = false;
    [SerializeField] private AnimationCurve easing = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Targets")]
    [SerializeField] private Image[] uiImages;
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    private Coroutine running;

    public override void PlayNode()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // 初期アルファを適用
        ApplyAlpha(startAlpha);

        if (duration <= 0f)
        {
            ApplyAlpha(endAlpha);
            running = null;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float u = Mathf.Clamp01(t / duration);
            float eased = Mathf.Clamp01(easing.Evaluate(u));
            float a = Mathf.LerpUnclamped(startAlpha, endAlpha, eased);
            ApplyAlpha(a);
            yield return null;
        }

        // 最終値で固定
        ApplyAlpha(endAlpha);
        running = null;
        nextNode?.PlayNode();
    }

    private void ApplyAlpha(float a)
    {
        a = Mathf.Clamp01(a);

        // UI.Image
        if (uiImages != null)
        {
            for (int i = 0; i < uiImages.Length; i++)
            {
                var img = uiImages[i];
                if (img == null) continue;
                Color c = img.color;
                c.a = a;
                img.color = c;
            }
        }

        // SpriteRenderer
        if (spriteRenderers != null)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                var sr = spriteRenderers[i];
                if (sr == null) continue;
                Color c = sr.color;
                c.a = a;
                sr.color = c;
            }
        }
    }
}
