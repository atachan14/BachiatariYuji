using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightEnabledNode : BaseNode
{
    [SerializeField] private Light2D targetLight;
    [SerializeField] private bool targetEnabled = true;

    public override void PlayNode()
    {
        if (targetLight == null)
        {
            Debug.LogWarning($"{nameof(LightEnabledNode)}: Target Light not set!");
            nextNode?.PlayNode();
            return;
        }

        // ë¶éûêÿë÷
        targetLight.enabled = targetEnabled;

        nextNode?.PlayNode();
    }
}