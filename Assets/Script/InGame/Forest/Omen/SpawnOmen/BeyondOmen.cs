// === BeyondOmen.cs ===
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BeyondOmen : Omen
{
    public override void Spawn(GameObject prefabRoot)
    {
        var manager = ForestManager.Instance;

        HashSet<Vector2Int> pickSet;
        Transform oldParent;

        if (ForestOmenGen.Instance.HoleCandidates.Count > 0 && manager.Rng.NextDouble() < 0.8)
        {
            pickSet = ForestOmenGen.Instance.HoleCandidates;
            oldParent = manager.holeWallParent;
        }
        else
        {
            pickSet = ForestOmenGen.Instance.EdgeCandidates;
            oldParent = manager.edgeWallParent;
        }

        var oldSet = new HashSet<Vector2Int>(
            ForestOmenGen.Instance.HoleCandidates.Union(ForestOmenGen.Instance.EdgeCandidates)
        );

        SpawnPrefab(prefabRoot,
            pickSet,
            oldParent,
            manager.beyondOmenParent,
            oldSet,
            manager.OmenCoords,
            manager.wallZ);
    }
}
