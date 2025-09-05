using System.Collections;
using UnityEngine;

public class SnakeBite : MonoBehaviour
{
    [SerializeField] private PunishParams para;      // ダメージ情報
    [SerializeField] private float extendSpeed = 10f; // 伸縮スピード
    [SerializeField] private float extendLength = 1f; // 最大伸び倍率（元のlocalScale.xに乗算）
    [SerializeField] private string targetLayer = "Yuji"; // 当たり判定レイヤー

    public void Bite(Transform target)
    {
        if (target == null) return;

        StartCoroutine(BiteRoutine(target));
    }

    private IEnumerator BiteRoutine(Transform target)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 direction = (target.position - transform.position).normalized;
        float t = 0f;

        // 伸びる方向を向く
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 伸縮フェーズ
        while (t < 1f)
        {
            t += Time.deltaTime * extendSpeed;

            // 線形補間で伸びる
            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x, originalScale.x * extendLength, t),
                originalScale.y,
                originalScale.z
            );

            // Raycastで当たり判定
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector2.Distance(transform.position, target.position), LayerMask.GetMask(targetLayer));
            if (hit.collider != null)
            {
                YujiParams.Instance.TakeDamage((int)para.damage,Color.green);
                // 一度当たったらそれ以上判定しない
                break;
            }

            yield return null;
        }

        // 元のサイズに戻す
        transform.localScale = originalScale;
    }
}
