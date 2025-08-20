using UnityEngine;

public class UnderEvent : EventBase
{
    public WindowTalkData underData;
    public override void DoEvent()
    {
        WindowTalkManager.Instance.ShowUnder(underData);
    }
}
