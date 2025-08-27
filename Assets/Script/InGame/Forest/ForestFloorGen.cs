using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestFloorGen : SingletonMonoBehaviour<ForestFloorGen>
{
    [Header("Distance生成パラメータ")]
    [SerializeField] float dayRatio = 0.05f;
    [SerializeField] float totalEvilRatio = 0.0001f;
    [SerializeField] int baseDistance = 200;

    [Header("クネクネ生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float turnChance = 0.1f;
    [SerializeField] int startStraight = 3;
    [SerializeField] int goalStraight = 3;


    [Header("生成オブジェクト")]
    public GameObject floorPrefab;
    public GameObject branchPrefab;

    [Header("生成先トランスフォーム")]
    public Transform floorParent;

    private HashSet<Vector2Int> floorCoords = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> branchCoords = new HashSet<Vector2Int>();
    private Vector2Int startPos;
    private Vector2Int goalPos;

    public void Generate()
    {
        floorCoords.Clear();
        startPos = Vector2Int.zero;

        int pathLength = CalculatePathLength();

        GenerateMainPath(pathLength);
        goalPos = FindGoalPosition();
        PlaceFloors();

        ForestGenManager.Instance.FloorCoords = floorCoords;
    }

    // =============================
    // メインルート生成
    // =============================
    private void GenerateMainPath(int pathLength)
    {
        Vector2Int currentPos = startPos;
        Vector2Int dir = Vector2Int.up;

        floorCoords.Add(startPos);

        // Start直進
        for (int i = 0; i < startStraight; i++)
        {
            currentPos += dir;
            floorCoords.Add(currentPos);
        }

        // クネクネ経路
        for (int i = startStraight; i < pathLength - goalStraight; i++)
        {
            if (ForestGenManager.Instance.Rng.NextDouble() < turnChance)
                dir = TurnDirection(dir);

            Vector2Int nextPos = currentPos + dir;

            // Start周囲禁止 or 既に通ったマスは禁止
            if (IsInStartExclusionZone(nextPos) || floorCoords.Contains(nextPos))
            {
                // 方向変えてみる
                dir = TurnDirection(dir);
                nextPos = currentPos + dir;

                // それでもダメなら逆方向
                if (IsInStartExclusionZone(nextPos) || floorCoords.Contains(nextPos))
                {
                    nextPos = currentPos - dir;
                }
            }

            currentPos = nextPos;
            floorCoords.Add(currentPos);

           
        }

        // ゴール直進
        for (int i = 0; i < goalStraight; i++)
        {
            currentPos += dir;
            floorCoords.Add(currentPos);
        }
    }




    // =============================
    // ゴール決定
    // =============================
    private Vector2Int FindGoalPosition()
    {
        Vector2Int farthest = floorCoords.OrderByDescending(f => ManhattanDistance(f, startPos)).First();
        int maxRadius = 10;
        Vector2Int goal = farthest;

        for (int r = 1; r <= maxRadius; r++)
        {
            List<Vector2Int> candidates = new List<Vector2Int>();

            for (int dx = -r; dx <= r; dx++)
            {
                int dy = r - Mathf.Abs(dx);
                Vector2Int[] positions =
                {
                    farthest + new Vector2Int(dx, dy),
                    farthest + new Vector2Int(dx, -dy)
                };

                foreach (var pos in positions)
                {
                    if (floorCoords.Contains(pos) || pos == startPos) continue;
                    candidates.Add(pos);
                }
            }

            if (candidates.Count > 0)
            {
                int idx = ForestGenManager.Instance.Rng.Next(candidates.Count);
                goal = candidates[idx];
                floorCoords.Add(goal);
                break;
            }
        }

        // ゴールと既存Floorを接続
        Vector2Int closest = floorCoords.OrderBy(f => ManhattanDistance(f, goal)).First(f => f != goal);
        Vector2Int cursor = goal;

        while (cursor != closest)
        {
            if (cursor.x < closest.x) cursor.x++;
            else if (cursor.x > closest.x) cursor.x--;
            else if (cursor.y < closest.y) cursor.y++;

            floorCoords.Add(cursor);
        }

        return goal;
    }

    // =============================
    // Floor配置
    // =============================
    private void PlaceFloors()
    {
        var parent = floorParent;

        foreach (var pos in floorCoords)
        {
            // そのマスが枝かどうかを判定する必要がある
            bool isBranch = branchCoords.Contains(pos); // ←後述

            var prefab = isBranch ? branchPrefab : floorPrefab;

            Object.Instantiate(
                prefab,
                new Vector3(pos.x, pos.y, 1f),
                Quaternion.identity,
                parent
            );
        }
        ForestGenManager.Instance.StartDoor.position = new Vector3(startPos.x, startPos.y, 0f);
        ForestGenManager.Instance.GoalDoor.position = new Vector3(goalPos.x, goalPos.y, 0f);

        // SpawnPos配置
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Vector2Int neighbor = goalPos + dir;
            if (floorCoords.Contains(neighbor))
            {
                ForestGenManager.Instance.SpawnPos.position =
                    new Vector3(neighbor.x, neighbor.y, 0f);
                break;
            }
        }
    }

    // =============================
    // ユーティリティ
    // =============================
    private int CalculatePathLength()
    {
        return Mathf.RoundToInt(baseDistance + GameData.Instance.Day * dayRatio + GameData.Instance.TotalEvil * totalEvilRatio);
    }

    private Vector2Int TurnDirection(Vector2Int currentDir)
    {
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return ForestGenManager.Instance.Rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return ForestGenManager.Instance.Rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }

    private Vector2Int RandomDirection()
    {
        int r = ForestGenManager.Instance.Rng.Next(4);
        return r switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.down,
            2 => Vector2Int.left,
            _ => Vector2Int.right,
        };
    }

    /// <summary>
    /// Start周囲の除外エリア（半径2マス → 25マス）
    /// </summary>
    private bool IsInStartExclusionZone(Vector2Int pos)
    {
        return Mathf.Abs(pos.x - startPos.x) <= 2 &&
               Mathf.Abs(pos.y - startPos.y) <= 2;
    }

    private int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
