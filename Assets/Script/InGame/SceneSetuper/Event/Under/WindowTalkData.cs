using UnityEngine;

[CreateAssetMenu(fileName = "UnderData", menuName = "Scriptable Objects/UnderData")]
public class WindowTalkData : ScriptableObject
{
    public string talkName;
    [TextArea] public string[] text;   // 1つの会話のセリフの流れ（順番で表示）

    public float fontSize = 50f;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public float fastDelay = 0.0f;


    public EventBase nextEvent; // （無い場合は null or 空配列）
}
