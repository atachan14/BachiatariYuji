using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthEffect : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetColor(int damage, Color color)
    {
        // �ő�A���t�@�v�Z
        float maxA = Mathf.Clamp01((float)damage / YujiParams.Instance.MaxHelth);

        // �F�ݒ�ia=0����X�^�[�g�j
        Color c = color;
        c.a = 0f;
        image.color = c;

        // �R���[�`���Ńt�F�[�h
        StartCoroutine(Fade(maxA));
    }

    private IEnumerator Fade(float maxA)
    {
        // �����Ȃ� maxA ��
        Color c = image.color;
        c.a = maxA;
        image.color = c;

        // 0.1�b�Ńt�F�[�h�A�E�g
        float duration = 0.3f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(maxA, 0f, t / duration);
            image.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}
