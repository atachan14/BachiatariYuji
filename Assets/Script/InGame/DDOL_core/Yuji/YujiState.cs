using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class YujiState : SingletonMonoBehaviour<YujiState>
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float MaxHelth { get; private set; }
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
        // InventryÇé¿ëïÇ∑ÇÈÇ‹Ç≈ï˙íuÅB
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
    void ApplyTimeEffects()
    {
        //TimeEffecté¿ëïÇ‹Ç≈ï˙íu
    }

    
}
