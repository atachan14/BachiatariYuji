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
        // �ŏ��͑S��������
        originCam.enabled = false;
        yujiCam.enabled = false;
        forestCam.enabled = false;
    }

    public void SwitchMode(CameraMode mode, float size)
    {
        // �S�����������Ă���
        originCam.enabled = false;
        yujiCam.enabled = false;
        forestCam.enabled = false;

        // �w��̃��[�h�����L����
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
