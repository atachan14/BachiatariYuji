using UnityEngine;

public class NextDayNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

    public override void PlayNode()
    {
        GameData.Instance.Day += 1;
        GameData.Instance.DayTime = DayTime.Morning;

        // --- Seed���� ---
        GameData.Instance.DaySeed = System.Environment.TickCount; // �܂��͗����Ő���
        Debug.Log($"[NextDayNode] New DaySeed = {GameData.Instance.DaySeed}");

        nextNode?.PlayNode();
    }
}
