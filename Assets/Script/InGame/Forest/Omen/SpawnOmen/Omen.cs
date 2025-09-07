// === Omen.cs ===
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Omen : MonoBehaviour
{
    public OmenDestinySO destiny;
    [SerializeField] List<GameObject> punishes;

    public void SelectPunish()
    {
        var selectedPunish = OmenManager.Instance.PunishDestinyPick(punishes);
        if (selectedPunish != null)
            selectedPunish.SetActive(true);
    }

    // prefabルートを受け取るように変更
    public abstract void Spawn(GameObject prefabRoot);

    // 共通ユーティリティ
    protected void SpawnPrefab(
        GameObject prefabRoot,
        HashSet<Vector2Int> candidateSet,
        Transform oldParent,
        Transform parent,
        HashSet<Vector2Int> oldCategory,
        HashSet<Vector2Int> newCategory,
        float zPos)
    {
        if (candidateSet == null || candidateSet.Count == 0)
        {
            Debug.LogWarning($"[Omen] {prefabRoot.name} の候補が空です");
            return;
        }

        var manager = ForestManager.Instance;
        var pos = candidateSet.ElementAt(manager.Rng.Next(candidateSet.Count));

        // 既存削除（位置一致）
        Transform oldObj = null;
        foreach (Transform child in oldParent)
        {
            if (Vector2Int.RoundToInt(child.position) == pos)
            {
                oldObj = child;
                break;
            }
        }
        if (oldObj != null) Destroy(oldObj.gameObject);

        // Prefabルートを生成
        GameObject newObj = Object.Instantiate(prefabRoot, new Vector3(pos.x, pos.y, zPos), Quaternion.identity, parent);

        // Coords更新
        oldCategory.Remove(pos);
        newCategory.Add(pos);

        candidateSet.Remove(pos);

        Debug.Log($"[Omen] {prefabRoot.name} を {pos} に生成");
    }
}
