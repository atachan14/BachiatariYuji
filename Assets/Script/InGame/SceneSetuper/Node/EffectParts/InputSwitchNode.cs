using UnityEngine;

public class InputSwitchNode : BaseNode
{
    [SerializeField] private InputMode mode;   // Inspector �őI��
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        InputReceiver.Instance.SwitchMode(mode);
        nextNode?.PlayNode();
    }
}