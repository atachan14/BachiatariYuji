using UnityEngine;

[CreateAssetMenu(fileName = "ForestGimmickSO", menuName = "Scriptable Objects/ForestGimmickSO")]
public class ForestOmenSO : ScriptableObject
{
    public string gimmickName;

    [Header("�o�����W�b�N")]
    public float preferredLevel;  // �����l (TotalEvil)
    public float sigma;           // �L���� (���U)
    public float rarity;          // �����x���тł̏o�₷���{��
    public float baseWeight;      // �펞�o���␳ (�Œ�ۏ�)
}