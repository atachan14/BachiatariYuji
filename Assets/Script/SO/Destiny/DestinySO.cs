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
        float curveValue = weightCurve.Evaluate(evilValue);
        return curveValue * rarity + baseWeight;
    }
}