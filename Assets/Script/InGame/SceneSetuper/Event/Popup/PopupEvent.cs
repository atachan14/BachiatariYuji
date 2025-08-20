using TMPro;
using UnityEngine;

public class PopupEvent : EventBase
{
    public PopupData[] talks;

    public override void DoEvent()
    {
        PopupManager.Instance.ShowPopup(talks[0], transform);
        Debug.Log("talk");
    }
}