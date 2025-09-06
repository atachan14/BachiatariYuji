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
public enum DamageType
{
    Nomal,
    Poison,
    Elec
}
public class YujiParams : SingletonMonoBehaviour<YujiParams>
{
    [Header("共通")]
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int health = 200;
    [SerializeField] private int fixDef = 0;
    [SerializeField] private int perDef = 0;
    [SerializeField] private int ccDef = 0;
    [SerializeField] private int vision = 5;
    [SerializeField] private int hallucinations = 0;

    [Header("TopDown")]

    [Header("SideScroll")]
    [SerializeField] private int jumpForce = 7;

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

    public int CcDef
    {
        get => ccDef;
        set => ccDef = value;
    }

    public int Vision
    {
        get => vision;
        set => vision = Mathf.Max(0, value);
    }

    public int Hallucinations
    {
        get => hallucinations;
        set => hallucinations = Mathf.Max(0, value);
    }

    public int JumpForce
    {
        get => jumpForce;
        set => jumpForce = Mathf.Max(0, value);
    }
    public void TakeDamage(int damage, Color color)
    {
        int finalDamage = Mathf.Max(0, damage * 100 / (100 + perDef) - fixDef);
        health -= finalDamage;

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
        health += heal;
        OnHealed?.Invoke(heal);
    }
}