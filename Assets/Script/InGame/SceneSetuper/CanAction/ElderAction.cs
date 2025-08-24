using UnityEngine;

public class ElderAction : CanAction
{
    public override void ChooseNode()
    {
        switch (GameData.Instance.DayEvil)
        {
            case <100:
                currentNode = GetNode("99");
                break;

            case <1000:
                currentNode = GetNode("999");
                break;
        }
    }
}
