using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : SingletonMonoBehaviour<GlobalLightController>
{
    [SerializeField] private Light2D globalLight;

    [Header("Intensity Settings")]
    [SerializeField] private float morningIntensity = 1f;
    [SerializeField] private float nightIntensity = 0.3f;

    [Header("Color Settings")]
    [SerializeField] private Color morningColor = Color.white;
    [SerializeField] private Color nightColor = new Color(0.6f, 0.7f, 1f); // �������ۂ�

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;

    private Coroutine currentCoroutine;

    public void SetDayTime(DayTime time)
    {
        float targetIntensity = time == DayTime.Morning ? morningIntensity : nightIntensity;
        Color targetColor = time == DayTime.Morning ? morningColor : nightColor;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ChangeLightCoroutine(targetIntensity, targetColor));
    }

    private System.Collections.IEnumerator ChangeLightCoroutine(float targetIntensity, Color targetColor)
    {
        float startIntensity = globalLight.intensity;
        Color startColor = globalLight.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            globalLight.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        globalLight.intensity = targetIntensity;
        globalLight.color = targetColor;
        currentCoroutine = null;
    }
}
