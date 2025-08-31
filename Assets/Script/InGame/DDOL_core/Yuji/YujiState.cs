using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class YujiState : SingletonMonoBehaviour<YujiState>
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float MaxHelth { get; private set; }
    [field: SerializeField] public float Helth { get; private set; }
    [field: SerializeField] public float FixDef { get; private set; }
    [field: SerializeField] public float PerDef { get; private set; }
    [field: SerializeField] public float CcDef { get; private set; }
    [field: SerializeField] public float Vision { get; private set; }

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
    }

    void ApplyInventry()
    {
        // Inventry‚ðŽÀ‘•‚·‚é‚Ü‚Å•ú’uB
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

                    case EffectType.MaxHP:
                        MaxHelth += e.value;
                        if (Helth > MaxHelth) Helth = MaxHelth;
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
    void ApplyTimeEffects()
    {
        //TimeEffectŽÀ‘•‚Ü‚Å•ú’u
    }
}
