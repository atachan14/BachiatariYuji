using System.Collections.Generic;
using UnityEngine;

public class ForestWallGen : SingletonMonoBehaviour<ForestWallGen>
{
    [Header("Prefabリスト")]
    [SerializeField] List<GameObject> wallPrefabs = new List<GameObject>();

    [Header("生成先トランスフォーム")]
    [SerializeField] Transform wallParent;

    [Header("追加Wall生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float extraWallChance = 0.05f; // ランダム壁率
    [SerializeField] int margin = 5; // マップ拡張マージン

    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        var rng = manager.Rng;

        // ---- Floor周囲に必ずWall ----
        HashSet<Vector2Int> sumFloor = new ();
        sumFloor.UnionWith(manager.FloorCoords);
        sumFloor.UnionWith(manager.BranchCoords);

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var f in sumFloor)
        {
            foreach (var d in dirs)
            {
                var pos = f + d;
                if (manager.FloorCoords.Contains(pos)) continue;
                if (manager.BranchCoords.Contains(pos)) continue;
                if (manager.GimmickCoords.Contains(pos)) continue;
                if (manager.WallCoords.Contains(pos)) continue;

                PlaceWall(pos, rng);
            }
        }

        // ---- 追加のランダムWall ----
        // 範囲決定
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var pos in manager.FloorCoords)
        {
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }
        foreach (var pos in manager.GimmickCoords)
        {
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        minX -= margin; maxX += margin;
        minY -= margin; maxY += margin;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var pos = new Vector2Int(x, y);

                if (manager.FloorCoords.Contains(pos)) continue;
                if (manager.GimmickCoords.Contains(pos)) continue;
                if (manager.WallCoords.Contains(pos)) continue;

                if (rng.NextDouble() < extraWallChance)
                {
                    PlaceWall(pos, rng);
                }
            }
        }
    }

    private void PlaceWall(Vector2Int pos, System.Random rng)
    {
        var manager = ForestGenManager.Instance;
        if (wallPrefabs == null || wallPrefabs.Count == 0) return;

        GameObject prefab = wallPrefabs[rng.Next(wallPrefabs.Count)];
        Instantiate(prefab, new Vector3(pos.x, pos.y, manager.wallZ),
            Quaternion.identity, wallParent);

        manager.WallCoords.Add(pos);
    }
}
