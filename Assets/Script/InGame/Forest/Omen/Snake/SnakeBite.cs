using System.Collections;
using UnityEngine;

public class SnakeBite : MonoBehaviour
{
    [SerializeField] private PunishParams para;      // �_���[�W���
    [SerializeField] private float extendSpeed = 10f; // �L�k�X�s�[�h
    [SerializeField] private float extendLength = 1f; // �ő�L�є{���i����localScale.x�ɏ�Z�j
    [SerializeField] private string targetLayer = "Yuji"; // �����蔻�背�C���[

    public void Bite(Transform target)
    {
        if (target == null) return;

        StartCoroutine(BiteRoutine(target));
    }

    private IEnumerator BiteRoutine(Transform target)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 direction = (target.position - transform.position).normalized;
        float t = 0f;

        // �L�т����������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // �L�k�t�F�[�Y
        while (t < 1f)
        {
            t += Time.deltaTime * extendSpeed;

            // ���`��ԂŐL�т�
            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x, originalScale.x * extendLength, t),
                originalScale.y,
                originalScale.z
            );

            // Raycast�œ����蔻��
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector2.Distance(transform.position, target.position), LayerMask.GetMask(targetLayer));
            if (hit.collider != null)
            {
                YujiParams.Instance.TakeDamage((int)para.damage,Color.green);
                // ��x���������炻��ȏ㔻�肵�Ȃ�
                break;
            }

            yield return null;
        }

        // ���̃T�C�Y�ɖ߂�
        transform.localScale = originalScale;
    }
}
