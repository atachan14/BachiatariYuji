using UnityEngine;

public enum CameraMode { Fixed, Follow, SideScroll }

public class CameraController : DDOL_child<CameraController>
{
    [SerializeField] Camera cam;
    [SerializeField] private OriginCamera originCam;
    [SerializeField] private YujiCamera yujiCam;
    [SerializeField] private ForestCamera forestCam;

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
}
