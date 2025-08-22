using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "TalkSO", menuName = "NodeSO/TalkSO")]
public class TalkSO : NodeSO
{
    [TextArea] public string[] text;   // 1�̉�b�̃Z���t�̗���i���Ԃŕ\���j

    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public bool canFastForward = true;


    public BaseNode nextEvent; // �i�����ꍇ�� null or ��z��j
}
