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
                // Yujiに直接視界あり → そのまま追いかけ
                transform.position = Vector2.MoveTowards(transform.position, yuji.position, moveSpeed * Time.deltaTime);
                path.Clear();
                pathTimer = 0;
            }
            else
            {
                // 遮蔽物あり → PathChase() で経路追跡
                PathChase();
            }
        }
        else
        {
            // 視界に何も障害がない場合も直接追跡
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
                // ゴールにたどり着けなかった場合は path を null にする
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

        // ゴールにたどり着けなかった
        if (!cameFrom.ContainsKey(goal))
            return null;

        // ゴールからスタートに戻りながらパスを復元
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

        // 今の目的地
        Vector2 targetPos = path[currentIndex];
        Vector2 currentPos = transform.position;

        // 移動
        transform.position = Vector2.MoveTowards(
            currentPos,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // 近づいたら次のノードへ
        if (Vector2.Distance(currentPos, targetPos) < 0.05f)
        {
            currentIndex++;
        }
    }
}
