using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    private void LateUpdate()
    {
        transform.position = new Vector3(Yuji.Instance.transform.position.x, Yuji.Instance.transform.position.y, -10);
    }
}
