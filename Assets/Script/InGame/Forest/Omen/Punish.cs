using UnityEngine;

public class Punish : MonoBehaviour
{
    public PunishDestinySO destiny;
    [SerializeField] Transform parent;
    [SerializeField] CircleCollider2D col;
    [SerializeField] float colEvilRatio = 5000;
    public float colRange = 2f;


    protected virtual void OnEnable()
    {
        colRange += DayData.Instance.DayEvil / colEvilRatio;
        col.radius = colRange / Mathf.Max(parent.localScale.x, parent.localScale.y);
    }
}
