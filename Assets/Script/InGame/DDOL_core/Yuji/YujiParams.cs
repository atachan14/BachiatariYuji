using UnityEngine;

public class YujiParams : DDOL_child<YujiParams>
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("�W�����v�ݒ�")]
    [SerializeField] private float jumpForce = 7f;

    [Header("���Ⴊ�ݐݒ�")]
    [SerializeField] private float crouchScaleY = 0.5f;

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
