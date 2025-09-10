using UnityEngine;

public class GoToNode : BaseNode
{
    [Header("�Œ�^�[�Q�b�g")]
    [SerializeField] private Transform fixedTarget;

    [Header("Yuji��I�t�Z�b�g")]
    [SerializeField] private bool useOffsetFromYuji = false;
    [SerializeField] private Vector3 yujiOffset;

    [SerializeField] private float moveSpeedRatio = 1;

    public override void PlayNode()
    {
        Vector3 destination;

        if (useOffsetFromYuji)
        {
            destination = Yuji.Instance.transform.position + yujiOffset;
        }
        else if (fixedTarget != null)
        {
            destination = fixedTarget.position;
        }
        else
        {
            Debug.LogWarning("Target ���ݒ�I");
            nextNode?.PlayNode();
            return;
        }

        StartCoroutine(MoveToTarget(destination));
    }

    private System.Collections.IEnumerator MoveToTarget(Vector3 destination)
    {
        var yuji = Yuji.Instance.transform;

        while ((yuji.position - destination).sqrMagnitude > 0.01f)
        {
            yuji.position = Vector3.MoveTowards(
                yuji.position, destination, YujiState.Instance.MoveSpeed / 100 * moveSpeedRatio * Time.deltaTime);
            yield return null;
        }

        nextNode?.PlayNode();
    }
}
