using TMPro;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogWindow;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private GameObject nextCursor;

    private ChoiceNode currentNode;
    private ChoiceSO currentSo;


}
