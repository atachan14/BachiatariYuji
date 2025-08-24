using UnityEngine;

public class OfferingBoxAction : CanAction
{
    public override void ChooseNode()
    {
        switch (GameData.Instance.DayTime)
        {
            case DayTime.Morning:
                currentNode = GetNode("Morning");
                break;

            case DayTime.Night:
                currentNode = GetNode("Night");
                break;
        }

    }
}
