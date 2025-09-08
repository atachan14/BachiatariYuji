using UnityEngine;

public class VisionCamera : MonoBehaviour
{
    [SerializeField] private Transform yuji;
    [SerializeField] private Camera cam;
    private void LateUpdate()
    {
        transform.position = new Vector3(yuji.position.x, yuji.position.y, -10);
        cam.orthographicSize = Mathf.Max(0f, YujiState.Instance.Vision / 100f);
    }
}
