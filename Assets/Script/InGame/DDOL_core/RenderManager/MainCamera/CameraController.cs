using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraMode { Fixed, Follow, SideScroll, Title }

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField] Camera cam;
    [SerializeField] private OriginCamera originCam;
    [SerializeField] private YujiCamera yujiCam;
    [SerializeField] private ForestCamera forestCam;
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
        // 最初は全部無効化
        originCam.enabled = false;
        yujiCam.enabled = false;
        forestCam.enabled = false;
    }

    public void SwitchMode(CameraMode mode, float size)
    {
        // 全部無効化してから
        originCam.enabled = false;
        yujiCam.enabled = false;
        forestCam.enabled = false;

        // 指定のモードだけ有効化
        switch (mode)
        {
            case CameraMode.Fixed:
                originCam.enabled = true;
                break;
            case CameraMode.Follow:
                yujiCam.enabled = true;
                break;
            case CameraMode.SideScroll:
                forestCam.enabled = true;
                break;
        }
        cam.orthographicSize = size;
    }

    void RefreshMode(Scene s, LoadSceneMode m)
    {
        SwitchMode(SceneData.Instance.CameraMode, SceneData.Instance.CameraSize);
    }
}
