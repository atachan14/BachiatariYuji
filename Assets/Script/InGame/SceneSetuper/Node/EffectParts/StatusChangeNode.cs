using UnityEngine;

public class StatusChangeNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        // TODO: Yuji のスプライトを寝てる状態に変更
        // Yuji.Instance.SetSleepingSprite();
        Debug.Log("statusChange");

        nextNode?.PlayNode();
    }
}
