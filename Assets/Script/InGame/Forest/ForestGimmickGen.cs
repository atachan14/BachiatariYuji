using System.Collections.Generic;
using UnityEngine;

public class ForestGimmickGen : SingletonMonoBehaviour<ForestGimmickGen>
{
    [Header("生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float baseSpawnChance = 0.01f; // 1% 基準
    [SerializeField] int exclusionRadius = 2; // Start/Goal周囲除外範囲

    [Header("Prefabリスト")]
    [SerializeField] List<GameObject> wallGimmickPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> floorGimmickPrefabs = new List<GameObject>();

    [Header("生成先トランスフォーム")]
    [SerializeField] Transform wallGimmickParent;
    [SerializeField] Transform floorGimmickParent;

    public void Generate()
    {
        var floorCoords = ForestGenManager.Instance.MainFloorCoords;
        var rng = ForestGenManager.Instance.Rng;

        // 除外エリア計算
        HashSet<Vector2Int> exclusionZone = GetExclusionZone();

        // FloorGimmick配置
        foreach (var pos in floorCoords)
        {
            if (exclusionZone.Contains(pos)) continue;
            if (RollSpawn(rng))
            {
                GameObject prefab = PickWeightedGimmick(floorGimmickPrefabs, rng);
                if (prefab != null)
                    Instantiate(prefab, new Vector3(pos.x, pos.y, ForestGenManager.Instance.floorGimmickZ),
                        Quaternion.identity, floorGimmickParent);
            }
        }

        // WallGimmick配置
        HashSet<Vector2Int> emptyAround = FindEmptyAround(floorCoords);
        foreach (var pos in emptyAround)
        {
            if (exclusionZone.Contains(pos)) continue;
            if (RollSpawn(rng))
            {
                GameObject prefab = PickWeightedGimmick(wallGimmickPrefabs, rng);
                if (prefab != null)
                    Instantiate(prefab, new Vector3(pos.x, pos.y, ForestGenManager.Instance.floorGimmickZ),
                        Quaternion.identity, wallGimmickParent);
                ForestGenManager.Instance.GimmickCoords.Add(pos);
            }
        }
    }

    private HashSet<Vector2Int> GetExclusionZone()
    {
        var zone = new HashSet<Vector2Int>();

        for (int dx = -exclusionRadius; dx <= exclusionRadius; dx++)
        {
            for (int dy = -exclusionRadius; dy <= exclusionRadius; dy++)
            {
              
            }
        }
        return zone;
    }

    private HashSet<Vector2Int> FindEmptyAround(HashSet<Vector2Int> floorCoords)
    {
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var f in floorCoords)
        {
            foreach (var d in dirs)
            {
                Vector2Int candidate = f + d;
                if (!floorCoords.Contains(candidate))
                    result.Add(candidate);
            }
        }
        return result;
    }

    private bool RollSpawn(System.Random rng)
    {
        float evilFactor = 1f + GameData.Instance.TotalEvil / 10000f;
        float chance = baseSpawnChance * evilFactor;
        return rng.NextDouble() < chance;
    }

    /// <summary>
    /// SOの重み付けを使ってPrefabを選択
    /// </summary>
    private GameObject PickWeightedGimmick(List<GameObject> prefabs, System.Random rng)
    {
        if (prefabs == null || prefabs.Count == 0) return null;

        float totalEvil = GameData.Instance.TotalEvil;
        float totalWeight = 0f;
        List<float> weights = new List<float>();

        foreach (var prefab in prefabs)
        {
            var gimmick = prefab.GetComponent<ForestGimmick>();
            if (gimmick == null || gimmick.so == null)
            {
                weights.Add(0f);
                continue;
            }

            var so = gimmick.so;

            // ガウス分布っぽい重み付け
            float diff = totalEvil - so.preferredLevel;
            float gauss = Mathf.Exp(-(diff * diff) / (2f * so.sigma * so.sigma));

            float weight = so.baseWeight + so.rarity * gauss;
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return null;

        // ルーレット選択
        float pick = (float)(rng.NextDouble() * totalWeight);
        float accum = 0f;

        for (int i = 0; i < prefabs.Count; i++)
        {
            accum += weights[i];
            if (pick <= accum)
                return prefabs[i];
        }

        return prefabs[prefabs.Count - 1]; // fallback
    }
}
