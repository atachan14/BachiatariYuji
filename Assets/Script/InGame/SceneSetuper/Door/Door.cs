using UnityEngine;


// シーン名
public enum SceneName
{
    House1F,
    House2F,
    Town,
    Forest,
    ForestTopDown,
    Shrine
}

// Door名（シーン内でユニーク）
public enum DoorName
{
    Start,
    GoUp,
    GoDown,
    GoTown,
    GoHouse,
    GoForest,
    GoShrine
}
public class Door : MonoBehaviour
{

    [Header("Door設定")]
    public DoorName doorName;           // 自分のDoor名（ユニーク）
    public SceneName targetScene;       // 移動先シーン
    public DoorName targetDoorName;     // 偏移先でのSpawnPointとして使うDoor名

    [Header("SpawnPoint")]
    [SerializeField] private Transform spawnPos;        // Yujiがここに出現する
    [SerializeField] private Transform triggerPos; // 子のTriggerPoint

    [SerializeField] private BoxCollider2D triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<BoxCollider2D>();
        if (triggerCol != null && triggerPos != null)
        {
            // 親ColliderのオフセットをTriggerPointのローカル位置に設定
            triggerCol.offset = (Vector2)triggerPos.localPosition;

            // 親ColliderのサイズをTriggerPointのスケールに合わせる（必要なら）
            triggerCol.size = new Vector2(triggerCol.size.x * triggerPos.localScale.x,
                                   triggerCol.size.y * triggerPos.localScale.y);
        }
        else
        {
            Debug.Log("col ka pos ga nai!");
        }
    }
    public void SpawnYuji()
    {
        Yuji.Instance.transform.position = spawnPos.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            SceneChanger.Instance.TransitionTo(targetScene, targetDoorName);
    }
    private void OnDrawGizmos()
    {
        // SpawnPoint: 緑マーカー
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(spawnPos.position, 0.1f);

        // TriggerPoint: 赤半透明でCollider枠
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // 半透明赤
                                                    // TriggerPointに合わせて描画
        Gizmos.matrix = Matrix4x4.TRS(triggerPos.position, Quaternion.identity, triggerPos.lossyScale);
        Gizmos.DrawCube(Vector3.zero, triggerCol.size);
        Gizmos.matrix = Matrix4x4.identity;
    }


   

}