using UnityEngine;

[CreateAssetMenu(fileName = "TalkSO", menuName = "NodeSO/TalkSO")]
public class TalkSO : NodeSO
{
    public string talkName;
    [TextArea] public string[] text;   // 1�̉�b�̃Z���t�̗���i���Ԃŕ\���j

    public float fontSize = 50f;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public bool canFastForward = true;


    public BaseNode nextEvent; // �i�����ꍇ�� null or ��z��j
}
