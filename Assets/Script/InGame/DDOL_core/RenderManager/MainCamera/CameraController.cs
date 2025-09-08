using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraMode { Fixed, Follow, SideScroll, Vision }

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField] Camera cam;
    [SerializeField] private OriginCamera originCam;
    [SerializeField] private FollowCamera FollowCam;
    [SerializeField] private SideScrollCamera sideScrollCam;
    [SerializeField] private VisionCamera visionCam;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += RefreshMode;
        TitleManager.OnTitleMenu += HandleTitleMenu;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshMode;
        TitleManager.OnTitleMenu -= HandleTitleMenu;

    }

    void HandleTitleMenu()
    {
        Debug.Log("HandleTitleMenu");
        SwitchMode(CameraMode.Follow, 0.3f);
    }
    protected override void Awake()
    {
        base.Awake();
        // �ŏ��͑S��������
        originCam.enabled = false;
        FollowCam.enabled = false;
        sideScrollCam.enabled = false;
        visionCam.enabled = false;
    }

    public void SwitchMode(CameraMode mode, float size)
    {
        // �S�����������Ă���
        originCam.enabled = false;
        FollowCam.enabled = false;
        sideScrollCam.enabled = false;
        visionCam.enabled = false;

        // �w��̃��[�h�����L����
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
