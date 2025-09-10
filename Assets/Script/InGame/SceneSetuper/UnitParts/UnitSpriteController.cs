using UnityEditor;
using UnityEngine;

public class UnitSpriteController : MonoBehaviour
{
    public SpriteRenderer sr;
    private Sprite currentSprite;

    [Header("4方向のSprite")]
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    // ウヨウヨの現在値
    private Vector3 posOffset;
    private Vector3 scaleOffset;

    private void Update()
    {
        UpdateHallucinationEffect();
    }
    public void SetSprite(Sprite s)
    {
        sr.sprite = s;
    }

    public void SetDirSprite(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            sr.sprite = (dir.x > 0 ? spriteRight : spriteLeft);
        else
            sr.sprite = (dir.y > 0 ? spriteUp : spriteDown);
    }

    private void UpdateHallucinationEffect()
    {
        float maxOffset = YujiState.Instance.Hallucinations / 100f;
        if (maxOffset <= 0f)
        {
            posOffset = Vector3.zero;
            scaleOffset = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            return;
        }

        float step = maxOffset / 100f;

        // posOffset をランダムに増減
        posOffset.x = Mathf.Clamp(posOffset.x + Random.Range(-1, 2) * step, -maxOffset, maxOffset);
        posOffset.y = Mathf.Clamp(posOffset.y + Random.Range(-1, 2) * step, -maxOffset, maxOffset);

        // scaleOffset をランダムに増減
        scaleOffset.x = Mathf.Clamp(scaleOffset.x + Random.Range(-1, 2) * step, -maxOffset, maxOffset);
        scaleOffset.y = Mathf.Clamp(scaleOffset.y + Random.Range(-1, 2) * step, -maxOffset, maxOffset);

        // 反映
        transform.localPosition = posOffset;
        transform.localScale = Vector3.one + scaleOffset;
    }
}
