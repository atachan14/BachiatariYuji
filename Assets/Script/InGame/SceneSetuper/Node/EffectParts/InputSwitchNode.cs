using UnityEngine;

public class InputSwitchNode : BaseNode
{
    [SerializeField] private InputMode mode;   // Inspector ‚Å‘I‘ð
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        InputReceiver.Instance.SwitchMode(mode);
        nextNode?.PlayNode();
    }
}