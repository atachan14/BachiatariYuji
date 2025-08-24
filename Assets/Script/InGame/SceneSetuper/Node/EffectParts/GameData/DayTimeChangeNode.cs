using UnityEngine;

public class DayTimeChangeNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;
    [SerializeField] private DayTime dayTime;

    public override void PlayNode()
    {
        GameData.Instance.DayTime = dayTime;

        nextNode?.PlayNode();
    }
}
