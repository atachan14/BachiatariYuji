using System.Collections.Generic;
using UnityEngine;
public enum LayerName
{
    SmallUnit,MiddleUnit,LargeUnit,EffectField
}
public class Omen : MonoBehaviour
{
    public OmenDestinySO destiny;
    [SerializeField] List<GameObject> punishes;


    public void SelectPunish()
    {
        var selectedPunish = OmenManager.Instance.PunishDestinyPick(punishes);
        if (selectedPunish != null)
        selectedPunish.SetActive(true);
    }

  
}
