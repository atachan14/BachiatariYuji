using UnityEngine;

public class Punish : MonoBehaviour
{
    public PunishDestinySO destiny;
    [SerializeField] Transform parent;
    [SerializeField] CircleCollider2D col;
    [SerializeField] float doubleColRangeEvil = 5000;
    public float colRange = 2f;


    protected virtual void OnEnable()
    {
        colRange *= (doubleColRangeEvil != 0) ? (1 + DayData.Instance.DayEvil / doubleColRangeEvil) : 1f;
        col.radius = colRange / Mathf.Max(parent.localScale.x, parent.localScale.y);
    }
}
