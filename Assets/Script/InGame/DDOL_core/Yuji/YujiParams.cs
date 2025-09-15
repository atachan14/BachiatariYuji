using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;
public enum StealSize
{
    Small,
    Medium,
    Large
}
public class YujiParams : SingletonMonoBehaviour<YujiParams>
{
    [Header("共通")]
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int health = 200;
    [SerializeField] private int fixDef = 0;
    [SerializeField] private int perDef = 0;
    [SerializeField] private int detoxPower = 1;
    [SerializeField] private int vision = 5;

    [Header("TopDown")]


    [Header("SaisenSteal")]
    public int StealPower = 1;
    public int MaxStealStreak = 3;  // 最大連続回数
    public int MaxSteal = 2000;
    public List<StealSizeSO> UnlockedStealSizes;  // 現在選べるサイズ
    public float SigmaRatio = 0.3f;
    public int StealOpenPower = 30;

    public event Action<int, Color> OnDamaged;
    public event Action<int> OnHealed;


    public int MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value);
    }

    public int MaxHelth
    {
        get => maxHealth;
        set => maxHealth = Mathf.Max(0, value);
    }
    
    public int Health
    {
        get => health;
    }
    public int FixDef
    {
        get => fixDef;
        set => fixDef = value;
    }

    public int PerDef
    {
        get => perDef;
        set => perDef = value;
    }

    public int DetoxPower
    {
        get => detoxPower;
        set => detoxPower = value;
    }

    public int Vision
    {
        get => vision;
        set => vision = Mathf.Max(0, value);
    }


    public void TakeDamage(int damage, Color color)
    {
        int finalDamage = Mathf.Max(0, damage * 100 / (100 + perDef) - fixDef);

        // healthが0未満にならないように減算
        health = Mathf.Max(0, health - finalDamage);

        OnDamaged?.Invoke(finalDamage, color);

        if (health <= 0)
        {
            Debug.Log("Game Over");
        }
    }
    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Color.red);
    }
    public void GetHeal(int heal)
    {
        // 最大体力を越えないように回復
        int actualHeal = Mathf.Min(heal, maxHealth - health);
        health += actualHeal;

        // 回復イベント
        OnHealed?.Invoke(actualHeal);
    }

    public void SleepHeal()
    {
        // 回復量 = maxHealth の1/3
        int healAmount = maxHealth / 10;

        // もし現在の health に回復を足して maxHealth を超える場合、overflow を maxHealth に加算
        int projectedHealth = health + healAmount;
        if (projectedHealth > maxHealth)
        {
            int overflow = projectedHealth - maxHealth;
            maxHealth += overflow;
        }

        // 回復は最後に GetHeal で適用（UI/Effect もここで）
        GetHeal(healAmount);

        Debug.Log($"[SleepHeal] 回復量: {healAmount}, 現在体力: {health}, 最大体力: {maxHealth}");
    }
}