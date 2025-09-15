using System.Collections;
using UnityEngine;

public class SnakeBite : MonoBehaviour
{
    [SerializeField] private PunishParams para;       // �_���[�W���
    [SerializeField] private float duration = 0.5f;   // �L�k�ɂ����鎞�ԁi�b�j

    public void Bite(Transform target)
    {
        if (target == null) return;
        StartCoroutine(BiteRoutine(target));
    }

    private IEnumerator BiteRoutine(Transform target)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.localPosition;
        Quaternion originalRotation = transform.rotation;

        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float distance = Vector2.Distance(transform.position, target.position);
        float targetLength = distance * 1.1f; // 10%���܂�

        // --- �L�΂��t�F�[�Y ---
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float currentLength = Mathf.Lerp(originalScale.x, targetLength, t);
            transform.localScale = new Vector3(currentLength, originalScale.y, originalScale.z);
            transform.localPosition = originalPosition + direction * (currentLength - originalScale.x) * 0.5f;

            yield return null;
        }

        // --- �߂��t�F�[�Y ---
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float currentLength = Mathf.Lerp(targetLength, originalScale.x, t);
            transform.localScale = new Vector3(currentLength, originalScale.y, originalScale.z);
            transform.localPosition = originalPosition + direction * (currentLength - originalScale.x) * 0.5f;

            yield return null;
        }

        // �ŏI�I�Ɋ��S�Ɍ��̏�Ԃɖ߂�
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        transform.rotation = originalRotation;
    }
}
