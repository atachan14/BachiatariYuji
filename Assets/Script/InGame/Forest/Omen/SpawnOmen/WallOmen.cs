// === WallOmen.cs ===
using UnityEngine;

public class WallOmen : Omen
{
    public override void Spawn(GameObject prefabRoot)
    {
        var manager = ForestManager.Instance;
        SpawnPrefab(prefabRoot,
            ForestOmenGen.Instance.WallCandidates,
            manager.innerWallParent,
            manager.wallOmenParent,
            manager.SoftWallCoords,
            manager.OmenCoords,
            manager.wallZ);
    }
}
