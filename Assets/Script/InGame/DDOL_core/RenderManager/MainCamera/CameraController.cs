using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraMode { Fixed, Follow, SideScroll, Vision }

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField] public Camera cam;
    [SerializeField] private OriginCamera originCam;
    [SerializeField] private FollowCamera FollowCam;
    [SerializeField] private SideScrollCamera sideScrollCam;
    [SerializeField] private VisionCamera visionCam;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += RefreshMode;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshMode;

    }

    protected override void Awake()
    {
        base.Awake();
        // 最初は全部無効化
        originCam.enabled = false;
        FollowCam.enabled = false;
        sideScrollCam.enabled = false;
        visionCam.enabled = false;
    }

    public void SwitchMode(CameraMode mode, float size)
    {
        // 全部無効化してから
        originCam.enabled = false;
        FollowCam.enabled = false;
        sideScrollCam.enabled = false;
        visionCam.enabled = false;

        // 指定のモードだけ有効化
        switch (mode)
        {
            case CameraMode.Fixed:
                originCam.enabled = true;
                break;
            case CameraMode.Follow:
                FollowCam.enabled = true;
                break;
            case CameraMode.SideScroll:
                sideScrollCam.enabled = true;
                break;
            case CameraMode.Vision:
                visionCam.enabled = true;
                break;

        }
        cam.orthographicSize = size;
    }

    void RefreshMode(Scene s, LoadSceneMode m)
    {
        SwitchMode(SceneData.Instance.CameraMode, SceneData.Instance.CameraSize);
    }
}
