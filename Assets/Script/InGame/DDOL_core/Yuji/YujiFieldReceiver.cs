using System.Collections.Generic;
using UnityEngine;

public class YujiFieldReceiver : SingletonMonoBehaviour<YujiFieldReceiver>
{
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerName.EffectField.ToString()))
        {
            var field = other.GetComponent<EffectField>();
            if (field == null)
                Debug.LogError("[YujiFieldReceiver] EffectField Layerなのにコンポーネントが無い！");
            else
                YujiState.Instance.activeFieldEffects.Add(field);
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
                YujiState.Instance.activeFieldEffects.Remove(field);
        }
    }
}