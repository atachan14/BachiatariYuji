using UnityEngine;

public enum DayTime
{
    Morning, Night
}
public class GameData : SingletonMonoBehaviour<GameData>
{
    [field: SerializeField] private int day;
    [field: SerializeField] private DayTime dayTime;
    [field: SerializeField] private int bank;
    [field: SerializeField] private int cash;
    [field: SerializeField] private int dayEvil;
    [field: SerializeField] private int totalEvil;
    [field: SerializeField] private int daySeed;

    private void Start()
    {
        Day = day;
        DayTime = dayTime;
        Cash = cash;  // 初期表示をアニメーションなしで反映
        Bank = bank;
    }
    // Day
    public int Day
    {
        get => day;
        set
        {
            int old = day;
            day = value;
            DayWindowManager.Instance.ChangeDay();
        }
    }

    // DayTime
    public DayTime DayTime
    {
        get => dayTime;
        set
        {
            dayTime = value;
            DayWindowManager.Instance.ChangeDayTime();
            GlobalLightController.Instance.SetDayTime(dayTime);
        }
    }

    // Cash
    public int Cash
    {
        get => cash;
        set
        {
            int old = cash;
            cash = value;
            CashWindowManager.Instance.valueChangeAnimator.ChangeValue(old, cash);
        }
    }

    // Bank
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

    // DayEvil
    public int DayEvil
    {
        get => dayEvil;
        set
        {
            int old = dayEvil;
            dayEvil = value;
        }
    }

    // TotalEvil
    public int TotalEvil
    {
        get => totalEvil;
        set
        {
            int old = totalEvil;
            totalEvil = value;
        }
    }

    public int DaySeed
    {
        get => daySeed;
        set => daySeed = value;

    }
}
