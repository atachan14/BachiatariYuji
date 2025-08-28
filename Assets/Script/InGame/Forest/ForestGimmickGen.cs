using System.Collections.Generic;
using UnityEngine;

public class ForestGimmickGen : SingletonMonoBehaviour<ForestGimmickGen>
{
    [Header("生成パラメータ")]
    [SerializeField, Range(0f, 1f)] private float baseFloorSpawnChance = 0.01f; // 床上
    [SerializeField, Range(0f, 1f)] private float baseWallSpawnChance = 0.01f;  // 壁用

    private ForestGenManager manager;
    private System.Random rng;

    public void Generate()
    {
        manager = ForestGenManager.Instance;
        rng = manager.Rng;

        // --- InnerGimmick: 床上（Floor + Branch） ---
        foreach (var pos in manager.FloorAndBranchCoords)
        {
            if (RollSpawn(rng, baseFloorSpawnChance))
            {
                manager.Register(pos, TileType.InnerGimmick);
            }
        }

        // --- WallGimmick: 床の周囲（未占有のみ） ---
        var wallCandidates = FindEmptyAround();
        foreach (var pos in wallCandidates)
        {
            if (RollSpawn(rng, baseWallSpawnChance))
            {
                manager.Register(pos, TileType.WallGimmick);
            }
        }
    }

    private HashSet<Vector2Int> FindEmptyAround()
    {
        var result = new HashSet<Vector2Int>();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var f in manager.FloorAndBranchCoords)
        {
            foreach (var d in dirs)
            {
                var c = f + d;
                if (!manager.AllOccupiedCoords.Contains(c))
                    result.Add(c);
            }
        }
        return result;
    }

    private bool RollSpawn(System.Random rng, float baseChance)
    {
        float evilFactor = 1f + GameData.Instance.TotalEvil / 10000f;
        float chance = baseChance * evilFactor;
        return rng.NextDouble() < chance;
    }
}
