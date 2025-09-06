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
        // Inventryを実装するまで放置。
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
                // 効果終了したらリストから削除
                activeTimeEffect.RemoveAt(i);
            }
            else
            {
                // 効果適用
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
                // 他も同様
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
                Debug.LogError("[YujiFieldReceiver] EffectField Layerなのにコンポーネントが無い！");
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
                Debug.LogError("[YujiFieldReceiver] EffectField Layerなのにコンポーネントが無い！");
            else
                activeFieldEffects.Remove(field);
        }
    }
}
