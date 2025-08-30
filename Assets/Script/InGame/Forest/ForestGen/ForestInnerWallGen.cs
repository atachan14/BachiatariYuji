using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestInnerWallGen : SingletonMonoBehaviour<ForestInnerWallGen>
{
    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        var rng = manager.Rng;

        // ---- Walkable�̎��͂ɕK��Wall ----
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var f in manager.AllOccupiedCoords.ToList())
        {
            foreach (var d in dirs)
            {
                var pos = f + d;

                // ���ɉ�����L����Ă�Ȃ�X�L�b�v
                if (manager.AllOccupiedCoords.Contains(pos)) continue;

                // Register�o�R��InnerWall��z�u
                manager.Register(pos, TileType.SoftWall);
            }
        }
    }
}
