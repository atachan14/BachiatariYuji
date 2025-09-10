using UnityEngine;

public class CanActionerRefreshNode : BaseNode
{

    public override void PlayNode()
    {
        

        nextNode?.PlayNode();
    }
}