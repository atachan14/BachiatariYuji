using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkSO", menuName = "NodeSO/TalkSO")]
public class TalkSO : NodeSO
{
    public List<LocalizedText> localizedTexts;

    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public int maxVisibleLines = 3;
    public float charDelay = 0.05f;
    public bool canFastForward = true;

    public string GetText(Language lang)
    {
        var entry = localizedTexts.Find(l => l.language == lang);
        if (!string.IsNullOrEmpty(entry.text))
            return entry.text;

        // フォールバック: 日本語 or 先頭のやつ
        if (localizedTexts.Count > 0)
            return localizedTexts[0].text;

        return string.Empty;
    }
}
