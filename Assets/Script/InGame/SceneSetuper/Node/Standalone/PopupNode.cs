
public class PopupNode : BaseNode
{
    public PopupSO so;

    public override void PlayNode()
    {
        PopupManager.Instance.ShowPopup(so, transform);
    }
}