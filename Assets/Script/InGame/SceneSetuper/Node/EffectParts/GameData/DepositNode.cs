using UnityEngine;

public class DepositNode : BaseNode
{
    [SerializeField] BaseNode nextNode;
    public override void PlayNode()
    {
        int cashBefore = GameData.Instance.Cash;
        int bankBefore = GameData.Instance.Bank;

        // ‘SŠzˆÚ“®
        GameData.Instance.Cash = 0;
        GameData.Instance.Bank = bankBefore + cashBefore;
        GameData.Instance.DayEvil = 0;

        nextNode?.PlayNode();
    }
}
