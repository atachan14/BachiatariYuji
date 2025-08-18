
using UnityEngine;

public class TalkManager : DDOL_child<TalkManager>
{
    public void ShowTalk(TalkData talk, Transform target)
    {
        switch (talk.uiType)
        {
            case TalkUIType.Popup:
                PopupManager.Instance.ShowPopup(target, talk);
                break;

            case TalkUIType.Under:
                //UnderUIManager.Instance.ShowUnder(talk);
                break;
        }
    }
}
