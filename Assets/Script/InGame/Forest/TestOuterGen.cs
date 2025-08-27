using UnityEngine;

public class TestOuterGen : MonoBehaviour
{
/*
public class ForestOuterGen : SingletonMonoBehaviour<ForestOuterGen>
{
    [Header("�����T�C�Y�")]
    [SerializeField] int baseTotalSize = 50;
    [SerializeField] float dayCoef = 0.05f;
    [SerializeField] float evilCoef = 0.0001f;
    [SerializeField] int minDimension = 6;

    [Header("�T�C�Y�U�ꕝ�i���K���z�j")]
    [SerializeField, Range(0.01f, 1f)] float jitterStdDev = 0.25f; // ���}�X���̗h��
    [SerializeField, Range(0.01f, 1f)] float ratioStdDev = 0.25f; // �c����̗h��

    

    [Header("�z�u�X�P�[�� / �h�A�]��")]
    [SerializeField, Min(0)] int doorEdgeMargin = 1; // �p����ǂꂾ��������

    [Header("Prefab")]
    [SerializeField] GameObject outerWallPrefab;
    [SerializeField] Transform outerWallsParent;


    int width, height;

    public void Generate()
    {
        // --- �@ �V�[�h������ ---
        int useSeed = GameData.Instance.DaySeed;
        Random.InitState(useSeed);

        // TODO: ���^�p�ł� GameData.Instance.Day / TotalEvil ��n��
        ComputeSize(1, 0f, out width, out height);

        // --- �A ���ʂ� Manager �ɕۑ� ---
        ForestGenManager.Instance.Width = width;
        ForestGenManager.Instance.Height = height;

        ClearChildren(outerWallsParent);
        BuildOuterWalls();
        ConfigurationDoor();

        Debug.Log($"[ForestOuterGen] size = {width} x {height}, seed={useSeed}");
    }

    // ===== �T�C�Y���� =====
    void ComputeSize(int day, float totalEvil, out int w, out int h)
    {
        float dayMul = 1f + (day - 1) * dayCoef;
        float evilMul = 1f + totalEvil * evilCoef;

        int baseTotal = Mathf.RoundToInt(baseTotalSize * dayMul * evilMul);

        // ���T�C�Y�F�����̂ݕۏ؁B��͐��K���z�̏o�ڂ������Ŗ����B
        float jitterFactor = SampleNormal(1f, jitterStdDev); // Clamp���Ȃ�
        int total = Mathf.RoundToInt(baseTotal * jitterFactor);
        if (total < minDimension * 2) total = minDimension * 2;

        // �c����F0..1�Ɏ��߂�
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

    // ===== Start/Goal�z�u =====
    void ConfigurationDoor()
    {
        Vector2Int startCell = PickStartOnBottom();
        Vector2Int goalCell = PickGoalOnTopDiagonal(startCell);

        Vector3 startPos = ToWorld(startCell.x, startCell.y);
        Vector3 goalPos = ToWorld(goalCell.x, goalCell.y);

        RemoveWallAt(startPos);
        RemoveWallAt(goalPos);

        // --- Manager �ɕۑ� ---
        if (ForestGenManager.Instance.StartDoor != null) ForestGenManager.Instance.StartDoor.position = startPos;
        if (ForestGenManager.Instance.GoalDoor != null) ForestGenManager.Instance.GoalDoor.position = goalPos;

        Debug.Log($"[ForestOuterGen] start={startCell}, goal={goalCell}");
    }

    // ����(y=0)�Œ�Ax�͊p������ă����_��
    Vector2Int PickStartOnBottom()
    {
        int minX = Mathf.Max(1, doorEdgeMargin);
        int maxXEx = Mathf.Max(minX + 1, width - doorEdgeMargin); // �r���I���
        int sx = Random.Range(minX, maxXEx);
        return new Vector2Int(sx, 0);
    }

    // ���(y=height-1)�́g���Α��n�[�t�h����x��I�ԁi�p�͗]���ŉ���j
    Vector2Int PickGoalOnTopDiagonal(Vector2Int start)
    {
        int topY = height - 1;
        int midX = width / 2;
        bool startOnLeft = start.x < midX;

        int minX, maxXEx;
        if (startOnLeft)
        {
            // �X�^�[�g�������� �� �S�[���͉E����
            minX = Mathf.Max(midX, doorEdgeMargin);
            maxXEx = Mathf.Max(minX + 1, width - doorEdgeMargin);
        }
        else
        {
            // �X�^�[�g���E���� �� �S�[���͍�����
            minX = Mathf.Max(doorEdgeMargin, 0);
            maxXEx = Mathf.Max(minX + 1, midX);
        }

        // �O�̂��߃K�[�h�i�ɒ[�ɏ��������̂Ƃ��j
        if (minX >= maxXEx)
        {
            minX = Mathf.Clamp(midX, doorEdgeMargin, width - doorEdgeMargin - 1);
            maxXEx = Mathf.Min(width - doorEdgeMargin, minX + 1);
        }

        int gx = Random.Range(minX, maxXEx);
        return new Vector2Int(gx, topY);
    }

    // ===== ���[�e�B���e�B =====
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
    
    Vector3 ToWorld(int x, int y) => new Vector3(x , y, 0f);

    void SpawnWall(int x, int y)
    {
        if (outerWallPrefab == null || outerWallsParent == null) return;
        var pos = ToWorld(x, y);
        var go = Instantiate(outerWallPrefab, pos, Quaternion.identity, outerWallsParent);
        go.name = $"Wall_{x}_{y}";
    }

    void RemoveWallAt(Vector3 pos)
    {
        if (outerWallsParent == null) return;
        for (int i = outerWallsParent.childCount - 1; i >= 0; i--)
        {
            var child = outerWallsParent.GetChild(i);
            if (Vector3.Distance(child.position, pos) < 0.1f)
            {
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
        }
    }
}
*/
}
