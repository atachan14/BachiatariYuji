using UnityEngine;

public class WaitNode : BaseNode
{
    [SerializeField] private float waitTime = 1f;

    public override void PlayNode()
    {
        StartCoroutine(WaitCoroutine());
    }

    private System.Collections.IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        nextNode?.PlayNode();
    }
}
