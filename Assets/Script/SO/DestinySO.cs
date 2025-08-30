using UnityEngine;
public enum DestinyType
{
    NoneOmen,FloorOmen,WallOmen,BeyondOmen,Punish,Floor,InnerTree
}

[CreateAssetMenu(fileName = "DestinySO", menuName = "Scriptable Objects/DestinySO")]
public class DestinySO : ScriptableObject
{
    public DestinyType destinyType;

    [Header("�o�����W�b�N")]
    public float peakDay;  // �����l (Day)
    public float sigma;           // �L���� (���U)
    public float rarity;          // �����x���тł̏o�₷���{��
    public float baseWeight;      // �펞�o���␳ (�Œ�ۏ�)
}