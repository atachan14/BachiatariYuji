using UnityEngine;

public abstract class BaseNode : MonoBehaviour
{
    [SerializeField] public string NodeName;
     public BaseNode nextNode;
    public abstract void PlayNode();
}
