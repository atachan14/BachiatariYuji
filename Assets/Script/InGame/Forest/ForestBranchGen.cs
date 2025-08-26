using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestBranchGen : SingletonMonoBehaviour<ForestBranchGen>
{
    [Header("Branch生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float branchTurnChance = 0.2f;
    [SerializeField] int maxBranchLength = 6;
    [SerializeField] int minBranchLength = 2;
    [SerializeField] int intermediateRadius = 1; // 中継地点の周囲空きマス半径

    [Header("Branch Prefab")]
    public GameObject branchPrefab;

    [Header("生成先トランスフォーム")]
    public Transform branchParent;

    /// <summary>
    /// mainFloors: 本筋Floor座標のHashSet
    /// </summary>
    public void Generate()
    {
        var rng = ForestGenManager.Instance.Rng;
        HashSet<Vector2Int> mainFloors = ForestGenManager.Instance.FloorCoords;
        HashSet<Vector2Int> occupied = new HashSet<Vector2Int>(mainFloors);

        // 中継地点候補を取得
        var intermediatePoints = mainFloors.Where(f =>
            CountAdjacentEmpty(f, occupied) >= 2).ToList();

        foreach (var point in intermediatePoints)
        {
            // 1つの中継地点から複数方向に枝を生成
            foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                if (rng.NextDouble() < 0.5) continue; // 生成するかランダムで決定

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

        // 枝の最終地点から本筋Floorに接続
        Vector2Int target = FindClosestFloor(pos, mainFloors);
        var path = GetPathTo(pos, target);
        branchCoords.UnionWith(path.Where(p => !occupied.Contains(p)));

        // 座標を登録してPrefabを生成
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
