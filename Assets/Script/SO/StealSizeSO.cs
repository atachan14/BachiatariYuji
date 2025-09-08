using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StealSizeSO", menuName = "Scriptable Objects/StealSizeSO")]
public class StealSizeSO : ScriptableObject
{
    public StealSize size;
    public List<LocalizedText> localizedTexts;
    public AnimationCurve distributionCurve; // 0~1 ����͂ɂ��ďd�݂�Ԃ�
    public int id;
}
