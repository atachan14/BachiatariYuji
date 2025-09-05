using UnityEngine;

public class GoToNode : BaseNode
{
    [SerializeField] private Transform target;  // Yuji ���������ꏊ
    [SerializeField] private float moveSpeedRatio = 1;

    public override void PlayNode()
    {
        if (target == null)
        {
            Debug.LogWarning("Target ���ݒ�I");
            nextNode?.PlayNode();
            return;
        }

        // Yuji �̈ړ��� Coroutine �ŏ���
        StartCoroutine(MoveToTarget());
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        var yuji = Yuji.Instance; // Yuji �� Singleton �z��
        while ((yuji.transform.position - target.position).sqrMagnitude > 0.01f)
        {
            yuji.transform.position = Vector3.MoveTowards(
                yuji.transform.position, target.position, YujiState.Instance.MoveSpeed * moveSpeedRatio * Time.deltaTime);
            yield return null;
        }

        nextNode?.PlayNode(); // ������Ɏ��m�[�h���s
    }
}
