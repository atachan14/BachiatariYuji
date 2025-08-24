using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "PopupSO", menuName = "NodeSO/PopupSO")]
public class PopupSO : NodeSO
{
    public List<LocalizedText> localizedTexts;
    public float fontSize = 0f;
    public TMP_FontAsset fontAsset;
    public float yOffset = 1f;
    public float lifeTime = 2f;

    public string GetText(Language lang)
    {
        var entry = localizedTexts.Find(l => l.language == lang);
        if (!string.IsNullOrEmpty(entry.text))
            return entry.text;

        // �t�H�[���o�b�N: ���{�� or �擪�̂��
        if (localizedTexts.Count > 0)
            return localizedTexts[0].text;

        return string.Empty;
    }
}