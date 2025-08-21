using UnityEngine;

public abstract class BaseNode : MonoBehaviour
{
    [SerializeField] public string NodeName;
    public abstract void PlayNode();
}
