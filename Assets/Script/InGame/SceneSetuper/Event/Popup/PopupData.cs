using UnityEngine;

public enum TalkUIType
{
    Under,Popup
}
[CreateAssetMenu(fileName = "TalkData", menuName = "Scriptable Objects/TalkData")]
public class PopupData : ScriptableObject
{
    public string PopupName; // Ž¯•ÊŽq—p
    [TextArea] public string text;
    public float textSize = 20f;
    public float yOffset = 1f;
    public float lifeTime = 3f;
}