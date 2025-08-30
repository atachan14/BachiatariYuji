using System.Collections.Generic;
using UnityEngine;

public class ForestFloorGen : SingletonMonoBehaviour<ForestFloorGen>
{
    [Header("Distance生成パラメータ")]
    [SerializeField] float dayRatio = 0.04f;
    [SerializeField] int baseDistance = 200;

    [Header("クネクネ生成パラメータ")]
    [SerializeField, Range(0f, 1f)] float turnChance = 0.4f;

    private int pathLength;
    private Vector2Int startPos;

    ForestManager manager;

    public void Generate()
    {
        manager = ForestManager.Instance;

        startPos = Vector2Int.zero;

        CalculatePathLength();
        GenerateMainPath();
    }

    // =============================
    // メインルート生成
    // =============================
    private void CalculatePathLength()
    {
        pathLength = Mathf.RoundToInt(
            baseDistance +
            GameData.Instance.Day * dayRatio 
        );
    }

    private void GenerateMainPath()
    {
        Vector2Int currentPos = startPos;
        Vector2Int dir = Vector2Int.up;

        manager.Register(currentPos,TileType.MainFloor);

        while (manager.MainFloorCoords.Count < pathLength)
        {
            if (manager.Rng.NextDouble() < turnChance)
                dir = manager.TurnDirection(dir);

            Vector2Int nextPos = currentPos + dir;

            // y<0は回避
            if (nextPos.y < 0)
                dir = manager.TurnDirection(dir);

            currentPos += dir;

            // 登録済みでも currentPos は進める
            if (!manager.MainFloorCoords.Contains(currentPos))
                manager.Register(currentPos, TileType.MainFloor);
        }
    }

   

    
}
