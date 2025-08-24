using System.Collections.Generic;
using System.Linq;

public static class LocalizedHelper
{
    public static string GetText(this List<LocalizedText> localizedTexts, Language lang)
    {
        var entry = localizedTexts.FirstOrDefault(l => l.language == lang);
        if (entry != null && !string.IsNullOrEmpty(entry.text))
            return entry.text;

        // フォールバック: 日本語 or 先頭のやつ
        var fallback = localizedTexts.FirstOrDefault(l => l.language == Language.JP);
        if (fallback != null)
            return fallback.text;

        return localizedTexts.FirstOrDefault()?.text ?? string.Empty;
    }
}