using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "PopupSO", menuName = "NodeSO/PopupSO")]
public class PopupSO : NodeSO
{
    [TextArea] public string text;
    public float fontSize = 20f;
    public TMP_FontAsset fontAsset;
    public float yOffset = 1f;
    public float lifeTime = 3f;
}