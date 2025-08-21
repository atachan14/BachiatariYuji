using System.Collections;
using UnityEngine;
using TMPro;

public class TalkManager : SingletonMonoBehaviour<TalkManager>
{
    [SerializeField] private GameObject DialogWindow;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private GameObject nextCursor;
    [SerializeField] private float fastForwardRatio = 20f;

    private TalkSO currentData;
    private int textIndex;
    private bool isFastForward = false;


    // ===== Public API =====
    public void ShowUnder(TalkSO data)
    {
        currentData = data;
        tmp.fontSize = currentData.fontSize;
        tmp.font = data.fontAsset ?? defaultFontAsset;

        StopAllCoroutines();
        StartCoroutine(PlayTalkRoutine());
    }

    // ===== Driver =====
    private IEnumerator PlayTalkRoutine()
    {
        EnterUnderTalkMode();

        for (textIndex = 0; textIndex < currentData.text.Length; textIndex++)
        {
            yield return StartCoroutine(PlayOneElement(currentData.text[textIndex]));

            // 要素の終わりでワンテンポ待つ → ページクリア
            yield return WaitNextPress();
            SetText("");
        }

        ExitUnderTalkMode();

        if (currentData.nextEvent != null)
            currentData.nextEvent.PlayNode();
    }

    private IEnumerator PlayOneElement(string fullText)
    {
        isFastForward = false;

        foreach (char c in fullText)
        {
            // 文字追加（オーバー時は自動で待ち＆スクロール）
            yield return TryAppendChar(c);

            // 文字ごとのディレイ（早送り対応）
            yield return WaitForCharDelay(currentData.charDelay);
        }

    }

    // 1文字追加して行オーバー処理
    private IEnumerator TryAppendChar(char c)
    {
        AppendChar(c);
        tmp.ForceMeshUpdate();

        // 行数オーバー？
        if (tmp.textInfo.lineCount > currentData.maxVisibleLines)
        {
            // 直前の追加を戻す
            tmp.text = tmp.text.Substring(0, tmp.text.Length - 1);
            tmp.ForceMeshUpdate();

            // 入力待ち
            yield return WaitNextPress();

            // スクロールしてから文字を再追加
            ScrollOffTopLine();
            AppendChar(c);
        }
    }

    // 秒数待機中も毎フレチェックして早送り
    private IEnumerator WaitForCharDelay(float delay)
    {
        float timer = 0f;
        while (timer < delay)
        {
            // 押されてたら早送りに切り替え
            if (InputReceiver.Instance.NextTalk && currentData.canFastForward)
                isFastForward = true;

            float dt = Time.deltaTime;
            if (isFastForward)
                dt *= fastForwardRatio; // 早送り速度調整（ここは1fでもOK）

            timer += dt;
            yield return null;
        }
    }



    // ===== Helpers =====
    private void EnterUnderTalkMode()
    {
        DialogWindow.SetActive(true);
        SetText("");
        InputReceiver.Instance.SwitchMode(InputMode.UnderTalk);
    }

    private void ExitUnderTalkMode()
    {
        DialogWindow.SetActive(false);
        InputReceiver.Instance.SwitchMode(SceneSetuper.Instance.GetInputMode());
    }

    private void SetText(string s)
    {
        tmp.text = s;
        tmp.ForceMeshUpdate();
    }

    private void AppendChar(char c)
    {
        tmp.text += c;
        tmp.ForceMeshUpdate();
    }

    private IEnumerator WaitNextPress()
    {
        // ▼マークを表示
        nextCursor.SetActive(true);

        // 入力待ち
        yield return new WaitUntil(() => InputReceiver.Instance.NextTalk);

        // ▼マークを非表示
        nextCursor.SetActive(false);

        // 押したフラグをリセット
        InputReceiver.Instance.NextTalk = false;
        isFastForward = false;
    }



    // 先頭の“見えている1行”だけを切り落としてスクロール
    private void ScrollOffTopLine()
    {
        var ti = tmp.textInfo;
        if (ti.lineCount == 0) return;

        var l0 = ti.lineInfo[0];
        int cutStart = l0.firstCharacterIndex;           // たぶん 0
        int cutEndInclusive = l0.lastCharacterIndex;     // 先頭行の最後の可視文字
        int cutLen = cutEndInclusive - cutStart + 1;

        string t = tmp.text;

        // 次の文字が改行ならそれも一緒に落とす（見た目上の改行/自動改行どちらでも問題なし）
        int nextIdx = cutEndInclusive + 1;
        if (nextIdx < t.Length && t[nextIdx] == '\n')
            cutLen++;

        string rest = t.Substring(cutStart + cutLen);
        // 先頭に改行が残ってたら掃除
        rest = rest.TrimStart('\n', '\r');

        SetText(rest);
    }
}
