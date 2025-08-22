
using UnityEngine;

public class TalkNode : BaseNode
{
    public TalkSO so;
    public BaseNode nextNode;
    public override void PlayNode()
    {
        TalkManager.Instance.ShowTalk(this);
    }
}
