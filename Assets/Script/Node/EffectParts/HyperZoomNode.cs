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

    [Header("UI Size")]
    [SerializeField] private bool isHyper = false;
    [SerializeField] private Vector2 normalSize = new Vector2(1920, 1080);
    [SerializeField] private Vector2 hyperSize = new Vector2(3840, 2160);

    private Coroutine running;

    public override void PlayNode()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(HyperZoomRoutine());
    }

    private IEnumerator HyperZoomRoutine()
    {
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

        // RectTransformサイズで操作
        RectTransform uiRect = StandingUI.Instance.GetComponent<RectTransform>();
        Vector2 startSizeDelta = uiRect.sizeDelta;
        Vector2 endSizeDelta = isHyper ? hyperSize : normalSize;

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

            // UIサイズ
            uiRect.sizeDelta = Vector2.Lerp(startSizeDelta, endSizeDelta, t);

            yield return null;
        }

        // 最終値で固定
        cam.transform.position = new Vector3(target.position.x, target.position.y, cam.transform.position.z);
        cam.orthographicSize = endSize;
        uiRect.sizeDelta = endSizeDelta;

        running = null;

        nextNode?.PlayNode();
    }
}
