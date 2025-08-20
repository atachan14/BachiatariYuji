using UnityEngine;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    [SerializeField] private GameObject popupPrefab;

    public void ShowPopup( PopupData talk, Transform target)
    {
        var popup = Instantiate(popupPrefab, transform);
        popup.GetComponent<PopupUI>().Init(target, talk);
    }
}