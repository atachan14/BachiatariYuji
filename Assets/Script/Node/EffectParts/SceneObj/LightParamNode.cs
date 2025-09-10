using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections;

public class LightParamNode : BaseNode
{
    [SerializeField] private Light2D targetLight;

    [Header("Target Parameters")]
    [SerializeField] private float targetIntensity = 1f;
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private float targetInnerRadius = 0f;
    [SerializeField] private float targetOuterRadius = 5f;

    [SerializeField] private float duration = 1f; // フェード時間

    public override void PlayNode()
    {
        if (targetLight == null)
        {
            Debug.LogWarning($"{nameof(LightParamNode)}: Target Light not set!");
            nextNode?.PlayNode();
            return;
        }

        StartCoroutine(ChangeLightCoroutine());
    }

    private IEnumerator ChangeLightCoroutine()
    {
        float startIntensity = targetLight.intensity;
        Color startColor = targetLight.color;
        float startInner = targetLight.pointLightInnerRadius;
        float startOuter = targetLight.pointLightOuterRadius;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(duration, 0.0001f));

            targetLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            targetLight.color = Color.Lerp(startColor, targetColor, t);
            targetLight.pointLightInnerRadius = Mathf.Lerp(startInner, targetInnerRadius, t);
            targetLight.pointLightOuterRadius = Mathf.Lerp(startOuter, targetOuterRadius, t);

            yield return null;
        }

        // 最終値に確実に反映
        targetLight.intensity = targetIntensity;
        targetLight.color = targetColor;
        targetLight.pointLightInnerRadius = targetInnerRadius;
        targetLight.pointLightOuterRadius = targetOuterRadius;

        nextNode?.PlayNode();
    }
}