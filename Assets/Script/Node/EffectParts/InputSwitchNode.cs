using UnityEngine;

public class InputSwitchNode : BaseNode
{
    [SerializeField] private InputMode mode;   // Inspector ‚Å‘I‘ð

    public override void PlayNode()
    {
        InputReceiver.Instance.SwitchMode(mode);
        nextNode?.PlayNode();
    }
}