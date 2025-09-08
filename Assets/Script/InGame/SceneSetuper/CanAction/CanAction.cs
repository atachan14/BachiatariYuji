using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanAction : MonoBehaviour
{
    [SerializeField] protected BaseNode[] nodes;
    protected BaseNode currentNode;

    private void Start()
    {
        ChooseNode();
    }
    public virtual void ChooseNode()
    {
        currentNode = nodes[0];
    }
    public virtual void DoAction()
    {
        currentNode.PlayNode();
    }
    protected BaseNode GetNode(string nodeName)
    {
        var node = nodes.FirstOrDefault(n => n.NodeName == nodeName);
        if (node == null)
        {
            Debug.LogWarning($"GetNode: ƒm[ƒh '{nodeName}' ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½I");
        }
        return node;
    }
    public Sprite GetIcon()
    {
        return currentNode.ActionIcon;
    }
}