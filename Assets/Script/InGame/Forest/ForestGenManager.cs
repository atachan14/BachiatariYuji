using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestGenManager : SingletonMonoBehaviour<ForestGenManager>
{
    [Header("���L���")]
    public System.Random Rng { get; private set; }

    public HashSet<Vector2Int> StartStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> MainFloorCoords { get; private set; } = new();
    public HashSet<Vector2Int> BranchCoords { get; private set; } = new();
    public HashSet<Vector2Int> GoalStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> GimmickCoords { get; private set; } = new();
    public HashSet<Vector2Int> InnerWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> OuterWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> AllOccupiedCoords => new HashSet<Vector2Int>(MainFloorCoords.Concat(BranchCoords));


    [Header("Z�Ǘ�")]
    public float doorZ = 0;
    public float floorZ = 1;
    public float floorGimmickZ = 0.9f;
    public float wallZ = 0;

    public void Generate()
    {
        Rng = new System.Random(GameData.Instance.DaySeed);

        ForestStartGen.Instance.Generate();   // �������� StartAround �ݒ�
        ForestFloorGen.Instance.Generate();
        ForestBranchGen.Instance.Generate();
        //ForestGoalGen.Instance.Generate();    // �������� GoalAround �ݒ�
        //ForestGimmickGen.Instance.Generate();
        //ForestInnerWallGen.Instance.Generate();
        //ForestOuterWallGen.Instance.Generate();
    }
   
}