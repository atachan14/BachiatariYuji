using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

// 共通メニュー基底
public abstract class MenuBase : MonoBehaviour
{
    protected bool isActive = false;
    protected int currentIndex = 0;

    protected abstract TextMeshProUGUI[] MenuItems { get; }

    private void Update()
    {
        if (isActive) UpdateMenu();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        isActive = true;
        Debug.Log(isActive);
        OnBeforeShow(); // ← ここで継承側が必要処理できる

        StartCoroutine(FadeIn());
        UpdateSelection();
    }

    protected virtual void OnBeforeShow() { }

    public virtual void Hide()
    {
        isActive = false;
        StartCoroutine(FadeOut());
    }

    protected virtual IEnumerator FadeIn()
    {
        foreach (var item in MenuItems)
        {
            var c = item.color;
            c.a = 0f;
            item.color = c;
        }

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            foreach (var item in MenuItems)
            {
                var c = item.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                item.color = c;
            }

            yield return null;
        }
    }

    protected virtual IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float time = 0f;

        Color[] startColors = new Color[MenuItems.Length];
        for (int i = 0; i < MenuItems.Length; i++)
            startColors[i] = MenuItems[i].color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            for (int i = 0; i < MenuItems.Length; i++)
            {
                var c = startColors[i];
                c.a = Mathf.Lerp(c.a, 0f, t);
                MenuItems[i].color = c;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    // 入力処理は共通化
    public void UpdateMenu()
    {
        if (!isActive || MenuItems.Length == 0) return;

        bool moved = false;

        if (InputReceiver.Instance.Down || InputReceiver.Instance.Right)
        {
            currentIndex = (currentIndex + 1) % MenuItems.Length;
            moved = true;
        }
        else if (InputReceiver.Instance.Up || InputReceiver.Instance.Left)
        {
            currentIndex = (currentIndex - 1 + MenuItems.Length) % MenuItems.Length;
            moved = true;
        }

        if (moved)
            UpdateSelection();

        if (InputReceiver.Instance.Confirm)
            OnConfirm(currentIndex);
    }

    // 選択中のハイライト更新
    protected virtual void UpdateSelection()
    {
        for (int i = 0; i < MenuItems.Length; i++)
            MenuItems[i].fontStyle = (i == currentIndex) ? FontStyles.Bold : FontStyles.Normal;
    }

    // 決定時の処理は継承先で実装
    protected abstract void OnConfirm(int index);
}
