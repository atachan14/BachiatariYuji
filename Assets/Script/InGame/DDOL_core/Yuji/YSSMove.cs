using UnityEngine;

public class YSSMove : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform yuji;      // 親
    [SerializeField] private Transform groundCheck;   // 足元の接地判定位置
    [SerializeField] private LayerMask groundLayer;   // 地面レイヤー
    [SerializeField] private float groundCheckRadius = 0.1f;

    [SerializeField] private Rigidbody2D rb;
    private InputReceiver input;
    private YujiParams yujiParams;

    private bool isGrounded;
    private bool isCrouching;

    private void Awake()
    {
        input = InputReceiver.Instance;
        yujiParams = YujiParams.Instance;
    }

    private void Update()
    {
        CheckGround();
        HandleMove();
        HandleJump();
        HandleCrouch();
    }

    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }
    }

    private void HandleMove()
    {
        float moveX = input.MoveAxisX; 
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveX * yujiParams.MoveSpeed;
        rb.linearVelocity = velocity;

       
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            float jumpForce = yujiParams.JumpForce; // YujiParamsに追加して使う
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!isCrouching && yuji != null)
            {
                yuji.localScale = new Vector3(yuji.localScale.x, yuji.localScale.y * 0.5f, yuji.localScale.z);
                isCrouching = true;
            }
        }
        else
        {
            if (isCrouching && yuji != null)
            {
                yuji.localScale = new Vector3(yuji.localScale.x, yuji.localScale.y * 2f, yuji.localScale.z);
                isCrouching = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // groundCheck の判定円をSceneビューで可視化
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
