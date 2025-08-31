using System.Collections;
using System.Linq;
using UnityEngine;

public class DogJumpOut : MonoBehaviour
{
    [SerializeField] Transform dogParent;

    public IEnumerator Exe(float jumpRange)
    {
        dogParent.gameObject.layer = LayerMask.NameToLayer(LayerName.SmallUnit.ToString());
        // ���݈ʒu��Yuji�̈ʒu
        Vector2Int dogPos = Vector2Int.RoundToInt(dogParent.position);
        Vector2 yujiPos = Yuji.Instance.transform.position;

        // Floor+Branch �̌��
        var manager = ForestManager.Instance; // ForestManager����Ȃ���ForestGenManager����ˁH
        var candidates = manager.FloorAndBranchCoords
            .Where(c => Vector2Int.Distance(dogPos, c) <= jumpRange);

        if (!candidates.Any())
        {
            Debug.LogWarning("[DogJumpOut] ��₪�Ȃ��̂Ŕ�яo�����s");
            yield break;
        }

        // Yuji�ɍł��߂����W��I��
        Vector2Int target = candidates
            .OrderBy(c => Vector2.Distance(yujiPos, c)) // Vector2Int��Vector2�ɈÖٕϊ������
            .First();

        // �r�������ƃW�����v
        Vector3 targetPos = new(target.x, target.y, dogParent.position.z);
        yield return StartCoroutine(JumpTo(targetPos));
        dogParent.gameObject.layer = LayerMask.NameToLayer(LayerName.MiddleUnit.ToString());
    }

    // �r�������ƃW�����v�ړ�
    private IEnumerator JumpTo(Vector3 targetPos, float duration = 0.5f, float height = 1.5f)
    {
        Vector3 startPos = dogParent.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // ���`���
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            // �p���{�����Z
            pos.y += Mathf.Sin(t * Mathf.PI) * height;

            dogParent.position = pos;
            yield return null;
        }

        dogParent.position = targetPos; // �Ō�s�^�b�ƒ��n
    }
}
