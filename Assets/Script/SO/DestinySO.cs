using UnityEngine;
public enum DestinyType
{
    Omen,Punish,InnerTree
}

[CreateAssetMenu(fileName = "DestinySO", menuName = "Scriptable Objects/DestinySO")]
public class DestinySO : ScriptableObject
{
    public DestinyType DestinyType;

    [Header("�o�����W�b�N")]
    public float preferredLevel;  // �����l (TotalEvil)
    public float sigma;           // �L���� (���U)
    public float rarity;          // �����x���тł̏o�₷���{��
    public float baseWeight;      // �펞�o���␳ (�Œ�ۏ�)
}