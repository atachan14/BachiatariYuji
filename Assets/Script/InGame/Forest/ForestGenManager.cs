using System.Collections.Generic;
using UnityEngine;

public class ForestGenManager : SingletonMonoBehaviour<ForestGenManager>
{
    [Header("ã§óLèÓïÒ")]



    [field: SerializeField] public Transform StartDoor { get; set; }
    [field: SerializeField] public Transform GoalDoor { get; set; }
    [field: SerializeField] public Transform SpawnPos { get; set; }
    public System.Random Rng { get; private set; }
    public HashSet<Vector2Int> FloorCoords { get; set; } = new();
    public HashSet<Vector2Int> BranchCoords { get; set; } = new();
    public HashSet<Vector2Int> GimmickCoords { get; set; } = new();
    public HashSet<Vector2Int> WallCoords { get; set; } = new();


    [Header("Zä«óù")]
    public float floorZ = 1;
    public float floorGimmickZ = 0.9f;
    public float wallZ = 0;

    public void Generate()
    {
        Rng = new System.Random(GameData.Instance.DaySeed);
        ForestFloorGen.Instance.Generate();
        ForestBranchGen.Instance.Generate();
        ForestGimmickGen.Instance.Generate();
        ForestWallGen.Instance.Generate();
    }
}

