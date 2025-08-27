using UnityEngine;

public class ForestStartGen : SingletonMonoBehaviour<ForestStartGen>
{
    [field: SerializeField] public Transform StartDoor { get; private set; }
    [field: SerializeField] public GameObject startStraightPrefab { get; private set; }
    [SerializeField] private Transform startParent;

    [Header("生成パラメータ")]
    [SerializeField] private int startStraight = 3;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        manager.StartStraightCoords.Clear();

        Vector2Int zero = Vector2Int.zero;
        Vector2Int current = zero;
        Vector2Int lastPlaced = zero;  // ←最後に置いたPrefabの座標を保持

        for (int i = 0; i < startStraight; i++)
        {
            manager.StartStraightCoords.Add(current);

            Instantiate(
                startStraightPrefab,
                new Vector3(current.x, current.y, manager.floorZ),
                Quaternion.identity,
                startParent
            );

            lastPlaced = current;  // ここで保持
            current += Vector2Int.down;
        }

        // 最後に置いたPrefabの上にDoorを配置
        StartDoor.position = new Vector3(lastPlaced.x, lastPlaced.y, manager.doorZ);
    }
}
