using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestOmenGen : SingletonMonoBehaviour<ForestOmenGen>
{
    [Header("設定")]
    public int aroundRadius = 2;
    public Color gizmoColor = Color.cyan;

    private HashSet<Vector2Int> eligibleCoords = new();
    private HashSet<Vector2Int> floorCandidates;
    private HashSet<Vector2Int> wallCandidates;
    private HashSet<Vector2Int> holeCandidates;
    private HashSet<Vector2Int> edgeCandidates;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;

        // まず全Occupiedをコピーして候補生成
        eligibleCoords = new HashSet<Vector2Int>(manager.AllOccupiedCoords);

        // Start/Goal周囲を排除
        Vector2Int startDoorPos = Vector2Int.RoundToInt(ForestStartGen.Instance.startDoor.position);
        Vector2Int goalDoorPos = Vector2Int.RoundToInt(ForestGoalGen.Instance.goalDoor.position);
        eligibleCoords.ExceptWith(GetAround(startDoorPos, aroundRadius));
        eligibleCoords.ExceptWith(GetAround(goalDoorPos, aroundRadius));

        // 座標セットごとの候補
        floorCandidates = new HashSet<Vector2Int>(manager.MainFloorCoords.Intersect(eligibleCoords));
        wallCandidates = new HashSet<Vector2Int>(manager.SoftWallCoords.Intersect(eligibleCoords));
        holeCandidates = new HashSet<Vector2Int>(manager.HoleWallCoords.Intersect(eligibleCoords));
        edgeCandidates = new HashSet<Vector2Int>(manager.EdgeWallCoords.Intersect(eligibleCoords));

        // 抽選回数は全候補数
        int drawCount = eligibleCoords.Count;
        Debug.Log($"[OmenGen] 抽選回数: {drawCount}");

        var allOmens = manager.GetAllOmen();

        for (int i = 0; i < drawCount; i++)
        {
            var picked = manager.OmenDestinyPick(allOmens);
            if (picked == null)
            {
                Debug.LogWarning("[OmenGen] picked が null");
                continue;
            }

            var omenComponent = picked.GetComponentInChildren<Omen>();
            if (omenComponent == null)
            {
                Debug.LogWarning($"[OmenGen] {picked.name} に Omen コンポーネントがない");
                continue;
            }

            switch (omenComponent.destiny.omenType)
            {
                case OmenType.NoneOmen:
                    SpawnNone(picked);
                    break;
                case OmenType.FloorOmen:
                    SpawnFloorOmen(picked);
                    break;
                case OmenType.WallOmen:
                    SpawnWallOmen(picked);
                    break;
                case OmenType.BeyondOmen:
                    SpawnBeyondOmen(picked);
                    break;
            }
        }
    }

    #region 共通ユーティリティ

    private IEnumerable<Vector2Int> GetAround(Vector2Int center, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
                if (dx != 0 || dy != 0)
                    yield return new Vector2Int(center.x + dx, center.y + dy);
    }

    public HashSet<Vector2Int> GetAround(IEnumerable<Vector2Int> coords, int radius)
    {
        var result = new HashSet<Vector2Int>();
        foreach (var pos in coords)
            result.UnionWith(GetAround(pos, radius));
        return result;
    }

    private void SpawnOmen(GameObject omen, HashSet<Vector2Int> candidateSet, Transform parent, HashSet<Vector2Int> oldCategory, HashSet<Vector2Int> newCategory, float zPos)
    {
        if (candidateSet == null || candidateSet.Count == 0)
        {
            Debug.LogWarning($"[OmenGen] {omen.name} の候補が空です");
            return;
        }

        var manager = ForestGenManager.Instance;
        var pos = candidateSet.ElementAt(manager.Rng.Next(candidateSet.Count));

        // 既存Prefab削除
        Transform oldObj = null;
        foreach (Transform child in parent)
        {
            if (Vector2Int.RoundToInt(child.position) == pos)
            {
                oldObj = child;
                break;
            }
        }
        if (oldObj != null) Destroy(oldObj.gameObject);

        // 新しいOmen生成
        Instantiate(omen, new Vector3(pos.x, pos.y, zPos), Quaternion.identity, parent);

        // Coords更新
        oldCategory.Remove(pos);
        newCategory.Add(pos);

        // ✅ 生成済みは候補から削除
        candidateSet.Remove(pos);
    }


    #endregion

    #region 各種Spawn

    private void SpawnNone(GameObject omen)
    {
        Debug.Log($"[OmenGen] None: {omen.name}");
        // 何もしない
    }

    private void SpawnFloorOmen(GameObject omen)
    {
        Debug.Log($"[OmenGen] Foor: {omen.name}");
        var manager = ForestGenManager.Instance;
        SpawnOmen(omen, floorCandidates, manager.floorOmenParent, manager.MainFloorCoords, manager.OmenCoords, manager.floorZ);
    }

    private void SpawnWallOmen(GameObject omen)
    {
        Debug.Log($"[OmenGen] Wall: {omen.name}");
        var manager = ForestGenManager.Instance;
        SpawnOmen(omen, wallCandidates, manager.wallOmenParent, manager.SoftWallCoords, manager.OmenCoords, manager.wallZ);
    }

    private void SpawnBeyondOmen(GameObject omen)
    {
        Debug.Log($"[OmenGen] Beyond: {omen.name}");
        var manager = ForestGenManager.Instance;
        HashSet<Vector2Int> pickSet;

        // holeCandidatesがある場合は80%で優先
        if (holeCandidates != null && holeCandidates.Count > 0 && manager.Rng.NextDouble() < 0.8)
        {
            pickSet = holeCandidates;
        }
        else
        {
            pickSet = edgeCandidates;
        }

        // Hole + Edgeのどちらかから選択
        var oldSet = new HashSet<Vector2Int>(holeCandidates.Union(edgeCandidates));
        SpawnOmen(omen, pickSet, manager.floorOmenParent, oldSet, manager.OmenCoords, manager.wallZ);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        if (eligibleCoords == null) return;
        foreach (var pos in eligibleCoords)
            Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.1f);
    }
}
