using System.Collections.Generic;
using UnityEngine;
public enum StealSize
{
    Small,
    Medium,
    Large
}
public class YujiParams : SingletonMonoBehaviour<YujiParams>
{
    [Header("共通")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHP = 200;
    [SerializeField] private float fixDef;
    [SerializeField] private float perDef;
    [SerializeField] private float ccDef;
    [SerializeField] private float vision;

    [Header("TopDown")]

    [Header("SideScroll")]
    [SerializeField] private float jumpForce = 7f;

    [Header("SaisenSteal")]
    public int StealPower = 1;
    public int MaxStealStreak = 3;  // 最大連続回数
    public int MaxSteal = 2000;
    public List<StealSizeSO> UnlockedStealSizes;  // 現在選べるサイズ
    public float SigmaRatio = 0.3f;
    public float StealOpenPower = 30;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value);
    }

    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = Mathf.Max(0, value);
    }
    public float MaxHelth
    {
        get => maxHP;
        set => maxHP = Mathf.Max(0, value);
    }

    public float FixDef
    {
        get => fixDef;
        set => fixDef = value;
    }

    public float PerDef
    {
        get => perDef;
        set => perDef = value;
    }

    public float CcDef
    {
        get => ccDef;
        set => ccDef = value;
    }

    public float Vision
    {
        get => vision;
        set => vision = Mathf.Max(0, value);
    }
}