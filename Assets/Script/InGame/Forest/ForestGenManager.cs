using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    StartStraight,
    MainFloor,
    Branch,
    GoalStraight,
    WallGimmick,
    InnerGimmick,
    InnerWall,
    OuterWall
}

public class ForestGenManager : SingletonMonoBehaviour<ForestGenManager>
{
    #region Field
    [Header("共有情報")]
    public System.Random Rng { get; private set; }

    // 各種座標
    public HashSet<Vector2Int> StartStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> MainFloorCoords { get; private set; } = new();
    public HashSet<Vector2Int> BranchCoords { get; private set; } = new();
    public HashSet<Vector2Int> GoalStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> GimmickCoords { get; private set; } = new();
    public HashSet<Vector2Int> InnerWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> OuterWallCoords { get; private set; } = new();

    // 差分更新型
    public HashSet<Vector2Int> FloorAndBranchCoords { get; private set; } = new();
    public HashSet<Vector2Int> WalkableCoords { get; private set; } = new();
    public HashSet<Vector2Int> AllOccupiedCoords { get; private set; } = new();

    [Header("Z管理")]
    public float doorZ = 0;
    public float floorZ = 1;
    public float floorGimmickZ = 0.9f;
    public float wallZ = 0;

    [Header("Prefab & Parent")]
    public List<GameObject> startStraightPrefab;
    public List<GameObject> mainFloorPrefab;
    public List<GameObject> branchPrefab;
    public List<GameObject> goalStraightPrefab;
    public List<GameObject> wallGimmickPrefab;
    public List<GameObject> floorGimmickPrefab;
    public List<GameObject> innerWallPrefab;
    public List<GameObject> outerWallPrefab;

    public Transform startStraightParent;
    public Transform mainFloorParent;
    public Transform branchParent;
    public Transform goalStraightParent;
    public Transform wallGimmickParent;
    public Transform floorGimmickParent;
    public Transform innerWallParent;
    public Transform outerWallParent;
    #endregion

    public void Generate()
    {
        Rng = new System.Random(GameData.Instance.DaySeed);

        ForestStartGen.Instance.Generate();
        ForestFloorGen.Instance.Generate();
        ForestBranchGen.Instance.Generate();
        //ForestGoalGen.Instance.Generate();
        //ForestGimmickGen.Instance.Generate();
        //ForestInnerWallGen.Instance.Generate();
        //ForestOuterWallGen.Instance.Generate();
    }

    #region Register
    public void Register(Vector2Int pos, TileType type)
    {
        switch (type)
        {
            case TileType.StartStraight: RegisterStart(pos); break;
            case TileType.MainFloor: RegisterFloor(pos); break;
            case TileType.Branch: RegisterBranch(pos); break;
            case TileType.GoalStraight: RegisterGoal(pos); break;
            case TileType.WallGimmick: RegisterWallGimmick(pos); break;
            case TileType.InnerGimmick: RegisterFloorGimmick(pos); break;
            case TileType.InnerWall: RegisterInnerWall(pos); break;
            case TileType.OuterWall: RegisterOuterWall(pos); break;
            default: Debug.LogWarning($"未対応のTileType: {type}"); break;
        }
    }
    
    public void Register(IEnumerable<Vector2Int> positions, TileType type)
    {
        foreach (var pos in positions)
            Register(pos, type);
    }

    private void RegisterStart(Vector2Int pos)
    {
        var prefab = RandomPick(startStraightPrefab);
        StartStraightCoords.Add(pos);
        WalkableCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, startStraightParent);
    }

    private void RegisterFloor(Vector2Int pos)
    {
        var prefab = RandomPick(mainFloorPrefab);
        MainFloorCoords.Add(pos);
        FloorAndBranchCoords.Add(pos);
        WalkableCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, mainFloorParent);
    }

    private void RegisterBranch(Vector2Int pos)
    {
        var prefab = RandomPick(branchPrefab);
        BranchCoords.Add(pos);
        FloorAndBranchCoords.Add(pos);
        WalkableCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, branchParent);
    }

    private void RegisterGoal(Vector2Int pos)
    {
        var prefab = RandomPick(goalStraightPrefab);
        GoalStraightCoords.Add(pos);
        WalkableCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, goalStraightParent);
    }

    private void RegisterWallGimmick(Vector2Int pos)
    {
        var prefab = PickWeightedGimmick(wallGimmickPrefab);
        GimmickCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, wallGimmickParent);
    }

    private void RegisterFloorGimmick(Vector2Int pos)
    {
        var prefab = PickWeightedGimmick(floorGimmickPrefab);
        GimmickCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorGimmickZ), Quaternion.identity, floorGimmickParent);
    }

    private void RegisterInnerWall(Vector2Int pos)
    {
        var prefab = RandomPick(innerWallPrefab);
        InnerWallCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, innerWallParent);
    }

    private void RegisterOuterWall(Vector2Int pos)
    {
        var prefab = RandomPick(outerWallPrefab);
        OuterWallCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, outerWallParent);
    }

    #endregion

    #region Utility
    public Vector2Int TurnDirection(Vector2Int currentDir)
    {
        var rng = ForestGenManager.Instance.Rng;
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }
    #endregion

    #region Pick
    private GameObject RandomPick(List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0) return null;
        return prefabs[Rng.Next(prefabs.Count)];
    }

    private GameObject PickWeightedGimmick(List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0) return null;

        float totalEvil = GameData.Instance.TotalEvil;
        float totalWeight = 0f;
        List<float> weights = new List<float>();

        foreach (var prefab in prefabs)
        {
            var gimmick = prefab.GetComponent<ForestGimmick>();
            if (gimmick == null || gimmick.so == null) { weights.Add(0f); continue; }

            var so = gimmick.so;
            float diff = totalEvil - so.preferredLevel;
            float gauss = Mathf.Exp(-(diff * diff) / (2f * so.sigma * so.sigma));
            float weight = so.baseWeight + so.rarity * gauss;
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return null;

        float pick = (float)(Rng.NextDouble() * totalWeight);
        float accum = 0f;

        for (int i = 0; i < prefabs.Count; i++)
        {
            accum += weights[i];
            if (pick <= accum) return prefabs[i];
        }

        return prefabs[prefabs.Count - 1];
    }
    #endregion

}