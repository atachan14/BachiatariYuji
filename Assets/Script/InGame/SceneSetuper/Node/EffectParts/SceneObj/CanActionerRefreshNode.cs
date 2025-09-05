using UnityEngine;

public class CanActionerRefreshNode : BaseNode
{

    public override void PlayNode()
    {
        CanActionerManager.Instance.RefreshAll();

        nextNode?.PlayNode();
    }
}