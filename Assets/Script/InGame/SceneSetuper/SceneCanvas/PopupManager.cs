using UnityEngine;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    [SerializeField] private GameObject popupPrefab;

    public void ShowPopup( PopupSO talk, Transform target)
    {
        var popup = Instantiate(popupPrefab, transform);
        popup.GetComponent<PopupItem>().Init(target, talk);
    }
}