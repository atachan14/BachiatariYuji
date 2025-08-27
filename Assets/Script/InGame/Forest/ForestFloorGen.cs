using System.Collections.Generic;
using UnityEngine;

public class ForestFloorGen : SingletonMonoBehaviour<ForestFloorGen>
{
    [Header("Distance生成パラメータ")]
    [SerializeField] float dayRatio = 0.05f;
    [SerializeField] float totalEvilRatio = 0.0001f;
    [SerializeField] int baseDistance = 200;

    [Header("クネクネ生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float turnChance = 0.1f;

    [Header("生成オブジェクト")]
    [SerializeField] GameObject floorPrefab;

    [Header("生成先トランスフォーム")]
    [SerializeField] Transform floorParent;

    private int pathLength;
    private Vector2Int startPos;

    public void Generate()
    {
        var manager = ForestGenManager.Instance;
        manager.MainFloorCoords.Clear();

        startPos = Vector2Int.zero;

        CalculatePathLength();
        GenerateMainPath();
        PlaceFloors();
    }

    // =============================
    // メインルート生成
    // =============================
    private void CalculatePathLength()
    {
        pathLength = Mathf.RoundToInt(
            baseDistance +
            GameData.Instance.Day * dayRatio +
            GameData.Instance.TotalEvil * totalEvilRatio
        );
    }

    private void GenerateMainPath()
    {
        var manager = ForestGenManager.Instance;
        Vector2Int currentPos = startPos;
        Vector2Int dir = Vector2Int.up;

        manager.MainFloorCoords.Add(currentPos);

        for (int i = 0; i < pathLength; i++)
        {
            if (manager.Rng.NextDouble() < turnChance)
                dir = TurnDirection(dir);

            Vector2Int nextPos = currentPos + dir;

            // y < 0 または既に通ったマスは禁止
            if (nextPos.y < 0 || manager.MainFloorCoords.Contains(nextPos))
            {
                dir = TurnDirection(dir);
                nextPos = currentPos + dir;

                if (nextPos.y < 0 || manager.MainFloorCoords.Contains(nextPos))
                {
                    nextPos = currentPos - dir;
                    if (nextPos.y < 0) break;
                }
            }

            currentPos = nextPos;
            manager.MainFloorCoords.Add(currentPos);
        }
    }

    // =============================
    // Floor配置
    // =============================
    private void PlaceFloors()
    {
        var manager = ForestGenManager.Instance;
        foreach (var pos in manager.MainFloorCoords)
        {
            Object.Instantiate(
                floorPrefab,
                new Vector3(pos.x, pos.y, manager.floorZ),
                Quaternion.identity,
                floorParent
            );
        }
    }

    // =============================
    // ユーティリティ
    // =============================
    private Vector2Int TurnDirection(Vector2Int currentDir)
    {
        var rng = ForestGenManager.Instance.Rng;
        if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            return rng.NextDouble() < 0.5 ? Vector2Int.left : Vector2Int.right;
        else
            return rng.NextDouble() < 0.5 ? Vector2Int.up : Vector2Int.down;
    }
}
