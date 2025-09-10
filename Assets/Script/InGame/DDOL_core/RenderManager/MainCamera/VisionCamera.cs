using UnityEngine;

public class VisionCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private void LateUpdate()
    {
        transform.position = new Vector3(Yuji.Instance.transform.position.x, Yuji.Instance.transform.position.y, -10);
        cam.orthographicSize = Mathf.Max(0f, YujiState.Instance.Vision / 100f);
    }
}
