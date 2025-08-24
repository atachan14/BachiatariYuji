using System.Collections.Generic;
using UnityEngine;

public class YujiDoActioner : MonoBehaviour
{
    [SerializeField] private Transform yujiTransform;      // YujiTopDown / YujiSideScroll の Transform
    [SerializeField] private YujiFacingBase yujiFacing;    // YTDFacing / YSSFacing をセット
    [SerializeField] private float forwardOffset = 0.5f;   // 前方への距離

    private List<CanAction> canActionInRange = new List<CanAction>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && !canActionInRange.Contains(c))
        {
            canActionInRange.Add(c);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CanAction c = other.GetComponent<CanAction>();
        if (c != null && canActionInRange.Contains(c))
        {
            canActionInRange.Remove(c);
        }
    }

    private void Update()
    {
        // 1. 前方に移動
        if (yujiFacing != null && yujiTransform != null)
        {
            Vector3 forward = yujiFacing.FacingDir.normalized;
            transform.position = yujiTransform.position + forward * forwardOffset;
        }

        // 2. E押下時にアクション
        if (InputReceiver.Instance.Action && canActionInRange.Count > 0)
        {
            canActionInRange[0].DoAction(); // 基本1個なので[0]でOK
        }
    }
}
