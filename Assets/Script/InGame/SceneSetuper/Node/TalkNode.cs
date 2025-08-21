
public class TalkNode : BaseNode
{
    public TalkSO so;
    public override void PlayNode()
    {
        TalkManager.Instance.ShowUnder(so);
    }
}
