using TMPro;
using UnityEditor;
using UnityEngine;

public class AchieveMenuController : MenuBase
{
    public TextMeshProUGUI[] menuItems;
    protected override TextMeshProUGUI[] MenuItems => menuItems;

    protected override void OnConfirm(int index)
    {
    }
}
