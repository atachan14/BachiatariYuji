using UnityEngine;

public class OriginCamera : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(0, 0, -10);
    }

    private void LateUpdate()
    {
        // �����ƌŒ�Ȃ̂ŏ����Ȃ�
    }
}
