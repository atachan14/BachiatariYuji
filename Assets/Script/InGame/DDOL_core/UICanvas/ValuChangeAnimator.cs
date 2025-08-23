using System.Collections;
using TMPro;
using UnityEngine;

public class WindowValueChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;
    private Coroutine currentCoroutine;

    public void ChangeValue(int from, int to)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ChangeValueCoroutine(from, to));
    }

    private IEnumerator ChangeValueCoroutine(int from, int to)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int current = Mathf.RoundToInt(Mathf.Lerp(from, to, elapsed / duration));
            valueText.text = current.ToString();
            yield return null;
        }

        valueText.text = to.ToString();
        currentCoroutine = null;
    }

    // 初期値だけセットしたいとき用
    public void SetValue(int val)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        valueText.text = val.ToString();
        currentCoroutine = null;
    }
}