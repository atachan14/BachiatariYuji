using UnityEngine;

public class YSSFacing : YujiFacingBase
{

    public override void UpdateFacing()
    {
        float h = InputReceiver.Instance.MoveAxisX;

        if (h > 0.01f) FacingDir = Vector3.right;
        else if (h < -0.01f) FacingDir = Vector3.left;

        // Sprite/�A�j���X�V
        float angle = (FacingDir.x > 0) ? 0f : 180f;
        UpdateGraphics(2, angle); // SideScroll��2����
    }
}
