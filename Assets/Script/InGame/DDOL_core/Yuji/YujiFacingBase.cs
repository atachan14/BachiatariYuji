using UnityEngine;

public abstract class YujiFacingBase : MonoBehaviour
{
    [SerializeField] protected Sprite[] facingSprites;
    [SerializeField] protected Animator animator;

    public Vector3 FacingDir { get; protected set; } = Vector3.down;
    protected SpriteRenderer sr;


    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateFacing();
    }

    public abstract void UpdateFacing();

    protected void UpdateGraphics(int numDirections, float angle)
    {
        if (facingSprites == null || facingSprites.Length == 0) return;
        int index = Mathf.FloorToInt(angle / 360f * numDirections) % numDirections;

        if (sr != null) sr.sprite = facingSprites[index];
        if (animator != null) animator.SetInteger("FacingIndex", index);
    }
}
