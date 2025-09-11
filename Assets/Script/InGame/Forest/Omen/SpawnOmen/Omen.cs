using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Omen : MonoBehaviour
{
    public OmenDestinySO destiny;
    [SerializeField] List<GameObject> punishes;

    // �ǉ�: SpawnMode
    public enum SpawnMode
    {
        Replace, // �����폜���Ēu������
        Overlay  // �����c���ď�ɐ���
    }

    [SerializeField] protected SpawnMode spawnMode = SpawnMode.Replace;

    public void SelectPunish()
    {
        var selectedPunish = OmenManager.Instance.PunishDestinyPick(punishes);
        if (selectedPunish != null)
            selectedPunish.SetActive(true);
    }

    // prefab���[�g���󂯎��悤�ɕύX
    public abstract void Spawn(GameObject prefabRoot);

    // ���ʃ��[�e�B���e�B
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
            Debug.LogWarning($"[Omen] {prefabRoot.name} �̌�₪��ł�");
            return;
        }

        var manager = ForestManager.Instance;
        var pos = candidateSet.ElementAt(manager.Rng.Next(candidateSet.Count));

        // �� ���[�h���Ƃ̏�������
        if (spawnMode == SpawnMode.Replace)
        {
            // �����폜�i�ʒu��v�j
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
        }

        // Prefab���[�g�𐶐��iReplace�ł�Overlay�ł����ʁj
        GameObject newObj = Object.Instantiate(
            prefabRoot,
            new Vector3(pos.x, pos.y, zPos),
            Quaternion.identity,
            parent
        );

        // Replace �̏ꍇ���� oldCategory ���X�V
        if (spawnMode == SpawnMode.Replace)
        {
            oldCategory.Remove(pos);
        }

        // Overlay�ł�Replace�ł� newCategory �ɂ͓o�^
        newCategory.Add(pos);

        candidateSet.Remove(pos);

    }
}