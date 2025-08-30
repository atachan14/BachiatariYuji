using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestOuterWallGen : SingletonMonoBehaviour<ForestOuterWallGen>
{
    ForestManager manager;

    public void Generate()
    {
        manager = ForestManager.Instance;

        // �����̌��𖄂߂�
        HoleWallGen();

        // �O���ǂ𐶐�
        EdgeWallGen();
    }

    void HoleWallGen()
    {
        var occupiedSet = new HashSet<Vector2Int>(manager.AllOccupiedCoords);
        var holeCandidates = new List<Vector2Int>();

        // y���ƂɃO���[�v�����č��E�����̌����
        var groupedByY = manager.AllOccupiedCoords.GroupBy(c => c.y);
        foreach (var group in groupedByY)
        {
            var xs = group.Select(c => c.x).OrderBy(x => x).ToList();
            for (int i = 0; i < xs.Count - 1; i++)
            {
                int start = xs[i];
                int end = xs[i + 1];
                // �A�����ĂȂ������������Ƃ��Ēǉ�
                for (int x = start + 1; x < end; x++)
                {
                    holeCandidates.Add(new Vector2Int(x, group.Key));
                }
            }
        }

        // �㉺��Occupied�ň͂܂�Ă��邩�m�F���āAInnerHole�Ƃ��ēo�^
        foreach (var candidate in holeCandidates)
        {
            if (IsVerticallyEnclosed(candidate, occupiedSet))
            {
                manager.Register(candidate, TileType.HoleWall);
                occupiedSet.Add(candidate); // ���̌�┻��p�ɒǉ�
            }
        }
    }

    bool IsVerticallyEnclosed(Vector2Int pos, HashSet<Vector2Int> occupied)
    {
        // �������Occupied�����邩
        bool upBlocked = occupied.Any(c => c.x == pos.x && c.y > pos.y);
        // ��������Occupied�����邩
        bool downBlocked = occupied.Any(c => c.x == pos.x && c.y < pos.y);

        return upBlocked && downBlocked;
    }

    void EdgeWallGen()
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        var allCoords = new List<Vector2Int>(manager.AllOccupiedCoords); // �R�s�[

        foreach (var occupied in allCoords)
        {
            foreach (var d in dirs)
            {
                var pos = occupied + d;
                if (manager.AllOccupiedCoords.Contains(pos)) continue;
                manager.Register(pos, TileType.EdgeWall);
            }
        }
    }
}
