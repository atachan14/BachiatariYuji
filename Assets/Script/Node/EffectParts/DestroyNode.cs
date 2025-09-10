using UnityEngine;

public class DestroyNode : BaseNode
{
    [SerializeField] private GameObject[] targets;

    public override void PlayNode()
    {
        if (targets != null && targets.Length > 0)
        {
            foreach (var target in targets)
            {
                if (target != null)
                    Destroy(target);
            }
        }
        else
        {
            Debug.LogWarning("DestroyNode: No targets assigned!");
        }

        nextNode?.PlayNode();
    }
}
