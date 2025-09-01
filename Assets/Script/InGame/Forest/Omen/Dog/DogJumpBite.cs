using System.Collections;
using UnityEngine;

public class DogJumpBite : MonoBehaviour
{
    [SerializeField] DogChase chase;
    [SerializeField] BoxCollider2D col;
    [SerializeField] float biteDuration = 1f;       // 噛みつき時間
    [SerializeField] float damageInterval = 0.25f;   // ダメージ間隔
    [SerializeField] int damagePerTick = 10;         // ダメージ量

    public void Exe()
    {
        StartCoroutine(BiteFlow());
    }

    IEnumerator BiteFlow()
    {
        chase.isBiting = true;
        gameObject.layer = LayerMask.NameToLayer(LayerName.SmallUnit.ToString());

        // Yujiの現在位置をロック
        Vector2 lockTarget = Yuji.Instance.transform.position;

        bool hit = false;
        Vector2 bitePos = Vector2.zero;

        // ターゲット地点へ飛びかかる
        while (Vector2.Distance(transform.position, lockTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                lockTarget,
                chase.moveSpeed * 2f * Time.deltaTime
            );

            // Yujiに触れた瞬間にヒット判定
            if (Vector2.Distance(transform.position, Yuji.Instance.transform.position) < 0.4f)
            {
                hit = true;
                bitePos = transform.position; // その瞬間の座標を保存
                break;
            }

            yield return null;
        }

        if (hit)
        {
            // Yuji に対するオフセットを保存
            Transform yujiTf = Yuji.Instance.transform;
            Vector3 offset = transform.position - yujiTf.position;

            float timer = biteDuration;
            float damageTimer = 0f;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                damageTimer -= Time.deltaTime;

                // Yuji の現在位置 + ヒット時のオフセット に追従
                transform.position = yujiTf.position + offset;

                // 一定間隔でダメージ処理
                if (damageTimer <= 0f)
                {
                    YujiParams.Instance.TakeDamage(damagePerTick);
                    damageTimer = damageInterval;
                }

                yield return null;
            }
        }
        gameObject.layer = LayerMask.NameToLayer(LayerName.MiddleUnit.ToString());

        // 外れた場合は何もせず終了
        chase.isBiting = false;
    }
}
