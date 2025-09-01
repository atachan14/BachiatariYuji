using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayData : SingletonMonoBehaviour<DayData>
{
    [field: SerializeField] private DayTime dayTime;
    [field: SerializeField] private int cash;
    [field: SerializeField] private int dayEvil;

    public static event Action OnDayTimeChanged;
    public static event Action OnCashChanged;

    private void Start()
    {
        DayTime = dayTime;
        Cash = cash;
    }

    public DayTime DayTime
    {
        get => dayTime;
        set
        {
            dayTime = value;
            DayWindowManager.Instance.ChangeDayTime();
            SunLightController.Instance.SetDayTime(dayTime);
            OnDayTimeChanged?.Invoke();
        }
    }


    public int Cash
    {
        get => cash;
        set
        {
            int old = cash;
            cash = value;
            CashWindowManager.Instance.valueChangeAnimator.ChangeValue(old, cash);
            OnCashChanged?.Invoke();
        }
    }
    public int DayEvil
    {
        get => dayEvil;
        set
        {
            dayEvil = value;
        }
    }
}
