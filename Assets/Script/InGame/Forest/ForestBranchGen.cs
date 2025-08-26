using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestBranchGen : SingletonMonoBehaviour<ForestBranchGen>
{
    [Header("Branch�����p�����[�^")]
    [SerializeField, Range(0f, 1f)] float branchTurnChance = 0.2f;
    [SerializeField] int maxBranchLength = 6;
    [SerializeField] int minBranchLength = 2;
    [SerializeField] int intermediateRadius = 1; // ���p�n�_�̎��͋󂫃}�X���a

    [Header("Branch Prefab")]
    public GameObject branchPrefab;

    [Header("������g�����X�t�H�[��")]
    public Transform branchParent;

    /// <summary>
    /// mainFloors: �{��Floor���W��HashSet
    /// </summary>
    public void Generate()
    {
        var rng = ForestGenManager.Instance.Rng;
        HashSet<Vector2Int> mainFloors = ForestGenManager.Instance.FloorCoords;
        HashSet<Vector2Int> occupied = new HashSet<Vector2Int>(mainFloors);

        // ���p�n�_�����擾
        var intermediatePoints = mainFloors.Where(f =>
            CountAdjacentEmpty(f, occupied) >= 2).ToList();

        foreach (var point in intermediatePoints)
        {
            // 1�̒��p�n�_���畡�������Ɏ}�𐶐�
            foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                if (rng.NextDouble() < 0.5) continue; // �������邩�����_���Ō���

                GenerateBranchFrom(point, dir, mainFloors, occupied, rng);
            }
        }
    }

    private void GenerateBranchFrom(Vector2Int startPos, Vector2Int initialDir, HashSet<Vector2Int> mainFloors, HashSet<Vector2Int> occupied, System.Random rng)
    {
        Vector2Int dir = initialDir;
        int length = rng.Next(minBranchLength, maxBranchLength + 1);
        Vector2Int pos = startPos;
        HashSet<Vector2Int> branchCoords = ForestGenManager.Instance.BranchCoords;

        for (int i = 0; i < length; i++)
        {
            pos += dir;

            if (occupied.Contains(pos)) break;

            branchCoords.Add(pos);

            if (rng.NextDouble() < branchTurnChance)
                dir = RandomTurn(dir, rng);
        }

        // �}�̍ŏI�n�_����{��Floor�ɐڑ�
        Vector2Int target = FindClosestFloor(pos, mainFloors);
        var path = GetPathTo(pos, target);
        branchCoords.UnionWith(path.Where(p => !occupied.Contains(p)));

        // ���W��o�^����Prefab�𐶐�
        foreach (var bPos in branchCoords)
        {
            occupied.Add(bPos);
            if (branchPrefab != null)
            {
                Object.Instantiate(
                    branchPrefab,
                    new Vector3(bPos.x, bPos.y, ForestGenManager.Instance.floorZ),
                    Quaternion.identity,
                    branchParent
                );
            }
        }
    }

    private Vector2Int RandomTurn(Vector2Int currentDir, System.Random rng)
    {
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }

    private int CountAdjacentEmpty(Vector2Int pos, HashSet<Vector2Int> occupied)
    {
        int count = 0;
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var d in dirs)
        {
            if (!occupied.Contains(pos + d)) count++;
        }
        return count;
    }

    private Vector2Int FindClosestFloor(Vector2Int from, HashSet<Vector2Int> mainFloors)
    {
        return mainFloors.OrderBy(f => ManhattanDistance(f, from)).First();
    }

    private IEnumerable<Vector2Int> GetPathTo(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int cursor = from;

        while (cursor != to)
        {
            if (cursor.x < to.x) cursor.x++;
            else if (cursor.x > to.x) cursor.x--;
            else if (cursor.y < to.y) cursor.y++;
            else if (cursor.y > to.y) cursor.y--;

            path.Add(cursor);
        }

        return path;
    }

    private int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
