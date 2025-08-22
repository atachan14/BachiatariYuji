using System.Collections;
using System.Data;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct DialogTextData
{
    public string text;
    public TMP_FontAsset fontAsset;
    public float fontSize;
    public float charDelay;
    public int maxVisibleLines;
    public bool canFastForward;

    // TalkNodeから作る
    public DialogTextData(TalkNode node)
    {
        var so = node.so;
        text = string.Join("\n", so.text); // 配列ならまとめて1つの文字列に
        fontAsset = so.fontAsset;
        fontSize = so.fontSize;
        charDelay = so.charDelay;
        maxVisibleLines = so.maxVisibleLines;
        canFastForward = so.canFastForward;
    }

    // ChoiceNodeから作る
    public DialogTextData(ChoiceNode node)
    {
        var so = node.so;
        text = so.text;
        fontAsset = so.fontAsset;
        fontSize = so.fontSize;
        charDelay = so.charDelay;
        maxVisibleLines = 1;
        canFastForward = so.canFastForward;
    }
}



public class DialogManager : SingletonMonoBehaviour<DialogManager>
{
    [SerializeField] private GameObject dialogWindow;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private GameObject nextCursor;
    [SerializeField] private float fastForwardRatio = 20f;

    private bool isFastForward;

    public IEnumerator PlayTextRoutine(DialogTextData data)
    {
        EnterDialogMode();
        tmp.font = data.fontAsset ?? defaultFontAsset;
        tmp.fontSize = data.fontSize;
        tmp.text = "";
        isFastForward = false;

        foreach (char c in data.text)
        {
            yield return AppendCharWithScroll(c, data.maxVisibleLines);

            // 文字ごとのディレイ
            yield return WaitForCharDelay(data.charDelay, data.canFastForward);
        }

        // ページクリア待ち
        yield return WaitNextPress();

        ExitDialogMode();
    }

    // ======= 小分けメソッド =======
    private void EnterDialogMode()
    {
        dialogWindow.SetActive(true);
        InputReceiver.Instance.SwitchMode(InputMode.Dialog);
    }

    private void ExitDialogMode()
    {
        dialogWindow.SetActive(false);
        InputReceiver.Instance.SwitchMode(SceneSetuper.Instance.GetInputMode());
    }

    private IEnumerator AppendCharWithScroll(char c, int maxVisibleLines)
    {
        tmp.text += c;
        tmp.ForceMeshUpdate();

        if (tmp.textInfo.lineCount > maxVisibleLines)
        {
            // 直前追加を戻す
            tmp.text = tmp.text.Substring(0, tmp.text.Length - 1);
            tmp.ForceMeshUpdate();

            // 入力待ち
            yield return WaitNextPress();

            // スクロールして文字を再追加
            ScrollOffTopLine();
            tmp.text += c;
            tmp.ForceMeshUpdate();
        }
    }

    private IEnumerator WaitForCharDelay(float delay, bool canFastForward)
    {
        float timer = 0f;
        while (timer < delay)
        {
            if (InputReceiver.Instance.NextTalk && canFastForward)
                isFastForward = true;

            float dt = Time.deltaTime * (isFastForward ? fastForwardRatio : 1f);
            timer += dt;
            yield return null;
        }
    }

    private IEnumerator WaitNextPress()
    {
        nextCursor.SetActive(true);
        yield return new WaitUntil(() => InputReceiver.Instance.NextTalk);
        nextCursor.SetActive(false);
        InputReceiver.Instance.NextTalk = false;
        isFastForward = false;
    }

    private void ScrollOffTopLine()
    {
        var ti = tmp.textInfo;
        if (ti.lineCount == 0) return;

        var l0 = ti.lineInfo[0];
        int cutStart = l0.firstCharacterIndex;
        int cutEndInclusive = l0.lastCharacterIndex;
        int cutLen = cutEndInclusive - cutStart + 1;

        string t = tmp.text;

        int nextIdx = cutEndInclusive + 1;
        if (nextIdx < t.Length && t[nextIdx] == '\n')
            cutLen++;

        string rest = t.Substring(cutStart + cutLen);
        tmp.text = rest.TrimStart('\n', '\r');
    }
}
