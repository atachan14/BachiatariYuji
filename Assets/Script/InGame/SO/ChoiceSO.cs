using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ChoicesSO", menuName = "NodeSO/ChoicesSO")]
public class ChoiceSO : NodeSO
{
    [TextArea] public string text;

    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public int maxVisibleLines = 1;
    public float charDelay = 0.05f;
    public bool canFastForward = true;


}
