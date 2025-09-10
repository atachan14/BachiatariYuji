using UnityEngine;

public abstract class BaseNode : MonoBehaviour
{
    [SerializeField] public string NodeName;
    [SerializeField] private Sprite actionIcon; // Inspector‚Åİ’è
    public Sprite ActionIcon => actionIcon;     // UIê—p‚ÉŒöŠJ
    public BaseNode nextNode;
    public abstract void PlayNode();
}
