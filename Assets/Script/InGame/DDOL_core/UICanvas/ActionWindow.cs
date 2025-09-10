using UnityEngine;
using UnityEngine.UI;

public class ActionWindow : SingletonMonoBehaviour<ActionWindow>
{
    [SerializeField] Image icon;
    [SerializeField] Sprite notIcon;
    public void UpdateIcon()
    {
        if (YujiDoActioner.Instance.CanActionInRange.Count <= 0)
        {
            icon.sprite = notIcon;
        }
        else
        {
            icon.sprite = YujiDoActioner.Instance.CanActionInRange[0].GetIcon();
        }

    }
}
