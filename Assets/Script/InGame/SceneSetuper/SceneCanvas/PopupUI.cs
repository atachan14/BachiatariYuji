using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private RectTransform background; // Image �� RectTransform
   
    private PopupSO talk;       // ����I�t�Z�b�g���t�B�[���h��
    private Transform target;
    private Camera mainCam;

    // �f�o�b�O�p
    public int lineCount;
    public float width;
    public float height;

    public void Init(Transform target, PopupSO talk)
    {
        this.target = target;
        this.talk = talk;
        tmp.text = talk.text;
        tmp.fontSize = talk.fontSize;

        // TMP �̐����T�C�Y���擾
        Vector2 preferredSize = tmp.GetPreferredValues(tmp.text);
        tmp.rectTransform.sizeDelta = preferredSize;

        //// �f�o�b�O�p
        //lineCount = tmp.textInfo.lineCount;
        //width = preferredSize.x;
        //height = preferredSize.y;

        // �w�i���e�L�X�g��肿�傢�傫�߂Ɂi�]�� 20px �Ƃ��j
        Vector2 padding = new Vector2(20, 20);
        background.sizeDelta = preferredSize + padding;

        mainCam = Camera.main;
        Destroy(gameObject, talk.lifeTime);
    }

    private void Update()
    {
        if (target != null && mainCam != null)
        {
            Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + Vector3.up * talk.yOffset);
            transform.position = screenPos;
        }
    }
}
