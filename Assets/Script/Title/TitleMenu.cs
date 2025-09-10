using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleMenuController : MenuBase
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI continueText;
    public TextMeshProUGUI achieveText;
    public TextMeshProUGUI languageText;

    [Header("Localized Texts")]
    public List<LocalizedText> startLocalized;
    public List<LocalizedText> continueLocalized;
    public List<LocalizedText> achieveLocalized;
    public List<LocalizedText> languageLocalized;

    private TextMeshProUGUI[] menuItemsArray;
    protected override TextMeshProUGUI[] MenuItems => menuItemsArray;

    private void Awake()
    {
        menuItemsArray = new[] { startText, continueText, achieveText, languageText };
    }

    private void Start()
    {
        ApplyLocalization();
        UpdateSelection();
    }
    protected override void OnBeforeShow()
    {
        ApplyLocalization(); // Showのタイミングで最新言語に反映
    }

    private void ApplyLocalization()
    {
        var lang = OptionData.Instance.Language;
        startText.text = startLocalized.GetText(lang);
        continueText.text = continueLocalized.GetText(lang);
        achieveText.text = achieveLocalized.GetText(lang);
        languageText.text = languageLocalized.GetText(lang);
    }

    protected override void OnConfirm(int index)
    {
        switch (index)
        {
            case 0: TitleManager.Instance.StartGame(); break;
            case 1: TitleManager.Instance.ContinueGame(); break;
            case 2: TitleManager.Instance.ShowArchiveMenu(); break;
            case 3: TitleManager.Instance.ShowLanguageMenu(); break;
        }
    }
}
