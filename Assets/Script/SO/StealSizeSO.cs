using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StealSizeSO", menuName = "Scriptable Objects/StealSizeSO")]
public class StealSizeSO : ScriptableObject
{
    public StealSize size;
    public List<LocalizedText> localizedTexts;
    [Range(0f, 1f)]
    public float medianRate; // �����l�̊����iMaxSteal�ɑ΂���䗦�j

    public int id;
}
