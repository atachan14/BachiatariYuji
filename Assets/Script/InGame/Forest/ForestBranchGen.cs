using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestBranchGen : SingletonMonoBehaviour<ForestBranchGen>
{
    [Header("中継地点パラメータ")]
    [SerializeField] int maxIntermediateRadius = 2;
    [SerializeField] int minIntermediateRadius = 2;
    [SerializeField, Range(0f, 1f)] float branchTurnChance = 0.2f;

    [Header("Prefabs")]
    [SerializeField] GameObject branchPrefab;
    [SerializeField] GameObject intermediatePointPrefab;

    [Header("生成先トランスフォーム")]
    [SerializeField] Transform branchParent;

    private RectInt mapArea;
    private List<Vector2Int> candidates;
    ForestGenManager manager ;
    System.Random rng;

    public void Generate()
    {
        manager = ForestGenManager.Instance;
        rng = manager.Rng;

        mapArea = CalculateMapArea(manager.MainFloorCoords);

        int currentRadius = maxIntermediateRadius;

        while (currentRadius >= minIntermediateRadius)
        {
            bool generatedAny = false;

            while (true)
            {
                candidates = FindIntermediateCandidates(currentRadius);

                if (candidates.Count == 0) break;

                Shuffle(candidates);

                foreach (var candidate in candidates.ToList())
                {
                    if (TryIntermediatePoint(candidate))
                    {
                        generatedAny = true;
                        break; // 1つ生成したら候補更新のために break
                    }
                }
            }

            if (!generatedAny)
            {
                // この半径ではもう生成できない → 半径を1下げる
                currentRadius--;
            }
            else
            {
                // 生成できた場合は同じ半径で続行
            }
        }

        Debug.Log("中継地点生成 完了！");
    }


    private RectInt CalculateMapArea(HashSet<Vector2Int> allCoords)
    {
        int minX = allCoords.Min(f => f.x);
        int maxX = allCoords.Max(f => f.x);
        int minY = allCoords.Min(f => f.y);
        int maxY = allCoords.Max(f => f.y);

        if (minY < 0) maxY -= minY;
        return new RectInt(minX, 0, maxX - minX + 1, maxY + 1);
    }

    private List<Vector2Int> FindIntermediateCandidates(int currentRadius)
    {
        var manager = ForestGenManager.Instance;
        var list = new List<Vector2Int>();

        foreach (var x in Enumerable.Range(mapArea.xMin, mapArea.width))
            foreach (var y in Enumerable.Range(mapArea.yMin, mapArea.height))
            {
                var pos = new Vector2Int(x, y);
                if (manager.AllOccupiedCoords.Contains(pos)) continue;

                if (IsAreaEmpty(pos, currentRadius))
                    list.Add(pos);
            }

        return list;
    }

    private bool IsAreaEmpty(Vector2Int pos, int radius)
    {
        var manager = ForestGenManager.Instance;

        for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (manager.AllOccupiedCoords.Contains(pos + new Vector2Int(dx, dy)))
                    return false;
            }

        return true;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // 中継地点生成
    private bool TryIntermediatePoint(Vector2Int candidate)
    {
        // 1歩目を上下左右からランダムで決定
        var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        dirs = dirs.OrderBy(_ => rng.Next()).ToArray();

        foreach (var firstDir in dirs)
        {
            foreach (var secondDir in dirs)
            {
                if (firstDir == secondDir) continue;

                if (TryGenerateTwoBranches(candidate, firstDir, secondDir, rng))
                {
                    RegisterIntermediatePoint(candidate);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryGenerateTwoBranches(Vector2Int candidate, Vector2Int dirA, Vector2Int dirB, System.Random rng)
    {

        bool aConnected = SimulateBranch(candidate, dirA, rng, out var pathA);
        bool bConnected = SimulateBranch(candidate, dirB, rng, out var pathB);

        if (!aConnected || !bConnected)
            return false;

        // 成功したら両方登録
        RegisterPath(pathA);
        RegisterPath(pathB);

        return true;
    }

    private bool SimulateBranch(Vector2Int start, Vector2Int dir, System.Random rng, out List<Vector2Int> path)
    {
        path = new List<Vector2Int>();
        var manager = ForestGenManager.Instance;

        Vector2Int pos = start;
        bool connected = false;

        while (mapArea.Contains(pos))
        {
            // 曲がる前に隣接Floor/Branchがあれば接続
            if (CheckAdjacent(pos, out Vector2Int adjacent))
            {
                // 既存Floor/Branchに接続 → ここでは追加しない
                connected = true;
                break;
            }
            // クネクネ
            if (rng.NextDouble() < branchTurnChance)
                dir = TurnDirection(dir, rng);

            pos += dir;

            if (manager.AllOccupiedCoords.Contains(pos))
            {
                connected = true;
                break;
            }

            path.Add(pos);
        }

        return connected;
    }

    private bool CheckAdjacent(Vector2Int pos, out Vector2Int adjacent)
    {
        var manager = ForestGenManager.Instance;
        var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var d in dirs)
        {
            var check = pos + d;
            if (manager.AllOccupiedCoords.Contains(check))
            {
                adjacent = check;
                return true;
            }
        }

        adjacent = default;
        return false;
    }

    private void RegisterIntermediatePoint(Vector2Int pos)
    {
        var manager = ForestGenManager.Instance;
        manager.BranchCoords.Add(pos);

        if (intermediatePointPrefab != null)
            Instantiate(intermediatePointPrefab, new Vector3(pos.x, pos.y, manager.floorZ),
                        Quaternion.identity, branchParent);
    }

    private void RegisterPath(List<Vector2Int> path)
    {
        var manager = ForestGenManager.Instance;
        foreach (var p in path)
        {
            manager.BranchCoords.Add(p);

            if (branchPrefab != null)
                Instantiate(branchPrefab, new Vector3(p.x, p.y, manager.floorZ),
                            Quaternion.identity, branchParent);
        }
    }

    private Vector2Int TurnDirection(Vector2Int currentDir, System.Random rng)
    {
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }
}
