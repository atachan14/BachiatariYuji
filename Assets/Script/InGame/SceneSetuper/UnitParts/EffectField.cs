using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Slow,
    FixDef,
    PerDef,
    DetoxPower,
    Vision,
    Hallucinations,
    Drowsiness
    // etc...
}

[System.Serializable]
public struct FieldEffect
{
    public EffectType type;
    public float value; // Slowなら倍率(0.8とか)、Poisonなら毎秒ダメージとか
}

public class EffectField : MonoBehaviour
{
    public List<FieldEffect> Effects;
}
