using UnityEngine;


[CreateAssetMenu(fileName = "DestinySO", menuName = "DestinySO/DestinySO")]
public abstract class DestinySO : ScriptableObject
{
    [Header("�o�����W�b�N")]
    public float peak;            // �����l (Day)
    public float sigma;           // �L���� (���U)
    public float rarity;          // �����x���тł̏o�₷���{��
    public float baseWeight;      // �펞�o���␳ (�Œ�ۏ�)
}