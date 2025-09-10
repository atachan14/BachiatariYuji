using System.Collections;
using UnityEngine;

public class HyperZoomNode : BaseNode
{
    [Header("Target Settings")]
    [SerializeField] private bool targetIsYuji = true;
    [SerializeField] private Transform customTarget;

    [Header("Camera Zoom")]
    [SerializeField] private float endSize = 3.5f; // 最終カメラサイズ
    [SerializeField] private float zoomDuration = 1f;

    [Header("UI Scale")]
    [SerializeField] private bool isHyper = false;
    [SerializeField] private Vector3 normalScale = new Vector3(1920, 1080, 1f);
    [SerializeField] private Vector3 hyperScale = new Vector3(3840, 2160, 1f);

    private Coroutine running;

    public override void PlayNode()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(HyperZoomRoutine());
    }

    private IEnumerator HyperZoomRoutine()
    {
        // ターゲット決定
        Transform target = targetIsYuji ? Yuji.Instance.transform : customTarget;
        if (target == null)
        {
            Debug.LogWarning("HyperZoomNode: Target is null.");
            yield break;
        }

        Camera cam = CameraController.Instance.cam;
        if (cam == null)
        {
            Debug.LogWarning("HyperZoomNode: Main Camera not found.");
            yield break;
        }

        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        Vector3 startScale = StandingUI.Instance.transform.localScale;
        Vector3 endScale = isHyper ? hyperScale : normalScale;

        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / zoomDuration);

            // Camera
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, endSize, t);

            // UI
            StandingUI.Instance.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        // 最終値で固定
        cam.transform.position = new Vector3(target.position.x, target.position.y, cam.transform.position.z);
        cam.orthographicSize = endSize;
        StandingUI.Instance.transform.localScale = endScale;

        running = null;

        nextNode?.PlayNode();
    }
}
