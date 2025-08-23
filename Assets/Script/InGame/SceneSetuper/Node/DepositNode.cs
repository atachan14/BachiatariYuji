using UnityEngine;

public class DepositNode : BaseNode
{
    [SerializeField] BaseNode nextNode;
    public override void PlayNode()
    {
        int cashBefore = GameData.Instance.Cash;
        int bankBefore = GameData.Instance.Bank;

        if (cashBefore <= 0) return; // Cash ‚ª‚È‚¢ê‡‚Í‰½‚à‚µ‚È‚¢

        // ‘SŠzˆÚ“®
        GameData.Instance.Cash = 0;
        GameData.Instance.Bank = bankBefore + cashBefore;

        nextNode?.PlayNode();
    }
}
