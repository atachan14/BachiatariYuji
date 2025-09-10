using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEnabledNode : BaseNode
{
    [SerializeField] private List<Light2D> targetLights = new List<Light2D>();
    [SerializeField] private bool targetEnabled = true;

    public override void PlayNode()
    {
        if (targetLights == null || targetLights.Count == 0)
        {
            nextNode?.PlayNode();
            return;
        }

        foreach (var light in targetLights)
        {
            if (light != null)
                light.enabled = targetEnabled;
        }

        nextNode?.PlayNode();
    }
}
