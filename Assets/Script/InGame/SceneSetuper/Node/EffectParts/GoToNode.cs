using UnityEngine;

public class GoToNode : BaseNode
{
    [SerializeField] private Transform target;  // Yuji が向かう場所
    [SerializeField] private float moveSpeedRatio = 1;

    public override void PlayNode()
    {
        if (target == null)
        {
            Debug.LogWarning("Target 未設定！");
            nextNode?.PlayNode();
            return;
        }

        // Yuji の移動を Coroutine で処理
        StartCoroutine(MoveToTarget());
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        var yuji = Yuji.Instance; // Yuji は Singleton 想定
        while ((yuji.transform.position - target.position).sqrMagnitude > 0.01f)
        {
            yuji.transform.position = Vector3.MoveTowards(
                yuji.transform.position, target.position, YujiState.Instance.MoveSpeed * moveSpeedRatio * Time.deltaTime);
            yield return null;
        }

        nextNode?.PlayNode(); // 到着後に次ノード実行
    }
}
