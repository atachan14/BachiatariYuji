using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform yuji;

    private void LateUpdate()
    {
        transform.position = new Vector3(yuji.position.x, yuji.position.y, -10);
    }
}
