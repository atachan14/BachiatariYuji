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
        ApplyLocalization(); // �����\��
    }

    void Update()
    {
        // ����؂�ւ����f
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
        // ���ʃt�F�[�h�A�E�g����
        yield return StartCoroutine(FadeOutOthers(index));

        // �ʏ����Ɉڍs
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
                // ��I�����ڂ͓�������n�߂�
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
                if (i == keepIndex) continue; // �I�𒆂͂��̂܂܎c��
                Color c = menuItems[i].color;
                c.a = Mathf.Lerp(0f, 1f, t);
                menuItems[i].color = c;
            }

            yield return null;
        }

        // �O�̂��ߍŏI�l�␳
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
        TitleManager.Instance.SwitchMode(TitleMode.StartGame); // �Q�[���J�n���o�Ƃ�
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
