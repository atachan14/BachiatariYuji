using UnityEngine;

public class NextDayNode : BaseNode
{

    public override void PlayNode()
    {
        GameData.Instance.Day += 1;
        DayData.Instance.DayTime = DayTime.Morning;

        // --- Seed���� ---
        GameData.Instance.DaySeed = System.Environment.TickCount; // �܂��͗����Ő���

        nextNode?.PlayNode();
    }
}
