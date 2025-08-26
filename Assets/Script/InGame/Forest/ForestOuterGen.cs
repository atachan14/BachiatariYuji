using UnityEngine;
/*
public class ForestOuterGen : SingletonMonoBehaviour<ForestOuterGen>
{
    [Header("生成サイズ基準")]
    [SerializeField] int baseTotalSize = 50;
    [SerializeField] float dayCoef = 0.05f;
    [SerializeField] float evilCoef = 0.0001f;
    [SerializeField] int minDimension = 6;

    [Header("サイズ振れ幅（正規分布）")]
    [SerializeField, Range(0.01f, 1f)] float jitterStdDev = 0.25f; // 総マス数の揺れ
    [SerializeField, Range(0.01f, 1f)] float ratioStdDev = 0.25f; // 縦横比の揺れ

    [Header("ランダム制御")]
    [SerializeField] int seed = -1;  // -1ならランダム、指定で固定

    [Header("配置スケール / ドア余白")]
    [SerializeField] float cellSize = 1f;
    [SerializeField, Min(0)] int doorEdgeMargin = 1; // 角からどれだけ離すか

    int width, height;

    public void Generate()
    {
        // --- ① シード初期化 ---
        int useSeed = (seed == -1) ? GameData.Instance.DaySeed : seed;
        Random.InitState(useSeed);

        // TODO: 実運用では GameData.Instance.Day / TotalEvil を渡す
        ComputeSize(1, 0f, out width, out height);

        // --- ② 結果を Manager に保存 ---
        ForestGenManager.Instance.Width = width;
        ForestGenManager.Instance.Height = height;
        ForestGenManager.Instance.CellSize = cellSize;

        ClearChildren(ForestGenManager.Instance.outerWallsParent);
        BuildOuterWalls();
        ConfigurationDoor();

        Debug.Log($"[ForestOuterGen] size = {width} x {height}, seed={useSeed}");
    }

    // ===== サイズ決定 =====
    void ComputeSize(int day, float totalEvil, out int w, out int h)
    {
        float dayMul = 1f + (day - 1) * dayCoef;
        float evilMul = 1f + totalEvil * evilCoef;

        int baseTotal = Mathf.RoundToInt(baseTotalSize * dayMul * evilMul);

        // 総サイズ：下限のみ保証。上は正規分布の出目しだいで無限。
        float jitterFactor = SampleNormal(1f, jitterStdDev); // Clampしない
        int total = Mathf.RoundToInt(baseTotal * jitterFactor);
        if (total < minDimension * 2) total = minDimension * 2;

        // 縦横比：0..1に収める
        float t = Mathf.Clamp01(SampleNormal(0.5f, ratioStdDev));
        w = Mathf.RoundToInt(total * t);
        h = total - w;

        if (w < minDimension) w = minDimension;
        if (h < minDimension) h = minDimension;
    }

    float SampleNormal(float mean, float stdDev)
    {
        // Box-Muller
        float u1 = 1f - Random.value;
        float u2 = 1f - Random.value;
        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        return mean + stdDev * z;
    }

    // ===== Start/Goal配置 =====
    void ConfigurationDoor()
    {
        Vector2Int startCell = PickStartOnBottom();
        Vector2Int goalCell = PickGoalOnTopDiagonal(startCell);

        Vector3 startPos = ToWorld(startCell.x, startCell.y);
        Vector3 goalPos = ToWorld(goalCell.x, goalCell.y);

        RemoveWallAt(startPos);
        RemoveWallAt(goalPos);

        // --- Manager に保存 ---
        if (ForestGenManager.Instance.StartDoor != null) ForestGenManager.Instance.StartDoor.position = startPos;
        if (ForestGenManager.Instance.GoalDoor != null) ForestGenManager.Instance.GoalDoor.position = goalPos;

        Debug.Log($"[ForestOuterGen] start={startCell}, goal={goalCell}");
    }

    // 下辺(y=0)固定、xは角を避けてランダム
    Vector2Int PickStartOnBottom()
    {
        int minX = Mathf.Max(1, doorEdgeMargin);
        int maxXEx = Mathf.Max(minX + 1, width - doorEdgeMargin); // 排他的上限
        int sx = Random.Range(minX, maxXEx);
        return new Vector2Int(sx, 0);
    }

    // 上辺(y=height-1)の“反対側ハーフ”からxを選ぶ（角は余白で回避）
    Vector2Int PickGoalOnTopDiagonal(Vector2Int start)
    {
        int topY = height - 1;
        int midX = width / 2;
        bool startOnLeft = start.x < midX;

        int minX, maxXEx;
        if (startOnLeft)
        {
            // スタートが左半分 → ゴールは右半分
            minX = Mathf.Max(midX, doorEdgeMargin);
            maxXEx = Mathf.Max(minX + 1, width - doorEdgeMargin);
        }
        else
        {
            // スタートが右半分 → ゴールは左半分
            minX = Mathf.Max(doorEdgeMargin, 0);
            maxXEx = Mathf.Max(minX + 1, midX);
        }

        // 念のためガード（極端に小さい幅のとき）
        if (minX >= maxXEx)
        {
            minX = Mathf.Clamp(midX, doorEdgeMargin, width - doorEdgeMargin - 1);
            maxXEx = Mathf.Min(width - doorEdgeMargin, minX + 1);
        }

        int gx = Random.Range(minX, maxXEx);
        return new Vector2Int(gx, topY);
    }

    // ===== ユーティリティ =====
    void ClearChildren(Transform t)
    {
        if (t == null) return;
        for (int i = t.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying) Destroy(t.GetChild(i).gameObject);
            else DestroyImmediate(t.GetChild(i).gameObject);
        }
    }

    void BuildOuterWalls()
    {
        for (int x = 0; x < width; x++)
        {
            SpawnWall(x, 0);
            SpawnWall(x, height - 1);
        }
        for (int y = 1; y < height - 1; y++)
        {
            SpawnWall(0, y);
            SpawnWall(width - 1, y);
        }
    }
    
    Vector3 ToWorld(int x, int y) => new Vector3(x * cellSize, y * cellSize, 0f);

    void SpawnWall(int x, int y)
    {
        if (ForestGenManager.Instance.wallPrefab == null || ForestGenManager.Instance.outerWallsParent == null) return;
        var pos = ToWorld(x, y);
        var go = Instantiate(ForestGenManager.Instance.wallPrefab, pos, Quaternion.identity, ForestGenManager.Instance.outerWallsParent);
        go.name = $"Wall_{x}_{y}";
    }

    void RemoveWallAt(Vector3 pos)
    {
        if (ForestGenManager.Instance.outerWallsParent == null) return;
        for (int i = ForestGenManager.Instance.outerWallsParent.childCount - 1; i >= 0; i--)
        {
            var child = ForestGenManager.Instance.outerWallsParent.GetChild(i);
            if (Vector3.Distance(child.position, pos) < 0.1f)
            {
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
        }
    }
}
*/
