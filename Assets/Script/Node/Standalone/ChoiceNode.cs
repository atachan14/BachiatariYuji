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

        bool done = false;
        ChoiceData selected = null;

        // ChoiceContainerManagerにコールバック渡す
        yield return ChoiceContainerManager.Instance.PlayChoiceRoutine(choiceData, choice =>
        {
            selected = choice;
            done = true;
        });

        // 選択完了まで待機
        while (!done) yield return null;

        DialogWindowManager.Instance.ExitDialogMode();

        // 次のNodeへ
        if (selected != null && selected.nextNode != null)
        {
            selected.nextNode.PlayNode();
        }
    }
}
