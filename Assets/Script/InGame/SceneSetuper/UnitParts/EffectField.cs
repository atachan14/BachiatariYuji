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
    public float value; // Slow�Ȃ�{��(0.8�Ƃ�)�APoison�Ȃ疈�b�_���[�W�Ƃ�
}

public class EffectField : MonoBehaviour
{
    public List<FieldEffect> Effects;
}
