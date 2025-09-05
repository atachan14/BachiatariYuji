using UnityEngine;

public class DepositNode : BaseNode
{
    public override void PlayNode()
    {
        int cashBefore = DayData.Instance.Cash;
        int bankBefore = GameData.Instance.Bank;

        // ‘SŠzˆÚ“®
        DayData.Instance.Cash = 0;
        GameData.Instance.Bank = bankBefore + cashBefore;
        DayData.Instance.DayEvil = 0;

        nextNode?.PlayNode();
    }
}
