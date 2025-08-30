using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestOuterWallGen : SingletonMonoBehaviour<ForestOuterWallGen>
{
    ForestManager manager;

    public void Generate()
    {
        manager = ForestManager.Instance;

        // 内陸の穴を埋める
        HoleWallGen();

        // 外周壁を生成
        EdgeWallGen();
    }

    void HoleWallGen()
    {
        var occupiedSet = new HashSet<Vector2Int>(manager.AllOccupiedCoords);
        var holeCandidates = new List<Vector2Int>();

        // yごとにグループ化して左右方向の穴を列挙
        var groupedByY = manager.AllOccupiedCoords.GroupBy(c => c.y);
        foreach (var group in groupedByY)
        {
            var xs = group.Select(c => c.x).OrderBy(x => x).ToList();
            for (int i = 0; i < xs.Count - 1; i++)
            {
                int start = xs[i];
                int end = xs[i + 1];
                // 連続してない部分を穴候補として追加
                for (int x = start + 1; x < end; x++)
                {
                    holeCandidates.Add(new Vector2Int(x, group.Key));
                }
            }
        }

        // 上下もOccupiedで囲まれているか確認して、InnerHoleとして登録
        foreach (var candidate in holeCandidates)
        {
            if (IsVerticallyEnclosed(candidate, occupiedSet))
            {
                manager.Register(candidate, TileType.HoleWall);
                occupiedSet.Add(candidate); // 次の候補判定用に追加
            }
        }
    }

    bool IsVerticallyEnclosed(Vector2Int pos, HashSet<Vector2Int> occupied)
    {
        // 上方向にOccupiedがあるか
        bool upBlocked = occupied.Any(c => c.x == pos.x && c.y > pos.y);
        // 下方向にOccupiedがあるか
        bool downBlocked = occupied.Any(c => c.x == pos.x && c.y < pos.y);

        return upBlocked && downBlocked;
    }

    void EdgeWallGen()
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        var allCoords = new List<Vector2Int>(manager.AllOccupiedCoords); // コピー

        foreach (var occupied in allCoords)
        {
            foreach (var d in dirs)
            {
                var pos = occupied + d;
                if (manager.AllOccupiedCoords.Contains(pos)) continue;
                manager.Register(pos, TileType.EdgeWall);
            }
        }
    }
}
