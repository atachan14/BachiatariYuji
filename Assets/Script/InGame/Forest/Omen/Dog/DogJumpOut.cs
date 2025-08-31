using System.Collections;
using System.Linq;
using UnityEngine;

public class DogJumpOut : MonoBehaviour
{
    [SerializeField] Transform dogParent;

    public IEnumerator Exe(float jumpRange)
    {
        dogParent.gameObject.layer = LayerMask.NameToLayer(LayerName.SmallUnit.ToString());
        // 現在位置とYujiの位置
        Vector2Int dogPos = Vector2Int.RoundToInt(dogParent.position);
        Vector2 yujiPos = Yuji.Instance.transform.position;

        // Floor+Branch の候補
        var manager = ForestManager.Instance; // ForestManagerじゃなくてForestGenManagerだよね？
        var candidates = manager.FloorAndBranchCoords
            .Where(c => Vector2Int.Distance(dogPos, c) <= jumpRange);

        if (!candidates.Any())
        {
            Debug.LogWarning("[DogJumpOut] 候補がないので飛び出し失敗");
            yield break;
        }

        // Yujiに最も近い座標を選ぶ
        Vector2Int target = candidates
            .OrderBy(c => Vector2.Distance(yujiPos, c)) // Vector2Int→Vector2に暗黙変換される
            .First();

        // ビョンっとジャンプ
        Vector3 targetPos = new(target.x, target.y, dogParent.position.z);
        yield return StartCoroutine(JumpTo(targetPos));
        dogParent.gameObject.layer = LayerMask.NameToLayer(LayerName.MiddleUnit.ToString());
    }

    // ビョンっとジャンプ移動
    private IEnumerator JumpTo(Vector3 targetPos, float duration = 0.5f, float height = 1.5f)
    {
        Vector3 startPos = dogParent.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 線形補間
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            // パラボラ加算
            pos.y += Mathf.Sin(t * Mathf.PI) * height;

            dogParent.position = pos;
            yield return null;
        }

        dogParent.position = targetPos; // 最後ピタッと着地
    }
}
