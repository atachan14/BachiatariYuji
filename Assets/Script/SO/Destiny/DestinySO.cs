using UnityEngine;

[CreateAssetMenu(fileName = "DestinySO", menuName = "DestinySO/DestinySO")]
public abstract class DestinySO : ScriptableObject
{
    [Header("�o�����W�b�N")]
    public AnimationCurve weightCurve;
    public float rarity;     // �����x���тł̏o�₷���{��
    public float baseWeight; // �펞�o���␳ (�Œ�ۏ�)

    public float GetWeight(float evilValue)
    {
        float curveValue = weightCurve.Evaluate(evilValue);
        return curveValue * rarity + baseWeight;
    }
}