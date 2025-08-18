using UnityEngine;

public enum TalkUIType
{
    Under,Popup
}
[CreateAssetMenu(fileName = "TalkData", menuName = "Scriptable Objects/TalkData")]
public class TalkData : ScriptableObject
{
    public string talkName; // Ž¯•ÊŽq—p
    public TalkUIType uiType;
    [TextArea] public string text;
    public float textSize = 20f;
    public float yOffset = 1f;
    public float lifeTime = 3f;
}