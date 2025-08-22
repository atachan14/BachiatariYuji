using System.Collections;
using UnityEngine;
using TMPro;

public class TalkManager : SingletonMonoBehaviour<TalkManager>
{
    private TalkNode currentNode;
    public void ShowTalk(TalkNode node)
    {
        currentNode = node;
        StopAllCoroutines();
        StartCoroutine(DialogManager.Instance.PlayTextRoutine(new DialogTextData(currentNode)));
    }
}
