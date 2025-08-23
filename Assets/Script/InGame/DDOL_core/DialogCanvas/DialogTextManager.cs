using System.Collections;
using TMPro;
using UnityEngine;

public class DialogTextManager : SingletonMonoBehaviour<DialogTextManager>
{

    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private GameObject nextCursor;
    [SerializeField] private float fastForwardRatio = 20f;

    private bool isFastForward;

    public IEnumerator PlayTextRoutine(TalkSO so)
    {
        tmp.font = so.fontAsset ?? defaultFontAsset;
        tmp.fontSize = so.fontSize;
        tmp.text = "";
        isFastForward = false;

        foreach (char c in so.text)
        {
            yield return AppendCharWithScroll(c, so.maxVisibleLines);

            // 文字ごとのディレイ
            yield return WaitForCharDelay(so.charDelay, so.canFastForward);
        }

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

}
