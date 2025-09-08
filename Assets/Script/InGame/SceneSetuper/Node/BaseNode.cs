using UnityEngine;

public abstract class BaseNode : MonoBehaviour
{
    [SerializeField] public string NodeName;
    [SerializeField] private Sprite actionIcon; // Inspector�Őݒ�
    public Sprite ActionIcon => actionIcon;     // UI��p�Ɍ��J
    public BaseNode nextNode;
    public abstract void PlayNode();
}
