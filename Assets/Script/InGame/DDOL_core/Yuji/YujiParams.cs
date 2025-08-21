using UnityEngine;

public class YujiParams : SingletonMonoBehaviour<YujiParams>
{
    [Header("‹¤’Ê")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("TopDown")]

    [Header("SideScroll")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float crouchScaleY = 0.5f;



    [Header("ThiefGame")]


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
