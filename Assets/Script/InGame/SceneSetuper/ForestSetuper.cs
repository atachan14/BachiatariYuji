using UnityEngine;

public class ForestSetuper : SceneSetuper
{
    protected override void Start()
    {
        ForestGenManager.Instance.Generate();
        if (GameData.Instance.DayEvil > 0) OmenManager.Instance.Setup();

        base.Start();
    }
}
