using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class SceneSetuper : MonoBehaviour
{
    public static SceneSetuper Instance { get; private set; }
    [SerializeField] private SceneViewMode viewMode;
    [SerializeField] private CameraMode cameraMode;
    [SerializeField] private float cameraSize = 5f;

    [Header("シーン内のDoor一覧")]
    public List<Door> doorList = new List<Door>();

    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        InputReceiver.Instance.SwitchMode(GetInputMode());
        Yuji.Instance.SwitchMode(viewMode);
        CameraController.Instance.SwitchMode(cameraMode, cameraSize);
        SelectSpawnDoor();
    }

    public InputMode GetInputMode()
    {
        switch (viewMode)
        {
            case SceneViewMode.TopDown: return InputMode.TopDown;
            case SceneViewMode.SideScroll: return InputMode.SideScroll;
            default: return InputMode.TopDown; // fallback
        }
    }


    public void SelectSpawnDoor()
    {
        Door door = FindDoor(SceneChanger.Instance.SpawnDoorName);
        if (door != null)
        {
            door.SpawnYuji(); // Door自身にSpawnの責務を持たせる
        }
        else
        {
            Debug.LogWarning($"SpawnDoorName {SceneChanger.Instance.SpawnDoorName} が見つかりませんでした。DoorList: " +
                  string.Join(", ", doorList.Select(d => d.doorName)));
        }

    }

    public Door FindDoor(DoorName doorName)
    {
        return doorList.FirstOrDefault(d => d.doorName == doorName);
    }
}
