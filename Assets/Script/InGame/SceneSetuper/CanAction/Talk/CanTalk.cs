using TMPro;
using UnityEngine;

public class CanTalk : CanAction
{
    public TalkData[] talks;

    public override void DoAction()
    {
        TalkManager.Instance.ShowTalk(talks[0], transform);
        Debug.Log("talk");
    }
}