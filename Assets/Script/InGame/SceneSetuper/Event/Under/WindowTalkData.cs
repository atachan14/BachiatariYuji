using UnityEngine;

[CreateAssetMenu(fileName = "UnderData", menuName = "Scriptable Objects/UnderData")]
public class WindowTalkData : ScriptableObject
{
    public string talkName;
    [TextArea] public string[] text;   // 1�̉�b�̃Z���t�̗���i���Ԃŕ\���j

    public float fontSize = 50f;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public float fastDelay = 0.0f;


    public EventBase nextEvent; // �i�����ꍇ�� null or ��z��j
}
