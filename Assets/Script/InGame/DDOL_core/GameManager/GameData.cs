using UnityEngine;

public enum DayTime
{
    Morning, Night
}
public class GameData : SingletonMonoBehaviour<GameData>
{
    [field: SerializeField] private int day;
    [field: SerializeField] private int bank;
    [field: SerializeField] private int totalEvil;
    [field: SerializeField] private int daySeed;

    private void Start()
    {
        Day = day;
        Bank = bank;
    }
    public int Day
    {
        get => day;
        set
        {
            day = value;
            DayWindowManager.Instance.ChangeDay();
        }
    }
    public int Bank
    {
        get => bank;
        set
        {
            int old = bank;
            bank = value;
            BankWindowManager.Instance.valueChangeAnimator.ChangeValue(old, bank);
        }
    }
    public int TotalEvil
    {
        get => totalEvil;
        set
        {
            totalEvil = value;
        }
    }

    public int DaySeed
    {
        get => daySeed;
        set => daySeed = value;

    }
}
