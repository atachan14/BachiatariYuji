using UnityEngine;

public class NextDayNode : BaseNode
{

    public override void PlayNode()
    {
        GameData.Instance.Day += 1;
        DayData.Instance.DayTime = DayTime.Morning;

        // --- SeedŒˆ’è ---
        GameData.Instance.DaySeed = System.Environment.TickCount; // ‚Ü‚½‚Í—”‚Å¶¬

        nextNode?.PlayNode();
    }
}
