using UnityEngine;

public class ChoiceNode : BaseNode
{
    public ChoiceSO so;
    public BaseNode[] nextNodes; // choiceSO.choices とインデックス対応

    public override void PlayNode()
    {
        //ChoiceManager.Instance.ShowChoices(choiceSO.choices, OnChoiceSelected);
    }

    private void OnChoiceSelected(int index)
    {
        if (index >= 0 && index < nextNodes.Length)
        {
            nextNodes[index].PlayNode();
        }
    }
}
