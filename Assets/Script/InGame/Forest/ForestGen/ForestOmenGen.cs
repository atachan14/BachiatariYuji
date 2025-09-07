// === ForestOmenGen.cs ===
// 大幅にスリム化
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestOmenGen : SingletonMonoBehaviour<ForestOmenGen>
{
    [Header("設定")]
    public int aroundRadius = 2;
    public Color gizmoColor = Color.cyan;

    public HashSet<Vector2Int> EligibleCoords { get; private set; }
    public HashSet<Vector2Int> FloorCandidates { get; private set; }
    public HashSet<Vector2Int> WallCandidates { get; private set; }
    public HashSet<Vector2Int> HoleCandidates { get; private set; }
    public HashSet<Vector2Int> EdgeCandidates { get; private set; }

    public void Generate()
    {
        var manager = ForestManager.Instance;

        EligibleCoords = new HashSet<Vector2Int>(manager.AllOccupiedCoords);

        // Start/Goal周囲を排除
        Vector2Int startDoorPos = Vector2Int.RoundToInt(ForestStartGen.Instance.startDoor.position);
        Vector2Int goalDoorPos = Vector2Int.RoundToInt(ForestGoalGen.Instance.goalDoor.position);
        EligibleCoords.ExceptWith(GetAround(startDoorPos, aroundRadius));
        EligibleCoords.ExceptWith(GetAround(goalDoorPos, aroundRadius));

        FloorCandidates = new HashSet<Vector2Int>(manager.MainFloorCoords.Intersect(EligibleCoords));
        WallCandidates = new HashSet<Vector2Int>(manager.SoftWallCoords.Intersect(EligibleCoords));
        HoleCandidates = new HashSet<Vector2Int>(manager.HoleWallCoords.Intersect(EligibleCoords));
        EdgeCandidates = new HashSet<Vector2Int>(manager.EdgeWallCoords.Intersect(EligibleCoords));

        int drawCount = EligibleCoords.Count;
        Debug.Log($"[OmenGen] 抽選回数: {drawCount}");

        var allOmens = manager.GetAllOmen();

        // === ForestOmenGen.cs ===
        // 呼び出し部分を prefabRoot を渡すように変更
        for (int i = 0; i < drawCount; i++)
        {
            var picked = manager.OmenDestinyPick(allOmens);
            if (picked == null) continue;

            var omenComponent = picked.GetComponentInChildren<Omen>();
            if (omenComponent == null) continue;

            // Prefabルートごと渡す！
            omenComponent.Spawn(picked);
        }

    }

    private IEnumerable<Vector2Int> GetAround(Vector2Int center, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
                if (dx != 0 || dy != 0)
                    yield return new Vector2Int(center.x + dx, center.y + dy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        if (EligibleCoords == null) return;
        foreach (var pos in EligibleCoords)
            Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.1f);
    }
}
