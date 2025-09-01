using System.Collections;
using UnityEngine;

public class DogJumpBite : MonoBehaviour
{
    [SerializeField] DogChase chase;
    [SerializeField] BoxCollider2D col;
    [SerializeField] float biteDuration = 1f;       // ���݂�����
    [SerializeField] float damageInterval = 0.25f;   // �_���[�W�Ԋu
    [SerializeField] int damagePerTick = 10;         // �_���[�W��

    public void Exe()
    {
        StartCoroutine(BiteFlow());
    }

    IEnumerator BiteFlow()
    {
        chase.isBiting = true;
        gameObject.layer = LayerMask.NameToLayer(LayerName.SmallUnit.ToString());

        // Yuji�̌��݈ʒu�����b�N
        Vector2 lockTarget = Yuji.Instance.transform.position;

        bool hit = false;
        Vector2 bitePos = Vector2.zero;

        // �^�[�Q�b�g�n�_�֔�т�����
        while (Vector2.Distance(transform.position, lockTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                lockTarget,
                chase.moveSpeed * 2f * Time.deltaTime
            );

            // Yuji�ɐG�ꂽ�u�ԂɃq�b�g����
            if (Vector2.Distance(transform.position, Yuji.Instance.transform.position) < 0.4f)
            {
                hit = true;
                bitePos = transform.position; // ���̏u�Ԃ̍��W��ۑ�
                break;
            }

            yield return null;
        }

        if (hit)
        {
            // Yuji �ɑ΂���I�t�Z�b�g��ۑ�
            Transform yujiTf = Yuji.Instance.transform;
            Vector3 offset = transform.position - yujiTf.position;

            float timer = biteDuration;
            float damageTimer = 0f;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                damageTimer -= Time.deltaTime;

                // Yuji �̌��݈ʒu + �q�b�g���̃I�t�Z�b�g �ɒǏ]
                transform.position = yujiTf.position + offset;

                // ���Ԋu�Ń_���[�W����
                if (damageTimer <= 0f)
                {
                    YujiParams.Instance.TakeDamage(damagePerTick);
                    damageTimer = damageInterval;
                }

                yield return null;
            }
        }
        gameObject.layer = LayerMask.NameToLayer(LayerName.MiddleUnit.ToString());

        // �O�ꂽ�ꍇ�͉��������I��
        chase.isBiting = false;
    }
}
