using UnityEngine;

public class ForestCamera : MonoBehaviour
{
    [SerializeField] private Transform yuji;

    private void LateUpdate()
    {
        transform.position = new Vector3(yuji.position.x, -3, -10);
    }
}
