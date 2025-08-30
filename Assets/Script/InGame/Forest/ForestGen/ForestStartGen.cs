using UnityEngine;

public class ForestStartGen : SingletonMonoBehaviour<ForestStartGen>
{
    [SerializeField] public Transform startDoor;

    [Header("生成パラメータ")]
    [SerializeField] private int startStraight = 4;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;

        Vector2Int zero = Vector2Int.zero;
        Vector2Int current = zero;
        Vector2Int lastPlaced = zero;

        for (int i = 0; i < startStraight; i++)
        {
            // ManagerのRegisterを使う
            manager.Register(current, TileType.StartStraight);

            lastPlaced = current;
            current += Vector2Int.down;
        }

        // 最後に置いたPrefabの上にDoorを配置
        startDoor.position = new Vector3(lastPlaced.x, lastPlaced.y, manager.doorZ);
    }
}
