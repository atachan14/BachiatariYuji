using System.Collections.Generic;
using UnityEngine;

public class DogChase : MonoBehaviour
{
    public float moveSpeed = 3f;
    [SerializeField] float findPathCD = 0.5f;

    Transform yuji;
    List<Vector2Int> path = new();
    int currentIndex = 0;
    float pathTimer;

    public void Exe()
    {
        if (yuji == null) yuji = Yuji.Instance.transform;
        pathTimer -= Time.deltaTime;

        Vector2 dir = (yuji.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, yuji.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Wall", "Yuji"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Yuji"))
            {
                // Yuji�ɒ��ڎ��E���� �� ���̂܂ܒǂ�����
                transform.position = Vector2.MoveTowards(transform.position, yuji.position, moveSpeed * Time.deltaTime);
                path.Clear();
                pathTimer = 0;
            }
            else
            {
                // �Օ������� �� PathChase() �Ōo�H�ǐ�
                PathChase();
            }
        }
        else
        {
            // ���E�ɉ�����Q���Ȃ��ꍇ�����ڒǐ�
            transform.position = Vector2.MoveTowards(transform.position, yuji.position, moveSpeed * Time.deltaTime);
        }


    }

    void PathChase()
    {
       
        if (pathTimer <= 0f)
        {
            var newPath = FindPath(
                Vector2Int.RoundToInt(transform.position),
                Vector2Int.RoundToInt(yuji.position),
                ForestManager.Instance.WalkableCoords
            );

            if (newPath != null && newPath.Count > 0)
            {
                path = newPath;
                currentIndex = 0;
            }
            else
            {
                // �S�[���ɂ��ǂ蒅���Ȃ������ꍇ�� path �� null �ɂ���
                path = null;
            }

            pathTimer = findPathCD;
        }

        FollowPath();
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, HashSet<Vector2Int> walkable)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var dir in directions)
            {
                var next = current + dir;

                if (walkable.Contains(next) && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        // �S�[���ɂ��ǂ蒅���Ȃ�����
        if (!cameFrom.ContainsKey(goal))
            return null;

        // �S�[������X�^�[�g�ɖ߂�Ȃ���p�X�𕜌�
        List<Vector2Int> path = new List<Vector2Int>();
        var cur = goal;
        while (cur != start)
        {
            path.Add(cur);
            cur = cameFrom[cur];
        }
        path.Reverse();

        return path;
    }
    void FollowPath()
    {
        if (path == null || path.Count == 0) return;
        if (currentIndex >= path.Count) return;

        // ���̖ړI�n
        Vector2 targetPos = path[currentIndex];
        Vector2 currentPos = transform.position;

        // �ړ�
        transform.position = Vector2.MoveTowards(
            currentPos,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // �߂Â����玟�̃m�[�h��
        if (Vector2.Distance(currentPos, targetPos) < 0.05f)
        {
            currentIndex++;
        }
    }
}
