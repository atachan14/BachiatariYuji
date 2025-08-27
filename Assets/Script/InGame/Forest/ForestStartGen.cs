using UnityEngine;

public class ForestStartGen : SingletonMonoBehaviour<ForestStartGen>
{
    [field: SerializeField] public Transform StartDoor { get; private set; }
    [field: SerializeField] public GameObject startStraightPrefab { get; private set; }
    [SerializeField] private Transform startParent;

    [Header("�����p�����[�^")]
    [SerializeField] private int startStraight = 3;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        manager.StartStraightCoords.Clear();

        Vector2Int zero = Vector2Int.zero;
        Vector2Int current = zero;
        Vector2Int lastPlaced = zero;  // ���Ō�ɒu����Prefab�̍��W��ێ�

        for (int i = 0; i < startStraight; i++)
        {
            manager.StartStraightCoords.Add(current);

            Instantiate(
                startStraightPrefab,
                new Vector3(current.x, current.y, manager.floorZ),
                Quaternion.identity,
                startParent
            );

            lastPlaced = current;  // �����ŕێ�
            current += Vector2Int.down;
        }

        // �Ō�ɒu����Prefab�̏��Door��z�u
        StartDoor.position = new Vector3(lastPlaced.x, lastPlaced.y, manager.doorZ);
    }
}
