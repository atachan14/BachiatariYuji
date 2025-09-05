using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TitleMenuController : MonoBehaviour
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

    private TextMeshProUGUI[] menuItems;
    private int currentIndex = 0;

    void Start()
    {
        menuItems = new TextMeshProUGUI[] { startText, continueText, achieveText, languageText };
        UpdateSelection();
        ApplyLocalization(); // 初期表示
    }

    void Update()
    {
        // 言語切り替え反映
        ApplyLocalization();

        if (TitleManager.Instance.currentMode != TitleMode.TitleMenu) return;

        if (InputReceiver.Instance.Up)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menuItems.Length - 1;
            UpdateSelection();
        }

        if (InputReceiver.Instance.Down)
        {
            currentIndex++;
            if (currentIndex >= menuItems.Length) currentIndex = 0;
            UpdateSelection();
        }

        if (InputReceiver.Instance.Confirm)
        {
            StartCoroutine(ExecuteSelectionCoroutine(currentIndex));
        }
    }

    private void ApplyLocalization()
    {
        var lang = OptionData.Instance.Language;
        startText.text = startLocalized.GetText(lang);
        continueText.text = continueLocalized.GetText(lang);
        achieveText.text = achieveLocalized.GetText(lang);
        languageText.text = languageLocalized.GetText(lang);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].fontStyle = (i == currentIndex) ? FontStyles.Bold : FontStyles.Normal;
        }
    }


    private IEnumerator ExecuteSelectionCoroutine(int index)
    {
        TitleManager.Instance.SwitchMode(TitleMode.Wait);
        // 共通フェードアウト処理
        yield return StartCoroutine(FadeOutOthers(index));

        // 個別処理に移行
        switch (index)
        {
            case 0:
                StartGame();
                break;
            case 1:
                ContinueGame();
                break;
            case 2:
                Achieve();
                break;
            case 3:
                Language();
                break;
        }
    }

    private IEnumerator FadeOutOthers(int keepIndex)
    {
        float duration = 1f;
        float time = 0f;

        Color[] startColors = new Color[menuItems.Length];
        for (int i = 0; i < menuItems.Length; i++)
        {
            startColors[i] = menuItems[i].color;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == keepIndex) continue;
                Color c = startColors[i];
                c.a = Mathf.Lerp(1f, 0f, t);
                menuItems[i].color = c;
            }

            yield return null;
        }

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == keepIndex) continue;
            Color c = menuItems[i].color;
            c.a = 0f;
            menuItems[i].color = c;
        }
    }

    public void Show()
    {
        StartCoroutine(FadeInOthers(currentIndex));
    }

    private IEnumerator FadeInOthers(int keepIndex)
    {
        TitleManager.Instance.currentMode= TitleMode.Wait;
        float duration = 1f;
        float time = 0f;

        Color[] startColors = new Color[menuItems.Length];
        for (int i = 0; i < menuItems.Length; i++)
        {
            startColors[i] = menuItems[i].color;
            if (i != keepIndex)
            {
                // 非選択項目は透明から始める
                Color c = menuItems[i].color;
                c.a = 0f;
                menuItems[i].color = c;
            }
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == keepIndex) continue; // 選択中はそのまま残す
                Color c = menuItems[i].color;
                c.a = Mathf.Lerp(0f, 1f, t);
                menuItems[i].color = c;
            }

            yield return null;
        }

        // 念のため最終値補正
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == keepIndex) continue;
            Color c = menuItems[i].color;
            c.a = 1f;
            menuItems[i].color = c;
        }
        TitleManager.Instance.currentMode=TitleMode.TitleMenu;
    }


    private void StartGame()
    {
        Debug.Log("Start Game!");
        TitleManager.Instance.SwitchMode(TitleMode.StartGame); // ゲーム開始演出とか
    }

    private void ContinueGame()
    {
        Debug.Log("Continue Game!");
    }

    private void Achieve()
    {
        Debug.Log("Achievements!");
        TitleManager.Instance.SwitchMode(TitleMode.AchieveMenu);
    }

    private void Language()
    {
        Debug.Log("Language Settings!");
        TitleManager.Instance.SwitchMode(TitleMode.LanguageMenu);
    }
}
