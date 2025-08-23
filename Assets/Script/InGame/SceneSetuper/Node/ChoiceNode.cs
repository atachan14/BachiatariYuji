using System.Collections;
using UnityEngine;


public class ChoiceNode : BaseNode
{
    public TalkSO so;
    public ChoiceData[] choiceData; // choiceSO.choices とインデックス対応

    public override void PlayNode()
    {
        StopAllCoroutines();
        StartCoroutine(PlayNodeCoroutine());
    }

 
    private IEnumerator PlayNodeCoroutine()
    {
        DialogWindowManager.Instance.EnterDialogMode();
        yield return DialogTextManager.Instance.PlayTextRoutine(so);
        yield return ChoiceContainerManager.Instance.PlayChoiceRoutine(choiceData);
        DialogWindowManager.Instance.ExitDialogMode();
    }
}
