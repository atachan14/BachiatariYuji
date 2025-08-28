using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestGoalGen : SingletonMonoBehaviour<ForestGoalGen>
{
    [SerializeField] private int candidatesNum = 3;
    [SerializeField] private int goalStraight = 3;
    [SerializeField] private Transform goalDoor;

    private ForestGenManager manager;
    private System.Random rng;

    public void Generate()
    {
        manager = ForestGenManager.Instance;
        rng = manager.Rng;

        // 0. �G���A����
        int goalArea = DetermineGoalArea();

        // 1. ListA: �ő�y�̍��W�Q
        var listA = GetTopYCoords();
        if (listA.Count == 0) return;

        // 2. ListB: Goal�G���A���̍��W�Ɍ���
        var listB = FilterByArea(listA, goalArea);
        if (listB.Count == 0) listB = listA;

        // 3. ���_����̋����Ń\�[�g���Č����i��
        var topCandidates = PickTopCandidates(listB);

        // 4. �����_����1�I��
        var posA = SelectRandom(topCandidates);

        // 5. GoalStraight��L�΂�
        var finalPos = PlaceGoalStraight(posA);

        // 6. �S�[���h�A�ݒu
        goalDoor.position=new Vector3(finalPos.x,finalPos.y,manager.doorZ);

        Debug.Log($"Goal���������I����W:{posA} �� GoalDoor:{finalPos} (�G���A:{goalArea})");
    }

    private int DetermineGoalArea()
    {
        int minX = manager.AllOccupiedCoords.Min(p => p.x);
        int maxX = manager.AllOccupiedCoords.Max(p => p.x);
        int width = maxX - minX + 1;

        int leftBoundary = minX + width / 3;
        int rightBoundary = minX + (width * 2) / 3;

        Vector2Int origin = Vector2Int.zero;

        int originArea;
        if (origin.x <= leftBoundary) originArea = 0;       // ��
        else if (origin.x >= rightBoundary) originArea = 2; // �E
        else originArea = 1;                                // ����

        if (originArea == 0) return 2;
        if (originArea == 2) return 0;
        return 1;
    }

    private List<Vector2Int> GetTopYCoords()
    {
        int maxY = manager.AllOccupiedCoords.Max(p => p.y);
        return manager.AllOccupiedCoords.Where(p => p.y == maxY).ToList();
    }

    private List<Vector2Int> FilterByArea(List<Vector2Int> listA, int goalArea)
    {
        int minX = manager.AllOccupiedCoords.Min(p => p.x);
        int maxX = manager.AllOccupiedCoords.Max(p => p.x);
        int width = maxX - minX + 1;

        int leftBoundary = minX + width / 3;
        int rightBoundary = minX + (width * 2) / 3;

        return listA.Where(p =>
        {
            if (p.x <= leftBoundary) return goalArea == 0;
            if (p.x >= rightBoundary) return goalArea == 2;
            return goalArea == 1;
        }).ToList();
    }

    private List<Vector2Int> PickTopCandidates(List<Vector2Int> listB)
    {
        return listB.OrderByDescending(p => p.sqrMagnitude)
                    .Take(candidatesNum)
                    .ToList();
    }

    private Vector2Int SelectRandom(List<Vector2Int> candidates)
    {
        return candidates[rng.Next(candidates.Count)];
    }

    private Vector2Int PlaceGoalStraight(Vector2Int startPos)
    {
        Vector2Int pos = startPos;
        for (int i = 0; i < goalStraight; i++)
        {
            pos += Vector2Int.up;
            manager.Register(pos, TileType.GoalStraight);
        }
        return pos;
    }

}
