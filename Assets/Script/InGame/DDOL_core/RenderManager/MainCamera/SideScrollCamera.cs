using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] private Transform yuji;

    private void LateUpdate()
    {
        transform.position = new Vector3(yuji.position.x, -3, -10);
    }
}
