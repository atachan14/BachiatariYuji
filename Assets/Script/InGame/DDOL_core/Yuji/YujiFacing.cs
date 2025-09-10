using UnityEngine;

public class YujiFacing : SingletonMonoBehaviour<YujiFacing>
{
    [SerializeField] UnitSpriteController unitSpriteController;
    public Vector3 Dir { get; private set; }
    private void Update()
    {
        UpdateFacing();
    }
    public void UpdateFacing()
    {
        Vector2 input = InputReceiver.Instance.MoveAxis;

        if (input.sqrMagnitude > 0.01f)
        {
            // Vector3‚É•ÏŠ·
            Vector3 temp = new Vector3(input.x, input.y, 0f).normalized;
            if(Dir != temp)
            {
                Dir = temp;
                unitSpriteController.SetDirSprite(Dir);
            }
        }
    }
}
