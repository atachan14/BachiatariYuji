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
        if (weightCurve == null)
        {
            Debug.LogError($"[GetWeight] weightCurve is NULL! SO: {name}");
            return baseWeight; // fallback
        }

        if (weightCurve.keys == null || weightCurve.keys.Length == 0)
        {
            Debug.LogError($"[GetWeight] weightCurve has NO KEYS! SO: {name}");
            return baseWeight;
        }
        float curveValue = weightCurve.Evaluate(evilValue);
        return curveValue * rarity + baseWeight;
    }
}