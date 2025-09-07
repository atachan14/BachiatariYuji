using UnityEngine;
public enum LayerName
{
    SmallUnit, MiddleUnit, LargeUnit, EffectField, Yuji
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
            daySeed = Random.Range(int.MinValue, int.MaxValue);// ‚Ü‚½‚Í—”‚Å¶¬
            DayData.Instance.MoningTotalEvil = TotalEvil;
            DayWindowManager.Instance.ChangeDay();
            DayData.Instance.ResetDayEvil();
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
     
    }

    public void AddTotalEvil(int value)
    {
        totalEvil += value;
    }

    public int DaySeed
    {
        get => daySeed;
    }
}
