using UnityEngine;

public class YTDFacing : YujiFacingBase
{
    public override void UpdateFacing()
    {
        Vector2 input = InputReceiver.Instance.MoveAxis;

        if (input.sqrMagnitude > 0.01f)
        {
            // Vector3に変換
            FacingDir = new Vector3(input.x, input.y, 0f).normalized;

            // スプライト更新用に角度計算
            float angle = Mathf.Atan2(FacingDir.z, FacingDir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            UpdateGraphics(4, angle); // 今は4方向表示
        }
    }

}
