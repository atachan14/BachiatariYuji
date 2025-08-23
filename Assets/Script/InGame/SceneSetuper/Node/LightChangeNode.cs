using UnityEngine;

public class LightChangeNode : BaseNode
{
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private float duration = 1f;
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        StartCoroutine(ChangeLightCoroutine());
    }

    private System.Collections.IEnumerator ChangeLightCoroutine()
    {
        var light = RenderSettings.ambientLight;
        Color startColor = light;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            RenderSettings.ambientLight = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }

        RenderSettings.ambientLight = targetColor;
        nextNode?.PlayNode();
    }
}
