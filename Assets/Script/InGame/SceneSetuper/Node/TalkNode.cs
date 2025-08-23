
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNode : BaseNode
{
    public TalkSO so;
    public BaseNode nextNode;
    public override void PlayNode()
    {
       
        StopAllCoroutines();
        StartCoroutine(PlayNodeRoutine());
        
    }

    IEnumerator PlayNodeRoutine()
    {
        Debug.Log(so);
        DialogWindowManager.Instance.EnterDialogMode();
        yield return StartCoroutine(DialogTextManager.Instance.PlayTextRoutine(so));
        yield return StartCoroutine(DialogTextManager.Instance.WaitNextPress());
        DialogWindowManager.Instance.ExitDialogMode();
        nextNode?.PlayNode();
    }
}
