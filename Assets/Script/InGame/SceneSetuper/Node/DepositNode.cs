using UnityEngine;

public class DepositNode : BaseNode
{
    [SerializeField] BaseNode nextNode;
    public override void PlayNode()
    {
        int cashBefore = GameData.Instance.Cash;
        int bankBefore = GameData.Instance.Bank;

        if (cashBefore <= 0) return; // Cash ���Ȃ��ꍇ�͉������Ȃ�

        // �S�z�ړ�
        GameData.Instance.Cash = 0;
        GameData.Instance.Bank = bankBefore + cashBefore;

        nextNode?.PlayNode();
    }
}
