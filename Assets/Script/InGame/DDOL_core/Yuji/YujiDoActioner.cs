using System.Collections.Generic;
using UnityEngine;

public class YujiDoActioner : MonoBehaviour
{
    [SerializeField] private Transform yujiTransform;      // YujiTopDown / YujiSideScroll �� Transform
    [SerializeField] private YujiFacingBase yujiFacing;    // YTDFacing / YSSFacing ���Z�b�g
    [SerializeField] private float forwardOffset = 1.0f;   // �O���ւ̋���

    private List<CanAction> canActionInRange = new List<CanAction>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("on trigger enter 2D");
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && !canActionInRange.Contains(c))
        {
            canActionInRange.Add(c);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("on trigger exit 2D");
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && canActionInRange.Contains(c))
        {
            canActionInRange.Remove(c);
        }
    }

    private void Update()
    {
        // 1. �O���Ɉړ�
        if (yujiFacing != null && yujiTransform != null)
        {
            Vector3 forward = yujiFacing.FacingDir.normalized;
            transform.position = yujiTransform.position + forward * forwardOffset;
        }

        // 2. E�������ɃA�N�V����
        if (InputReceiver.Instance.PlayE && canActionInRange.Count > 0)
        {
            canActionInRange[0].DoAction(); // ��{1�Ȃ̂�[0]��OK
        }
    }
}
