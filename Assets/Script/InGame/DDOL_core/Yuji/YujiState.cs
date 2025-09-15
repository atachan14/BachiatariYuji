using System.Collections.Generic;
using UnityEngine;
public enum YujiCondition
{
    Normal,   // �ʏ�
    Sleeping, // �����Ă�
    Falling,  // ������
}
public class YujiState : SingletonMonoBehaviour<YujiState>
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float FixDef { get; private set; }
    [field: SerializeField] public float PerDef { get; private set; }
    [field: SerializeField] public float DetoxPower { get; private set; }
    [field: SerializeField] public float Vision { get; private set; }
    [field: SerializeField] public float Hallucinations { get; set; }
    [field: SerializeField] public float DayDrowsiness { get; set; } = 0;
    [field: SerializeField] public float Drowsiness { get; set; }

    public List<EffectField> activeFieldEffects = new();
    public List<PoisonEffect> activePoisons = new();


    private void Update()
    {
        LoadParams();
        ApplyInventry();
        ApplyFieldEffects();
        ApplyPoisons();
        ApplyConditions();
    }

    void LoadParams()
    {
        MoveSpeed = YujiParams.Instance.MoveSpeed;
        FixDef = YujiParams.Instance.FixDef;
        PerDef = YujiParams.Instance.PerDef;
        DetoxPower = YujiParams.Instance.DetoxPower;
        Vision = YujiParams.Instance.Vision;
        Hallucinations = 0;
        Drowsiness = DayDrowsiness;
    }

    void ApplyInventry()
    {
        // Inventry����������܂ŕ��u�B
    }

    void ApplyFieldEffects()
    {
        foreach (var effect in activeFieldEffects)
        {
            foreach (var e in effect.Effects)
            {
                switch (e.type)
                {
                    case EffectType.Slow:
                        MoveSpeed *= (1 - e.value);
                        break;


                    case EffectType.FixDef:
                        FixDef += e.value;
                        break;

                    case EffectType.PerDef:
                        PerDef += e.value;
                        break;

                    case EffectType.DetoxPower:
                        DetoxPower += e.value;
                        break;

                    case EffectType.Vision:
                        Vision += e.value;
                        break;
                }
            }
        }
    }
    private void ApplyPoisons()
    {
        float dt = Time.deltaTime;

        for (int i = activePoisons.Count - 1; i >= 0; i--)
        {
            PoisonEffect effect = activePoisons[i];

            if (effect.Tick(dt))
            {
                activePoisons.RemoveAt(i);
            }
            else
            {
                effect.Apply(dt); // State��n���ēŏ�����PoisonEffect���ɔC����
            }
        }
    }

    private void ApplyConditions()
    {
        YujiSleeper.Instance.Apply();
    }
    public void TakePosion(PoisonEffect effect)
    {
        activePoisons.Add(effect);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerName.EffectField.ToString()))
        {
            var field = other.GetComponent<EffectField>();
            if (field == null)
                Debug.LogError("[YujiFieldReceiver] EffectField Layer�Ȃ̂ɃR���|�[�l���g�������I");
            else
                activeFieldEffects.Add(field);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerName.EffectField.ToString()))
        {
            var field = other.GetComponent<EffectField>();
            if (field == null)
                Debug.LogError("[YujiFieldReceiver] EffectField Layer�Ȃ̂ɃR���|�[�l���g�������I");
            else
                activeFieldEffects.Remove(field);
        }
    }
}
