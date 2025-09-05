using UnityEngine;

public class DayTimeChangeNode : BaseNode
{
    [SerializeField] private DayTime dayTime;

    public override void PlayNode()
    {
        DayData.Instance.DayTime = dayTime;

        nextNode?.PlayNode();
    }
}
