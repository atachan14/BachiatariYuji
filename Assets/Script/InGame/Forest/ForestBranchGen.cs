using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestBranchGen : SingletonMonoBehaviour<ForestBranchGen>
{
    [Header("中継地点パラメータ")]
    [SerializeField] private int maxIntermediateRadius = 2;
    [SerializeField] private int minIntermediateRadius = 2;
    [SerializeField, Range(0f, 1f)] private float branchTurnChance = 0.2f;


    private RectInt mapArea;
    private List<Vector2Int> candidates;

    private ForestGenManager manager;
    private System.Random rng;

    /// <summary>
    /// デバッグ用
    /// </summary>

    List<Vector2Int> intermediate = new();

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
                        break; // 1つ生成したら候補更新のため break
                    }
                }

                // すべて試して生成できなければ内側ループ終了
                 if (!generatedAny) break; // これで内側ループ終了
            }

            if (!generatedAny)
                currentRadius--; // 半径を下げて再挑戦
        }

        Debug.Log("Branch生成 完了！");
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

    private List<Vector2Int> FindIntermediateCandidates(int radius)
    {
        var list = new List<Vector2Int>();

        for (int x = mapArea.xMin; x < mapArea.xMax; x++)
            for (int y = mapArea.yMin; y < mapArea.yMax; y++)
            {
                var pos = new Vector2Int(x, y);
                if (manager.FloorAndBranchCoords.Contains(pos)) continue;

                if (IsAreaEmpty(pos, radius))
                    list.Add(pos);
            }

        return list;
    }

    private bool IsAreaEmpty(Vector2Int pos, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
                if (manager.FloorAndBranchCoords.Contains(pos + new Vector2Int(dx, dy)))
                    return false;

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

    private bool TryIntermediatePoint(Vector2Int candidate)
    {
        var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right }
                   .OrderBy(_ => rng.Next()).ToArray();

        foreach (var firstDir in dirs)
        {
            foreach (var secondDir in dirs)
            {
                if (firstDir == secondDir) continue;

                // 1マス目の座標を計算して被っていたらスキップ
                Vector2Int firstStep = candidate + firstDir;
                Vector2Int secondStep = candidate + secondDir;
                if (firstStep == secondStep) continue;

                if (TryGenerateTwoBranches(candidate, firstDir, secondDir))
                {
                    manager.Register(candidate, TileType.Branch);
                    intermediate.Add(candidate);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryGenerateTwoBranches(Vector2Int candidate, Vector2Int dirA, Vector2Int dirB)
    {
        Vector2Int firstStepA = candidate + dirA;
        Vector2Int firstStepB = candidate + dirB;

        // 1マス目が被ったら失敗
        if (firstStepA == firstStepB) return false;

        // 最初の1マス目を先に登録して path に追加
        List<Vector2Int> pathA = new List<Vector2Int> { firstStepA };
        List<Vector2Int> pathB = new List<Vector2Int> { firstStepB };

        bool aConnected = SimulateBranch(firstStepA, dirA, out var restA);
        bool bConnected = SimulateBranch(firstStepB, dirB, out var restB);

        if (!aConnected || !bConnected) return false;

        pathA.AddRange(restA);
        pathB.AddRange(restB);

        foreach (var p in pathA.Concat(pathB))
            manager.Register(p, TileType.Branch);

        return true;
    }


    private bool SimulateBranch(Vector2Int start, Vector2Int dir, out List<Vector2Int> path)
    {
        path = new List<Vector2Int>();
        Vector2Int pos = start;
        List<Vector2Int> tempPath = new List<Vector2Int>();

        while (mapArea.Contains(pos))
        {
            // 隣接チェックで接続完了
            if (CheckAdjacent(pos, out _))
            {
                path = tempPath;
                return true;
            }

            // ランダム方向変更
            if (rng.NextDouble() < branchTurnChance)
                dir = manager.TurnDirection(dir);

            // 1マス進む
            pos += dir;

            // Pathに追加（隣接前なので安全）
            tempPath.Add(pos);
        }

        // 到達できなかった場合は失敗
        path.Clear();
        return false;
    }


    private bool CheckAdjacent(Vector2Int pos, out Vector2Int adjacent)
    {
        var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var d in dirs)
        {
            var check = pos + d;
            if (manager.FloorAndBranchCoords.Contains(check))
            {
                adjacent = check;
                return true;
            }
        }

        adjacent = default;
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (manager == null) return;

        // 候補座標を青で表示
        if (candidates != null)
        {
            Gizmos.color = Color.blue;
            foreach (var c in candidates)
            {
                Gizmos.DrawSphere(new Vector3(c.x, c.y, 0), 0.2f);
            }
        }

        // 採用された中継地点を赤で表示

        Gizmos.color = Color.red;
        foreach (var p in intermediate)
        {
            Gizmos.DrawCube(new Vector3(p.x, p.y, 0), Vector3.one * 0.3f);
        }
    }
#endif
}
