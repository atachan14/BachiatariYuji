using UnityEngine;

public class PopupManager : DDOL_child<PopupManager>
{
    [SerializeField] private GameObject popupPrefab;

    public void ShowPopup(Transform target, TalkData talk)
    {
        var popup = Instantiate(popupPrefab, transform);
        popup.GetComponent<PopupUI>().Init(target, talk);
    }
}