using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DayTime
{
    Morning, Night,Count
}
public class DayData : SingletonMonoBehaviour<DayData>
{
    [field: SerializeField] private DayTime dayTime;
    [field: SerializeField] private int cash;
    [field: SerializeField] private int moningTotalEvil;
    [field: SerializeField] private int dayEvil;

    private void Start()
    {
        dayTime = DayTime.Morning;
        Cash = cash;
    }

    public DayTime DayTime
    {
        get => dayTime;
        
    }
    public void NextDayTime()
    {
        dayTime++;
        if ((int)dayTime >= (int)DayTime.Count)
        {
            GameData.Instance.Day++;
            dayTime = DayTime.Morning;
        }
        DayWindowManager.Instance.ChangeDayTime();
        SunLightController.Instance.SetDayTime(dayTime);
        CanActionerManager.Instance.RefreshAll();
    }



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
    public int DayEvil
    {
        get => dayEvil;
    }
    public void AddDayEvil(int value)
    {
        dayEvil += value;
        GameData.Instance.AddTotalEvil(value);
    }
    public void ResetDayEvil()
    {
        dayEvil = 0;
    }
    public int MoningTotalEvil
    {
        get => moningTotalEvil;
        set
        {
            moningTotalEvil = value;
        }
    }
}
