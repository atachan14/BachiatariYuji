using UnityEngine;

public class StatusChangeNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        // TODO: Yuji �̃X�v���C�g��Q�Ă��ԂɕύX
        // Yuji.Instance.SetSleepingSprite();
        Debug.Log("statusChange");

        nextNode?.PlayNode();
    }
}
