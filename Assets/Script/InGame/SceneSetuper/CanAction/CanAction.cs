using System.Linq;
using UnityEngine;

public  class CanAction : MonoBehaviour
{
    [SerializeField] protected BaseNode[] nodes;
    protected BaseNode currentNode;

    private void Start()
    {
        ChooseNode();
    }
    protected virtual void ChooseNode()
    {
        currentNode = nodes[0];
    }
    public virtual void DoAction()
    {
        currentNode.PlayNode();
    }
    protected BaseNode GetNode(string nodeName)
    {
        return nodes.FirstOrDefault(n => n.NodeName == nodeName);
    }

}