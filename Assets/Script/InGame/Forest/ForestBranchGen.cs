using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestBranchGen : SingletonMonoBehaviour<ForestBranchGen>
{
    [Header("中継地点パラメータ")]
    [SerializeField] int intermediateRadius = 2;      // 周囲何マス空いているか
    [SerializeField] int maxIntermediateCount = 4;    // 最大中継地点数
    [SerializeField] int minAngleThreshold = 90;      // 最小角度。失敗したら45度まで許可

    [Header("Branch Prefab")]
    public GameObject branchPrefab;

    [Header("生成先トランスフォーム")]
    public Transform branchParent;

    // マップ領域（FloorとBranchの最外周で囲む）
    private RectInt mapArea;

    public void Generate()
    {
        var rng = ForestGenManager.Instance.Rng;
        var floors = ForestGenManager.Instance.FloorCoords;
        var branches = ForestGenManager.Instance.BranchCoords;
        var occupied = new HashSet<Vector2Int>(floors.Concat(branches));

        // 1. マップ領域を計算
        mapArea = CalculateMapArea(floors);

        // 2. 中継地点候補を収集
        var candidates = FindIntermediateCandidates(occupied);

        // 3. シャッフルしてランダム順に処理
        Shuffle(candidates, rng);

        int usedCount = 0;
        foreach (var candidate in candidates)
        {
            if (usedCount >= maxIntermediateCount) break;

            if (TryGenerateBranchesFrom(candidate, occupied, rng))
            {
                usedCount++;
            }
        }

        if (usedCount < maxIntermediateCount)
        {
            Debug.LogWarning($"ForestBranchGen: 中継地点が不足しました ({usedCount}/{maxIntermediateCount})");
        }
    }


    // --- マップ領域を作る ---
    private RectInt CalculateMapArea(HashSet<Vector2Int> floors)
    {
        int minX = floors.Min(f => f.x);
        int maxX = floors.Max(f => f.x);
        int minY = floors.Min(f => f.y);
        int maxY = floors.Max(f => f.y);
        return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }

    // --- 中継地点候補を探す ---
    private List<Vector2Int> FindIntermediateCandidates(HashSet<Vector2Int> occupied)
    {
        var candidates = new List<Vector2Int>();

        for (int x = mapArea.xMin; x <= mapArea.xMax; x++)
        {
            for (int y = mapArea.yMin; y <= mapArea.yMax; y++)
            {
                var pos = new Vector2Int(x, y);

                if (occupied.Contains(pos)) continue;

                // 周囲 intermediateRadius マス以内に何もなければ候補
                if (IsAreaEmpty(pos, occupied, intermediateRadius))
                {
                    candidates.Add(pos);
                }
            }
        }
        return candidates;
    }

    private bool IsAreaEmpty(Vector2Int pos, HashSet<Vector2Int> occupied, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (occupied.Contains(pos + new Vector2Int(dx, dy)))
                    return false;
            }
        }
        return true;
    }

    private void Shuffle<T>(List<T> list, System.Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }


    // --- 中継地点から枝生成を試みる ---
    private bool TryGenerateBranchesFrom(Vector2Int candidate, HashSet<Vector2Int> occupied, System.Random rng)
    {
        var hitDirs = FindHitDirections(candidate, occupied);

        if (hitDirs.Count < 2) return false;

        // ランダム順に並べ替えてみる（偏り防止）
        hitDirs = hitDirs.OrderBy(_ => rng.Next()).ToList();

        bool created = false;

        // すべての組み合わせをチェック
        for (int i = 0; i < hitDirs.Count; i++)
        {
            for (int j = i + 1; j < hitDirs.Count; j++)
            {
                float angle = AngleBetween(hitDirs[i], hitDirs[j]);
                if (angle >= minAngleThreshold || angle >= 45)
                {
                    CreateBranch(candidate, hitDirs[i], occupied);
                    CreateBranch(candidate, hitDirs[j], occupied);
                    created = true;
                }
            }
        }

        return created;
    }

    // --- 方向探索（Floor/Branchに当たるまで進む）---
    private List<Vector2Int> FindHitDirections(Vector2Int origin, HashSet<Vector2Int> occupied)
    {
        var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        var hits = new List<Vector2Int>();

        foreach (var dir in dirs)
        {
            Vector2Int pos = origin;
            while (mapArea.Contains(pos))
            {
                pos += dir;
                if (occupied.Contains(pos))
                {
                    hits.Add(dir);
                    break;
                }
            }
        }
        return hits;
    }

    // --- 枝を生成（まっすぐ進んで既存Floor/Branchに接続）---
    private void CreateBranch(Vector2Int start, Vector2Int dir, HashSet<Vector2Int> occupied)
    {
        // まず中継地点自身を枝にする
        if (!occupied.Contains(start))
        {
            occupied.Add(start);
            ForestGenManager.Instance.BranchCoords.Add(start);

            if (branchPrefab != null)
            {
                Object.Instantiate(
                    branchPrefab,
                    new Vector3(start.x, start.y, ForestGenManager.Instance.floorZ),
                    Quaternion.identity,
                    branchParent
                );
            }
        }

        // そこから伸ばす
        Vector2Int pos = start;
        while (mapArea.Contains(new Vector2Int(pos.x, pos.y)))
        {
            pos += dir;
            if (occupied.Contains(pos)) break; // 接続したら終了

            occupied.Add(pos);
            ForestGenManager.Instance.BranchCoords.Add(pos);

            if (branchPrefab != null)
            {
                Object.Instantiate(
                    branchPrefab,
                    new Vector3(pos.x, pos.y, ForestGenManager.Instance.floorZ),
                    Quaternion.identity,
                    branchParent
                );
            }
        }
    }


    // --- ベクトル角度 ---
    private float AngleBetween(Vector2Int a, Vector2Int b)
    {
        Vector2 normA = ((Vector2)a).normalized;
        Vector2 normB = ((Vector2)b).normalized;
        return Vector2.Angle(normA, normB); // 0〜180
    }

#if UNITY_EDITOR
    // --- デバッグ用に中継地点候補をGizmosで表示 ---
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        var occupied = new HashSet<Vector2Int>(
            ForestGenManager.Instance.FloorCoords.Concat(ForestGenManager.Instance.BranchCoords));

        if (occupied.Count == 0) return;

        mapArea = CalculateMapArea(occupied);
        var candidates = FindIntermediateCandidates(occupied);

        Gizmos.color = Color.cyan;
        foreach (var c in candidates)
        {
            Gizmos.DrawSphere(new Vector3(c.x, c.y, ForestGenManager.Instance.floorZ), 0.2f);
        }
    }
#endif
}
