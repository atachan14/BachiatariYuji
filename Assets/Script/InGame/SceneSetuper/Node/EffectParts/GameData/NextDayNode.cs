using UnityEngine;

public class NextDayNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        GameData.Instance.Day += 1;
        DayData.Instance.DayTime = DayTime.Morning;

        // --- Seed���� ---
        GameData.Instance.DaySeed = System.Environment.TickCount; // �܂��͗����Ő���

        nextNode?.PlayNode();
    }
}
