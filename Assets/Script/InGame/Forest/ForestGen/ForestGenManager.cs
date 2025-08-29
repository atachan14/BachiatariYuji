using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    StartStraight,
    MainFloor,
    Branch,
    GoalStraight,
    FloorOmen,
    WallOmen,
    BeyondOmen,
    InnerWall,
    OuterWall
}

public class ForestGenManager : SingletonMonoBehaviour<ForestGenManager>
{
    #region Field
    [Header("共有情報")]
    public System.Random Rng { get; private set; }

    [Header("Map情報")]
    // 各種座標
    public HashSet<Vector2Int> StartStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> MainFloorCoords { get; private set; } = new();
    public HashSet<Vector2Int> BranchCoords { get; private set; } = new();
    public HashSet<Vector2Int> GoalStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> OmenCoords { get; private set; } = new();
    public HashSet<Vector2Int> InnerWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> OuterWallCoords { get; private set; } = new();

    // 差分更新型
    public HashSet<Vector2Int> NotStraightCoords { get; private set; } = new();
    public HashSet<Vector2Int> AllOccupiedCoords { get; private set; } = new();

    [Header("Z管理")]
    public float doorZ = 0;
    public float floorZ = 1;
    public float floorGimmickZ = 0.9f;
    public float wallZ = 0;

    [Header("Prefab & Parent")]
    public List<GameObject> startStraightPrefabs;
    public List<GameObject> mainFloorPrefabs;
    public List<GameObject> branchPrefabs;
    public List<GameObject> goalStraightPrefabs;
    public List<GameObject> floorOmenPrefabs;
    public List<GameObject> wallOmenPrefabs;
    public List<GameObject> beyondOmenPrefabs;
    public List<GameObject> innerWallPrefabs;
    public List<GameObject> outerWallPrefabs;

    public Transform startStraightParent;
    public Transform mainFloorParent;
    public Transform branchParent;
    public Transform goalStraightParent;
    public Transform floorOmenParent;
    public Transform wallOmenParent;
    public Transform beyondOmenParent;
    public Transform innerWallParent;
    public Transform outerWallParent;
    #endregion

    public void Generate()
    {
        Rng = new System.Random(GameData.Instance.DaySeed);

        ForestStartGen.Instance.Generate();
        ForestFloorGen.Instance.Generate();
        ForestBranchGen.Instance.Generate();
        ForestGoalGen.Instance.Generate();
        ForestInnerWallGen.Instance.Generate();
        ForestOmenGen.Instance.Generate();
        ForestOuterWallGen.Instance.Generate();
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
            case TileType.WallOmen: RegisterWallOmen(pos); break;
            case TileType.FloorOmen: RegisterFloorOmen(pos); break;
            case TileType.BeyondOmen: RegisterBeyondOmen(pos); break;
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
        var prefab = RandomPick(startStraightPrefabs);
        StartStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, startStraightParent);
    }

    private void RegisterFloor(Vector2Int pos)
    {
        var prefab = RandomPick(mainFloorPrefabs);
        MainFloorCoords.Add(pos);
        NotStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, mainFloorParent);
    }

    private void RegisterBranch(Vector2Int pos)
    {
        var prefab = RandomPick(branchPrefabs);
        BranchCoords.Add(pos);
        NotStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, branchParent);
    }

    private void RegisterGoal(Vector2Int pos)
    {
        var prefab = RandomPick(goalStraightPrefabs);
        GoalStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorZ), Quaternion.identity, goalStraightParent);
    }
    private void RegisterInnerWall(Vector2Int pos)
    {
        var prefab = RandomPick(innerWallPrefabs);
        InnerWallCoords.Add(pos);
        NotStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, innerWallParent);
    }

    private void RegisterFloorOmen(Vector2Int pos)
    {
        var prefab = OmenWeightedPick(floorOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorGimmickZ), Quaternion.identity, floorOmenParent);
    }
    private void RegisterWallOmen(Vector2Int pos)
    {
        var prefab = OmenWeightedPick(wallOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, wallOmenParent);
    }

    void RegisterBeyondOmen(Vector2Int pos)
    {
        var prefab = OmenWeightedPick(beyondOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, beyondOmenParent);
    }
    private void RegisterOuterWall(Vector2Int pos)
    {
        var prefab = RandomPick(outerWallPrefabs);
        OuterWallCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, outerWallParent);
    }

    #endregion

    #region Utility
    public Vector2Int TurnDirection(Vector2Int currentDir)
    {
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return Rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return Rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }

    public List<GameObject> GetAllOmen()
    {
        var allOmen = new List<GameObject>();
        allOmen.AddRange(floorOmenPrefabs);
        allOmen.AddRange(wallOmenPrefabs);
        allOmen.AddRange(beyondOmenPrefabs);
        return allOmen;
    }


    #endregion

    #region Pick
    private GameObject RandomPick(List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0) return prefabs[prefabs.Count - 1];
        return prefabs[Rng.Next(prefabs.Count)];
    }

    private GameObject OmenWeightedPick(List<GameObject> prefabs)
    {

        float totalEvil = GameData.Instance.TotalEvil;
        float totalWeight = 0f;
        List<float> weights = new List<float>();

        foreach (var prefab in prefabs)
        {
            var omen = prefab.GetComponentInChildren<Omen>();
            if (omen == null || omen.omenDestiny == null) { weights.Add(0f); continue; }

            var so = omen.omenDestiny;
            float diff = totalEvil - so.preferredLevel;
            float gauss = Mathf.Exp(-(diff * diff) / (2f * so.sigma * so.sigma));
            float weight = so.baseWeight + so.rarity * gauss;
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return prefabs[prefabs.Count - 1];

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