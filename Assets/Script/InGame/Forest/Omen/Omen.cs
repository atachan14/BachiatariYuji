using System.Collections.Generic;
using UnityEngine;
public enum LayerName
{
    SmallAnimal,MiddleAnimal,LargeAnimal
}
public class Omen : MonoBehaviour
{
    public OmenDestinySO destiny;
    [SerializeField] List<GameObject> punishes;


    public void SelectPunish()
    {
        var selectedPunish = OmenManager.Instance.PunishDestinyPick(punishes);
        selectedPunish.SetActive(true);
    }

  
}
