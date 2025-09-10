using UnityEngine;

public class NextDayTimeNode : BaseNode
{

    public override void PlayNode()
    {
        DayData.Instance.NextDayTime();

        nextNode?.PlayNode();
    }
}
