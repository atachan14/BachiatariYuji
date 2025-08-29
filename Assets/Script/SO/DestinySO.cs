using UnityEngine;
public enum DestinyType
{
    Omen,Punish,InnerTree
}

[CreateAssetMenu(fileName = "DestinySO", menuName = "Scriptable Objects/DestinySO")]
public class DestinySO : ScriptableObject
{
    public DestinyType DestinyType;

    [Header("出現ロジック")]
    public float preferredLevel;  // 中央値 (TotalEvil)
    public float sigma;           // 広がり (分散)
    public float rarity;          // 同レベル帯での出やすさ倍率
    public float baseWeight;      // 常時出現補正 (最低保証)
}