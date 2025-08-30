using UnityEngine;

public class ForestStartGen : SingletonMonoBehaviour<ForestStartGen>
{
    [SerializeField] public Transform startDoor;

    [Header("�����p�����[�^")]
    [SerializeField] private int startStraight = 4;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;

        Vector2Int zero = Vector2Int.zero;
        Vector2Int current = zero;
        Vector2Int lastPlaced = zero;

        for (int i = 0; i < startStraight; i++)
        {
            // Manager��Register���g��
            manager.Register(current, TileType.StartStraight);

            lastPlaced = current;
            current += Vector2Int.down;
        }

        // �Ō�ɒu����Prefab�̏��Door��z�u
        startDoor.position = new Vector3(lastPlaced.x, lastPlaced.y, manager.doorZ);
    }
}
