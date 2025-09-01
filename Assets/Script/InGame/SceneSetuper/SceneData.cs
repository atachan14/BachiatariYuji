using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneData : SingletonMonoBehaviour<SceneData>
{
    [field:SerializeField]public SceneViewMode SceneViewMode { get; set; }
    [field: SerializeField] public CameraMode CameraMode { get; set; }
    [field: SerializeField] public float CameraSize { get; set; } = 5f;
    [field: SerializeField] public bool IsOutDoor { get; set; }

    [Header("シーン内のDoor一覧")]
    public List<Door> doorList = new();


    protected virtual void Start()
    {
        SelectSpawnDoor();
    }

    public InputMode GetInputMode()
    {
        switch (SceneViewMode)
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
