using UnityEngine;


[CreateAssetMenu(fileName = "DestinySO", menuName = "DestinySO/DestinySO")]
public abstract class DestinySO : ScriptableObject
{
    [Header("出現ロジック")]
    public float peak;            // 中央値 (Day)
    public float sigma;           // 広がり (分散)
    public float rarity;          // 同レベル帯での出やすさ倍率
    public float baseWeight;      // 常時出現補正 (最低保証)
}