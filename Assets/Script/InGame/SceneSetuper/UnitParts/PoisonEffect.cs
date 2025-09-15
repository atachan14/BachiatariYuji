using UnityEngine;

[System.Serializable]
public class PoisonEffect
{
    public EffectType Type;
    public AnimationCurve Curve;
    public float MaxValue;
    public float Time;
    public float Duration;

    public PoisonEffect(EffectType type, AnimationCurve curve, float maxValue, float duration)
    {
        Type = type;
        Curve = curve;
        MaxValue = maxValue;
        Duration = duration;
        Time = duration;
    }

    public bool Tick(float deltaTime)
    {
        Time -= deltaTime;
        return Time <= 0f;
    }

    public float CurrentValue
    {
        get
        {
            if (Duration <= 0f) return 0f;
            float t = 1f - (Time / Duration);
            return MaxValue * Curve.Evaluate(t);
        }
    }

    public void Apply( float deltaTime)
    {
        switch (Type)
        {
            case EffectType.Hallucinations:
                YujiState.Instance.Hallucinations += CurrentValue;
                break;
            case EffectType.Drowsiness:
                YujiState.Instance.Drowsiness += CurrentValue;
                break;
                // ‘¼‚Ì“Åƒ^ƒCƒv‚à‚±‚±‚É’Ç‰Á
        }
    }
}
