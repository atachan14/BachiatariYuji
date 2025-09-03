using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthEffect : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetColor(int damage, Color color)
    {
        // 最大アルファ計算
        float maxA = Mathf.Clamp01((float)damage / YujiParams.Instance.MaxHelth);

        // 色設定（a=0からスタート）
        Color c = color;
        c.a = 0f;
        image.color = c;

        // コルーチンでフェード
        StartCoroutine(Fade(maxA));
    }

    private IEnumerator Fade(float maxA)
    {
        // いきなり maxA に
        Color c = image.color;
        c.a = maxA;
        image.color = c;

        // 0.1秒でフェードアウト
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
