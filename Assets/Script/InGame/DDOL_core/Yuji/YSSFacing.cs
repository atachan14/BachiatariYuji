using UnityEngine;

public class YSSFacing : YujiFacingBase
{

    public override void UpdateFacing()
    {
        float h = InputReceiver.Instance.MoveAxisX;

        if (h > 0.01f) FacingDir = Vector3.right;
        else if (h < -0.01f) FacingDir = Vector3.left;

        // Sprite/アニメ更新
        float angle = (FacingDir.x > 0) ? 0f : 180f;
        UpdateGraphics(2, angle); // SideScrollは2方向
    }
}
