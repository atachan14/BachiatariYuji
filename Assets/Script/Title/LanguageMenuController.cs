using UnityEngine;
using TMPro;
using System.Collections;

public class LanguageMenuController : MonoBehaviour
{
    public TextMeshProUGUI japaneseText;
    public TextMeshProUGUI englishText;

    private TextMeshProUGUI[] menuItems;
    private int currentIndex = 0;
    private bool isActive = false;

    private void Start()
    {
        menuItems = new TextMeshProUGUI[] { japaneseText, englishText };
        UpdateSelection();
        gameObject.SetActive(false); // 初期は非表示
    }

    private void Update()
    {
        if (!isActive) return;

        if (InputReceiver.Instance.Up 
            || InputReceiver.Instance.Down
            || InputReceiver.Instance.Left
            || InputReceiver.Instance.Right)
        {
            currentIndex = 1 - currentIndex; // 2択なので切り替え
            UpdateSelection();
        }

        if (InputReceiver.Instance.Confirm)
        {
            ApplyLanguage(currentIndex);
            TitleManager.Instance.SwitchMode(TitleMode.TitleMenu);
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].fontStyle = (i == currentIndex) ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    private void ApplyLanguage(int index)
    {
        isActive = false;
        if (index == 0)
        {
            OptionData.Instance.Language = Language.JP;
        }
        else
        {
            OptionData.Instance.Language = Language.EN;
        }
        StartCoroutine(FadeOutAndHide());
    }

    private IEnumerator FadeInAll()
    {
        float duration = 0.5f;
        float time = 0f;

        // 開始時は透明
        foreach (var item in menuItems)
        {
            var c = item.color;
            c.a = 0f;
            item.color = c;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            foreach (var item in menuItems)
            {
                var c = item.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                item.color = c;
            }

            yield return null;
        }
    }

    private IEnumerator FadeOutAndHide()
    {
        isActive = false;
        float duration = 0.5f;
        float time = 0f;

        Color[] startColors = new Color[menuItems.Length];
        for (int i = 0; i < menuItems.Length; i++)
            startColors[i] = menuItems[i].color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            for (int i = 0; i < menuItems.Length; i++)
            {
                var c = startColors[i];
                c.a = Mathf.Lerp(1f, 0f, t);
                menuItems[i].color = c;
            }

            yield return null;
        }

        // 最後に透明＆非表示
        foreach (var item in menuItems)
        {
            var c = item.color;
            c.a = 0f;
            item.color = c;
        }
        gameObject.SetActive(false);
    }

    public void Show()
    {
        isActive = true;
        gameObject.SetActive(true);
        StartCoroutine(FadeInAll());
    }
}
