public class ForestSetuper : SceneData
{
    protected override void Start()
    {
        ForestManager.Instance.Generate();
        if (DayData.Instance.DayEvil > 0) OmenManager.Instance.Setup();

        base.Start();
    }
}
