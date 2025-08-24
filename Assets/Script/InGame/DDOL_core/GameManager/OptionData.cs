using UnityEngine;

public enum Language{ JP,EN }

[System.Serializable]
public class LocalizedText
{
    public Language language;
    [TextArea] public string text;
}

public class OptionData : SingletonMonoBehaviour<OptionData>
{
    
    public Language Language = Language.JP;
}
