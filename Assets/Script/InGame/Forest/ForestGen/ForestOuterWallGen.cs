using System.Collections.Generic;
using UnityEngine;

public class ForestOuterWallGen : SingletonMonoBehaviour<ForestOuterWallGen>
{
    public void Generate()
    {
        var manager = ForestGenManager.Instance;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        var allCoords = new List<Vector2Int>(manager.AllOccupiedCoords); // ÉRÉsÅ[
        foreach (var occupied in allCoords)
        {
            foreach (var d in dirs)
            {
                var pos = occupied + d;
                if (manager.AllOccupiedCoords.Contains(pos)) continue;
                manager.Register(pos, TileType.OuterWall);
            }
        }

    }
}
