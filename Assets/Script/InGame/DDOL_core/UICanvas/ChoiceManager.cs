using TMPro;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    private ChoiceNode currentNode;

    public void ShowChoice(ChoiceNode node)
    {
        currentNode = node;
        StopAllCoroutines();
        StartCoroutine(DialogManager.Instance.PlayTextRoutine(new DialogTextData(currentNode)));


    }
}
