using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private RectTransform background; // Image の RectTransform
   
    private PopupSO talk;       // 頭上オフセットをフィールド化
    private Transform target;
    private Camera mainCam;

    // デバッグ用
    public int lineCount;
    public float width;
    public float height;

    public void Init(Transform target, PopupSO talk)
    {
        this.target = target;
        this.talk = talk;
        tmp.text = talk.text;
        tmp.fontSize = talk.fontSize;

        // TMP の推奨サイズを取得
        Vector2 preferredSize = tmp.GetPreferredValues(tmp.text);
        tmp.rectTransform.sizeDelta = preferredSize;

        //// デバッグ用
        //lineCount = tmp.textInfo.lineCount;
        //width = preferredSize.x;
        //height = preferredSize.y;

        // 背景をテキストよりちょい大きめに（余白 20px とか）
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
