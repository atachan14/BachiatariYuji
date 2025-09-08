using UnityEngine;

[CreateAssetMenu(fileName = "DestinySO", menuName = "DestinySO/DestinySO")]
public abstract class DestinySO : ScriptableObject
{
    [Header("出現ロジック")]
    public AnimationCurve weightCurve;
    public float rarity;     // 同レベル帯での出やすさ倍率
    public float baseWeight; // 常時出現補正 (最低保証)

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