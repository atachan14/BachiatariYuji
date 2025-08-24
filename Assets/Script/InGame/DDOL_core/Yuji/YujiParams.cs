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

    [Header("TopDown")]

    [Header("SideScroll")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float crouchScaleY = 0.5f;

    [Header("SaisenSteal")]
    [SerializeField] public int StealPower = 1;
    [SerializeField] public int MaxStealStreak = 3;  // 最大連続回数
    [SerializeField] public int MaxSteal = 2000;
    [SerializeField] public List<StealSizeSO> UnlockedStealSizes ;  // 現在選べるサイズ
    [SerializeField] public float SigmaRatio = 0.3f;
    [SerializeField] public float StealOpenPower = 30;

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

    public float CrouchScaleY
    {
        get => crouchScaleY;
        set => crouchScaleY = Mathf.Clamp(value, 0.1f, 1f);
    }
}
