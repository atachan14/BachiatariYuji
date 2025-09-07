using UnityEngine;

public class FloorOmen : Omen
{
    public override void Spawn(GameObject prefabRoot)
    {
        var manager = ForestManager.Instance;
        SpawnPrefab(prefabRoot,
            ForestOmenGen.Instance.FloorCandidates,
            manager.mainFloorParent,
            manager.floorOmenParent,
            manager.MainFloorCoords,
            manager.OmenCoords,
            manager.floorZ);
    }
}