using UnityEngine;

public class ForestSetuper : SceneSetuper
{
    protected override void Start()
    {
        ForestGenManager.Instance.Generate();

        base.Start();
    }
}
