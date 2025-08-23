using UnityEngine;

public class NextDayNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        GameData.Instance.Day += 1;
        GameData.Instance.DayTime = DayTime.Morning;

        nextNode?.PlayNode();
    }
}
