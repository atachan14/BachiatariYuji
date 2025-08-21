using UnityEngine;

public enum DayTime
{
    Morning,Night
}
public class GameData : SingletonMonoBehaviour<GameData>
{
    [field: SerializeField] public int Day { get; set; }
    [field: SerializeField] public DayTime DayTime { get; set; }
    [field: SerializeField] public int Bank { get; set; }
    [field: SerializeField] public int Cash { get; set; }
    [field: SerializeField] public int DayEvil { get; set; }
    [field: SerializeField] public int TotalEvil { get; set; }
}
