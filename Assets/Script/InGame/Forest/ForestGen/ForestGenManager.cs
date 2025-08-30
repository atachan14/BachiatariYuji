using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum TileType
{
    StartStraight,
    MainFloor,
    Branch,
    GoalStraight,

    SoftWall,
    HoleWall,
    EdgeWall,

    FloorOmen,
    WallOmen,
    BeyondOmen

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
    public HashSet<Vector2Int> SoftWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> HoleWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> EdgeWallCoords { get; private set; } = new();
    public HashSet<Vector2Int> OmenCoords { get; private set; } = new();

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

    public List<GameObject> innerWallPrefabs;
    public List<GameObject> outerWallPrefabs;

    public List<GameObject> NoneOmenPrefabs;
    public List<GameObject> floorOmenPrefabs;
    public List<GameObject> wallOmenPrefabs;
    public List<GameObject> beyondOmenPrefabs;

    public Transform startStraightParent;
    public Transform mainFloorParent;
    public Transform branchParent;
    public Transform goalStraightParent;

    public Transform innerWallParent;
    public Transform holeWallParent;
    public Transform edgeWallParent;

    public Transform floorOmenParent;
    public Transform wallOmenParent;
    public Transform beyondOmenParent;
    #endregion

    public void Generate()
    {
        Rng = new System.Random(GameData.Instance.DaySeed);

        ForestStartGen.Instance.Generate();
        ForestFloorGen.Instance.Generate();
        ForestBranchGen.Instance.Generate();
        ForestGoalGen.Instance.Generate();
        ForestInnerWallGen.Instance.Generate();
        ForestOuterWallGen.Instance.Generate();
        ForestOmenGen.Instance.Generate();
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

            case TileType.SoftWall: RegisterSoftWall(pos); break;
            case TileType.HoleWall: RegisterHoleWall(pos); break;
            case TileType.EdgeWall: RegisterEdgeWall(pos); break;

            case TileType.WallOmen: RegisterWallOmen(pos); break;
            case TileType.FloorOmen: RegisterFloorOmen(pos); break;
            case TileType.BeyondOmen: RegisterBeyondOmen(pos); break;

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
    private void RegisterSoftWall(Vector2Int pos)
    {
        var prefab = RandomPick(innerWallPrefabs);
        SoftWallCoords.Add(pos);
        NotStraightCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, innerWallParent);
    }

    private void RegisterFloorOmen(Vector2Int pos)
    {
        var prefab = OmenDestinyPick(floorOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, floorGimmickZ), Quaternion.identity, floorOmenParent);
    }
    private void RegisterWallOmen(Vector2Int pos)
    {
        var prefab = OmenDestinyPick(wallOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, wallOmenParent);
    }

    void RegisterBeyondOmen(Vector2Int pos)
    {
        var prefab = OmenDestinyPick(beyondOmenPrefabs);
        OmenCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, beyondOmenParent);
    }
    private void RegisterHoleWall(Vector2Int pos)
    {
        var prefab = RandomPick(outerWallPrefabs);
        HoleWallCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, holeWallParent);
    }
    private void RegisterEdgeWall(Vector2Int pos)
    {
        var prefab = RandomPick(outerWallPrefabs);
        EdgeWallCoords.Add(pos);
        AllOccupiedCoords.Add(pos);
        Instantiate(prefab, new Vector3(pos.x, pos.y, wallZ), Quaternion.identity, edgeWallParent);
    }

    public void ReplaceAndRegister(Vector2Int pos, GameObject prefab, TileType type)
    {
        // 座標にあるオブジェクトを探してDestroy
        var worldPos = new Vector3(pos.x, pos.y, 0);
        var hits = Physics2D.OverlapPointAll(worldPos); // Colliderがある場合
        foreach (var hit in hits)
            GameObject.Destroy(hit.gameObject);

        // 座標登録から消す
        AllOccupiedCoords.Remove(pos);
        switch (type)
        {
            case TileType.FloorOmen:
                MainFloorCoords.Remove(pos);
                break;
            case TileType.WallOmen:
                SoftWallCoords.Remove(pos);
                break;
            case TileType.BeyondOmen:
                EdgeWallCoords.Remove(pos);
                break;
        }

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
        allOmen.AddRange(NoneOmenPrefabs);
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

    public GameObject OmenDestinyPick(List<GameObject> prefabs)
    {

        float day = GameData.Instance.Day;
        float totalWeight = 0f;
        List<float> weights = new List<float>();

        foreach (var prefab in prefabs)
        {
            var omen = prefab.GetComponentInChildren<Omen>();
            if (omen == null || omen.destiny == null) { weights.Add(0f); continue; }

            var so = omen.destiny;
            float diff = day - so.peakDay;
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