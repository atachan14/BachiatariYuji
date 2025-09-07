using UnityEngine;

public class PunishParams : MonoBehaviour
{

    public float moveSpeed = 0;
    public float doubleMoveSpeedEvil = 5000;

    public float cd = 3;
    public float doubleCdEvil = 2000;

    public float damage = 0;
    public float doubleDamageEvil = 3000;

    public float effectValue = 0f;
    public float doubleEffectValueEvill = 3000f;



    private void Start()
    {
        moveSpeed *= (doubleMoveSpeedEvil != 0) ? (1 + DayData.Instance.DayEvil / doubleMoveSpeedEvil) : 1f;
        cd*= (doubleCdEvil != 0)? (1 + DayData.Instance.DayEvil / doubleCdEvil) : 1f;
        damage *= (doubleDamageEvil != 0) ? (1 + DayData.Instance.DayEvil / doubleDamageEvil) : 1f;
        effectValue *= (doubleEffectValueEvill != 0) ? (1 + DayData.Instance.DayEvil / doubleEffectValueEvill) : 1f;

    }
}
