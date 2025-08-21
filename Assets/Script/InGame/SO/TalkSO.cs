using UnityEngine;

[CreateAssetMenu(fileName = "TalkSO", menuName = "NodeSO/TalkSO")]
public class TalkSO : NodeSO
{
    public string talkName;
    [TextArea] public string[] text;   // 1つの会話のセリフの流れ（順番で表示）

    public float fontSize = 50f;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public bool canFastForward = true;


    public BaseNode nextEvent; // （無い場合は null or 空配列）
}
