using UnityEngine;

public enum Language { JP, EN, FR }

public class OptionData : MonoBehaviour
{
    
    [SerializeField] Language language = Language.JP;
}
