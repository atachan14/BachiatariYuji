using System.Collections.Generic;
using UnityEngine;

public class YujiDoActioner : SingletonMonoBehaviour<YujiDoActioner>
{
    [SerializeField] private Transform yujiTransform;      // YujiTopDown / YujiSideScroll �� Transform
    [SerializeField] private float forwardOffset = 0.5f;   // �O���ւ̋���

    public List<CanAction> CanActionInRange { get; } = new List<CanAction>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && !CanActionInRange.Contains(c))
        {
            CanActionInRange.Add(c);
        }
        ActionWindow.Instance.UpdateIcon();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && CanActionInRange.Contains(c))
        {
            CanActionInRange.Remove(c);
        }
        ActionWindow.Instance.UpdateIcon();
    }

    private void Update()
    {
        // 1. �O���Ɉړ�
        if (YujiFacing.Instance.Dir != null && yujiTransform != null)
        {
            Vector3 forward = YujiFacing.Instance.Dir.normalized;
            transform.position = yujiTransform.position + forward * forwardOffset;
        }

        // 2. E�������ɃA�N�V����
        if (InputReceiver.Instance.Action && CanActionInRange.Count > 0)
        {
            CanActionInRange[0].DoAction(); // ��{1�Ȃ̂�[0]��OK
        }
    }
}
