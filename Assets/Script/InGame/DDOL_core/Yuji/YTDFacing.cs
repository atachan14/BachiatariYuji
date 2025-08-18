using UnityEngine;

public class YTDFacing : YujiFacingBase
{
    public override void UpdateFacing()
    {
        Vector2 input = InputReceiver.Instance.MoveAxis;

        if (input.sqrMagnitude > 0.01f)
        {
            // Vector3�ɕϊ�
            FacingDir = new Vector3(input.x, input.y, 0f).normalized;

            // �X�v���C�g�X�V�p�Ɋp�x�v�Z
            float angle = Mathf.Atan2(FacingDir.z, FacingDir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            UpdateGraphics(4, angle); // ����4�����\��
        }
    }

}
