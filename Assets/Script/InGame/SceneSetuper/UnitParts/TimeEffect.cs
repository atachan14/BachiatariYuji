
[System.Serializable]
public class TimeEffect
{
     public EffectType Type;
    public float Value;       // 最大値
    public float Time;        // 残り時間
    public float Duration;    // 最初のTime（合計時間）

    public TimeEffect(EffectType type, float value, float duration)
    {
        Type = type;
        Value = value;
        Time = duration;
        Duration = duration;
    }

    public bool Tick(float deltaTime)
    {
        Time -= deltaTime;
        return Time <= 0;
    }

    // 残り時間に応じた現象量
    public float CurrentValue => Value * (Time / Duration);
}