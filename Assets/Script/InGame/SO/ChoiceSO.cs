using TMPro;
using UnityEngine;

[System.Serializable]
public class ChoiceData
{
    public string text;           // �I�����̕\�����e
    public BaseNode nextNode;     // �I�񂾂��Ԑ�
}

[CreateAssetMenu(fileName = "ChoicesSO", menuName = "NodeSO/ChoicesSO")]
public class ChoiceSO : ScriptableObject
{
    public string text;
    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public float charDelay = 0.05f;
    public bool canFastForward = true;

    public ChoiceData[] choices;


}
