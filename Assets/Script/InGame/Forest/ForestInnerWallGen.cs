using System.Collections.Generic;
using UnityEngine;

public class ForestInnerWallGen : SingletonMonoBehaviour<ForestInnerWallGen>
{
    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        var rng = manager.Rng;

        // ---- Walkableの周囲に必ずWall ----
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var f in manager.WalkableCoords)
        {
            foreach (var d in dirs)
            {
                var pos = f + d;

                // 既に何か占有されてるならスキップ
                if (manager.AllOccupiedCoords.Contains(pos)) continue;

                // Register経由でInnerWallを配置
                manager.Register(pos, TileType.InnerWall);
            }
        }
    }
}
