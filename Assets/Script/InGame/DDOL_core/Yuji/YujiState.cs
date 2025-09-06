using System.Collections.Generic;
using UnityEngine;

public class YujiState : SingletonMonoBehaviour<YujiState>
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float MaxHelth { get; private set; }
    [field: SerializeField] public float FixDef { get; private set; }
    [field: SerializeField] public float PerDef { get; private set; }
    [field: SerializeField] public float CcDef { get; private set; }
    [field: SerializeField] public float Vision { get; private set; }
    [field: SerializeField] public float Hallucinations { get; private set; }

    public List<EffectField> activeFieldEffects;
    public List<TimeEffect> activeTimeEffect;

    private void Update()
    {
        LoadParams();
        ApplyInventry();
        ApplyFieldEffects();
        ApplyTimeEffects();
    }

    void LoadParams()
    {
        MoveSpeed = YujiParams.Instance.MoveSpeed;
        MaxHelth = YujiParams.Instance.MaxHelth;
        FixDef = YujiParams.Instance.FixDef;
        PerDef = YujiParams.Instance.PerDef;
        CcDef = YujiParams.Instance.CcDef;
        Vision = YujiParams.Instance.Vision;
        Hallucinations = YujiParams.Instance.Hallucinations;
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

                    case EffectType.CcDef:
                        CcDef += e.value;
                        break;

                    case EffectType.Vision:
                        Vision += e.value;
                        break;
                }
            }
        }
    }
    private void ApplyTimeEffects()
    {
        float dt = Time.deltaTime;
        for (int i = activeTimeEffect.Count - 1; i >= 0; i--)
        {
            if (activeTimeEffect[i].Tick(dt))
            {
                // ���ʏI�������烊�X�g����폜
                activeTimeEffect.RemoveAt(i);
            }
            else
            {
                // ���ʓK�p
                ApplyTimeEffect(activeTimeEffect[i]);
            }
        }
    }

    private void ApplyTimeEffect(TimeEffect effect)
    {
        switch (effect.Type)
        {
            case EffectType.Slow:
                MoveSpeed *= (1 - effect.CurrentValue);
                break;
            case EffectType.Hallucinations:
                Hallucinations += effect.CurrentValue;
                break;
                // �������l
        }
    }

    public void TakeTimeEffect(TimeEffect effect)
    {
        activeTimeEffect.Add(effect);
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
