using TMPro;

public class LanguageMenuController : MenuBase
{
    public TextMeshProUGUI[] menuItems;

    protected override TextMeshProUGUI[] MenuItems => menuItems;

    private void Start()
    {
        UpdateSelection();
    }

    protected override void OnConfirm(int index)
    {
        OptionData.Instance.Language = (index == 0) ? Language.JP : Language.EN;
        TitleManager.Instance.ShowTitleMenu();
        Hide(); // ‘I‘ğŒã‚Íƒƒjƒ…[‚ğ•Â‚¶‚é
    }
}
