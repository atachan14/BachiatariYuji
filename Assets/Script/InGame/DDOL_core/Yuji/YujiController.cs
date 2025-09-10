using UnityEngine;

public class YujiController : SingletonMonoBehaviour<YujiController>
{
    [SerializeField] private UnitSpriteController spriteController;

    public Vector3 Dir { get; private set; }
    private bool canMove = true;

    private void Update()
    {
        if (!canMove) return;

        Vector2 input = InputReceiver.Instance.MoveAxis;
        if (input.sqrMagnitude > 0.01f)
        {
            // ‚±‚±‚Å³‹K‰»‚µ‚ÄDir‚àXV
            Vector3 moveDir = (Vector3)input.normalized;
            Dir = moveDir;
            spriteController.SetDirSprite(Dir);

            // ˆÚ“®
            float speed = YujiState.Instance.MoveSpeed / 100f;
            transform.position += speed * Time.deltaTime * moveDir;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
