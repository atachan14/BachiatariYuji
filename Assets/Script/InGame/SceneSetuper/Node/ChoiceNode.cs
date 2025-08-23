using System.Collections;
using UnityEngine;


public class ChoiceNode : BaseNode
{
    public TalkSO so;
    public ChoiceData[] choiceData; // choiceSO.choices �ƃC���f�b�N�X�Ή�

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

        // ChoiceContainerManager�ɃR�[���o�b�N�n��
        yield return ChoiceContainerManager.Instance.PlayChoiceRoutine(choiceData, choice =>
        {
            selected = choice;
            done = true;
        });

        // �I�������܂őҋ@
        while (!done) yield return null;

        DialogWindowManager.Instance.ExitDialogMode();

        // ����Node��
        if (selected != null && selected.nextNode != null)
        {
            selected.nextNode.PlayNode();
        }
    }
}
