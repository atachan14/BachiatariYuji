
[System.Serializable]
public class TimeEffect
{
     public EffectType Type;
    public float Value;       // �ő�l
    public float Time;        // �c�莞��
    public float Duration;    // �ŏ���Time�i���v���ԁj

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

    // �c�莞�Ԃɉ��������ۗ�
    public float CurrentValue => Value * (Time / Duration);
}