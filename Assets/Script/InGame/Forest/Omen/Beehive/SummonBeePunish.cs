using UnityEngine;

public class SummonBeePunish : Punish
{
    [SerializeField] GameObject bee;

    bool isSummoned = false;
    int num = 1;
    [SerializeField] int addOneRatioNum = 200;

    protected override void OnEnable()
    {
        base.OnEnable();
        num += DayData.Instance.DayEvil / addOneRatioNum;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSummoned)
        {
            for (int i = 0; i < num; i++)
            {
                Instantiate(bee, transform);
            }
            isSummoned = true;
        }
    }
}
