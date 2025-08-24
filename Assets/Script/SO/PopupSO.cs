using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
[System.Serializable]
public class LocalizedText
{
    public SystemLanguage language;
    [TextArea] public string text;
}
[CreateAssetMenu(fileName = "PopupSO", menuName = "NodeSO/PopupSO")]
public class PopupSO : NodeSO
{
    [TextArea] public string text;
    public List<LocalizedText> localizedTexts;
    public float fontSize = 0f;
    public TMP_FontAsset fontAsset;
    public float yOffset = 1f;
    public float lifeTime = 2f;

    public string GetText(SystemLanguage lang)
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