using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StealSizeSO", menuName = "Scriptable Objects/StealSizeSO")]
public class StealSizeSO : ScriptableObject
{
    public StealSize size;
    public List<LocalizedText> localizedTexts;
    public AnimationCurve distributionCurve; // 0~1 ‚ğ“ü—Í‚É‚µ‚Äd‚İ‚ğ•Ô‚·
    public int id;
}
