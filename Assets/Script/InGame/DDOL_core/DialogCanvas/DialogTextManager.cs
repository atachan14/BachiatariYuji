using System.Collections;
using TMPro;
using UnityEngine;
[System.Serializable]
public class TalkData
{
    public string text;
    public TMP_FontAsset fontAsset;
    public float fontSize;
    public int maxVisibleLines;
    public float charDelay;
    public bool canFastForward;

    // SO から生成するコンストラクタ
    public TalkData(TalkSO so)
    {
        text = so.GetText(OptionData.Instance.Language);
        fontAsset = so.fontAsset;
        fontSize = so.fontSize;
        maxVisibleLines = so.maxVisibleLines;
        charDelay = so.charDelay;
        canFastForward = so.canFastForward;
    }

    // 文字列だけで作る場合のコンストラクタ
    public TalkData(string t, TMP_FontAsset font = null, float size = 50f, int lines = 3, float delay = 0.05f, bool fastForward = true)
    {
        text = t;
        fontAsset = font;
        fontSize = size;
        maxVisibleLines = lines;
        charDelay = delay;
        canFastForward = fastForward;
    }
}
public class DialogTextManager : SingletonMonoBehaviour<DialogTextManager>
{

    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private GameObject nextCursor;
    [SerializeField] private float fastForwardRatio = 20f;

    private bool isFastForward;

    public IEnumerator PlayTextRoutine(TalkData data)
    {
        tmp.font = data.fontAsset ?? defaultFontAsset;
        tmp.fontSize = data.fontSize;
        tmp.text = "";
        isFastForward = false;

        foreach (char c in data.text)
        {
            yield return AppendCharWithScroll(c, data.maxVisibleLines);
            yield return WaitForCharDelay(data.charDelay, data.canFastForward);
        }
    }

    // 既存のSO用オーバーロード
    public IEnumerator PlayTextRoutine(TalkSO so)
    {
        return PlayTextRoutine(new TalkData(so));
    }
    public IEnumerator PlayTextRoutine(string text)
    {
        return PlayTextRoutine(new TalkData(text));
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
            if (InputReceiver.Instance.Confirm && canFastForward)
                isFastForward = true;

            float dt = Time.deltaTime * (isFastForward ? fastForwardRatio : 1f);
            timer += dt;
            yield return null;
        }
    }

    public IEnumerator WaitNextPress()
    {
        nextCursor.SetActive(true);
        yield return new WaitUntil(() => InputReceiver.Instance.Confirm);
        nextCursor.SetActive(false);
        InputReceiver.Instance.Confirm = false;
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

    public void ResetText()
    {
        tmp.text = "";
        tmp.ForceMeshUpdate();
        nextCursor.SetActive(false);
        isFastForward = false;
    }


}
