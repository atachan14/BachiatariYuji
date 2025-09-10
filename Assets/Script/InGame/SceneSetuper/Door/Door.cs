using UnityEngine;


// �V�[����
public enum SceneName
{
    House1F,
    House2F,
    Town,
    Forest,
    ForestTopDown,
    Shrine
}

// Door���i�V�[�����Ń��j�[�N�j
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

    [Header("Door�ݒ�")]
    public DoorName doorName;           // ������Door���i���j�[�N�j
    public SceneName targetScene;       // �ړ���V�[��
    public DoorName targetDoorName;     // �Έڐ�ł�SpawnPoint�Ƃ��Ďg��Door��

    [Header("SpawnPoint")]
    [SerializeField] private Transform spawnPos;        // Yuji�������ɏo������
    [SerializeField] private Transform triggerPos; // �q��TriggerPoint

    [SerializeField] private BoxCollider2D triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<BoxCollider2D>();
        if (triggerCol != null && triggerPos != null)
        {
            // �eCollider�̃I�t�Z�b�g��TriggerPoint�̃��[�J���ʒu�ɐݒ�
            triggerCol.offset = (Vector2)triggerPos.localPosition;

            // �eCollider�̃T�C�Y��TriggerPoint�̃X�P�[���ɍ��킹��i�K�v�Ȃ�j
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
        // SpawnPoint: �΃}�[�J�[
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(spawnPos.position, 0.1f);

        // TriggerPoint: �Ԕ�������Collider�g
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // ��������
                                                    // TriggerPoint�ɍ��킹�ĕ`��
        Gizmos.matrix = Matrix4x4.TRS(triggerPos.position, Quaternion.identity, triggerPos.lossyScale);
        Gizmos.DrawCube(Vector3.zero, triggerCol.size);
        Gizmos.matrix = Matrix4x4.identity;
    }


   

}