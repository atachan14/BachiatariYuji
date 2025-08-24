using UnityEngine;

[CreateAssetMenu(fileName = "StealSizeSO", menuName = "Scriptable Objects/StealSizeSO")]
public class StealSizeSO : ScriptableObject
{
    public StealSize size;
    public string displayName;
    [Range(0f, 1f)]
    public float medianRate; // ’†‰›’l‚ÌŠ„‡iMaxSteal‚É‘Î‚·‚é”ä—¦j
}
