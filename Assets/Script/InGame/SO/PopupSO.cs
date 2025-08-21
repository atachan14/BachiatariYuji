using UnityEngine;

public enum TalkUIType
{
    Under,Popup
}
[CreateAssetMenu(fileName = "TalkData", menuName = "NodeSO/PopupSO")]
public class PopupSO : NodeSO
{
    public string PopupName; // Ž¯•ÊŽq—p
    [TextArea] public string text;
    public float fontSize = 20f;
    public float yOffset = 1f;
    public float lifeTime = 3f;
}