using UnityEngine;

public class CanActionerRefreshNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        CanActionerManager.Instance.RefreshAll();

        nextNode?.PlayNode();
    }
}