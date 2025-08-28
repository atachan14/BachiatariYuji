using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestOmenGen : SingletonMonoBehaviour<ForestOmenGen>
{
    [Header("生成パラメータ")]
    [SerializeField, Range(0f, 1f)] private float baseFloorSpawnChance = 0.05f; // 床上
    [SerializeField, Range(0f, 1f)] private float baseWallSpawnChance = 0.05f;  // 壁用
    [SerializeField, Range(0f, 1f)] private float outerInLandSpawnChance = 0.8f;  // 壁外用
    [SerializeField, Range(0f, 1f)] private float outerSpawnChance = 0.05f;  // 壁外用


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
                manager.Register(pos, TileType.FloorOmen);
            }
        }

        // --- WallGimmick: InnerWallと差し替え
        foreach (var pos in manager.InnerWallCoords.ToList()) // ToList()でコピーして列挙中の変更を防ぐ
        {
            if (RollSpawn(rng, baseWallSpawnChance))
            {
                // 親Transformの中から該当座標のPrefabを探して削除
                foreach (Transform child in manager.innerWallParent)
                {
                    if (Vector2Int.RoundToInt(child.position) == pos)
                    {
                        Destroy(child.gameObject);
                        break; // 一つ見つけたらOK
                    }
                }
                // HashSetから削除
                manager.InnerWallCoords.Remove(pos);
                // 差し替え登録
                manager.Register(pos, TileType.WallOmen);
            }
        }


        // --- WallOutGimmick: InnerWallに隣接してる空きマス ---
        var wallCandidates = GetWallOutCandidates();
        foreach (var pos in wallCandidates)
        {
            if (IsInland(pos, manager.AllOccupiedCoords))
            {
                if (RollSpawn(rng, outerInLandSpawnChance))
                {
                    manager.Register(pos, TileType.OuterOmen);
                }
            }
            else
            {
                if (RollSpawn(rng, outerSpawnChance))
                {
                    manager.Register(pos, TileType.OuterOmen);
                }
            }
        }

    }

    private HashSet<Vector2Int> GetWallOutCandidates()
    {
        var result = new HashSet<Vector2Int>();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var f in manager.AllOccupiedCoords)
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

    private bool IsInland(Vector2Int pos, HashSet<Vector2Int> allOccupied)
    {
        bool left = false, right = false, up = false, down = false;

        foreach (var cell in allOccupied)
        {
            if (cell.x < pos.x && cell.y == pos.y) left = true;
            if (cell.x > pos.x && cell.y == pos.y) right = true;
            if (cell.y < pos.y && cell.x == pos.x) down = true;
            if (cell.y > pos.y && cell.x == pos.x) up = true;

            // 全部揃ったら即returnで早期終了できる
            if (left && right && up && down) return true;
        }

        return left && right && up && down;
    }


    private bool RollSpawn(System.Random rng, float baseChance)
    {
        float evilFactor = 1f + GameData.Instance.TotalEvil / 10000f;
        float chance = baseChance * evilFactor;
        return rng.NextDouble() < chance;
    }
}
