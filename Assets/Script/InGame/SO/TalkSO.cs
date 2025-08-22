using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "TalkSO", menuName = "NodeSO/TalkSO")]
public class TalkSO : NodeSO
{
    [TextArea] public string[] text;   // 1つの会話のセリフの流れ（順番で表示）

    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public bool canFastForward = true;


    public BaseNode nextEvent; // （無い場合は null or 空配列）
}
