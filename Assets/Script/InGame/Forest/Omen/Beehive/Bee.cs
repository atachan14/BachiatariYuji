using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int dmg = 5;
    [SerializeField] private float accel = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float resumeSpeed = 0.5f;   // 減速完了とみなす速度
    [SerializeField] private float dotThreshold = -0.2f; // 通り過ぎ判定の閾値

    private enum State { Chasing, Braking }
    private State state = State.Chasing;

    void Update()
    {
        switch (state)
        {
            case State.Chasing:
                UpdateChasing();
                break;
            case State.Braking:
                UpdateBraking();
                break;
        }
    }

    void UpdateChasing()
    {
        Vector2 targetPos = Yuji.Instance.transform.position;
        Vector2 toTarget = targetPos - rb.position;
        if (toTarget.sqrMagnitude < 0.01f) return; // Yujiとほぼ同位置なら加速しない

        Vector2 dir = toTarget.normalized;
        rb.AddForce(dir * accel);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // 突き抜け判定
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            Vector2 velDir = rb.linearVelocity.normalized;
            float dot = Vector2.Dot(velDir, dir);

            if (dot < dotThreshold) // 進行方向がYujiから逸れたら通り過ぎと判断
            {
                state = State.Braking;
            }
        }
    }

    void UpdateBraking()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            // 逆方向に力を加えて減速
            Vector2 brakeDir = -rb.linearVelocity.normalized;
            rb.AddForce(brakeDir * accel);

            // 速度が小さくなったら再追尾
            if (rb.linearVelocity.magnitude < resumeSpeed)
            {
                rb.linearVelocity = Vector2.zero;
                state = State.Chasing;
            }
        }
        else
        {
            state = State.Chasing;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Yuji にぶつかったら
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerName.Yuji.ToString()))
        {
            YujiParams.Instance.TakeDamage(dmg, Color.blue);
        }
    }
}
