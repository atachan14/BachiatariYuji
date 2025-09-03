using UnityEngine;

public class DogParams : MonoBehaviour
{

    public float moveSpeed = 2;
    public float doubleMoveSpeedEvil = 5000;

    public float biteDamage = 10;
    public float doubleBiteDamageEvil = 3000;

    public float biteCd = 1;


    private void Start()
    {
        moveSpeed *= (1 + DayData.Instance.DayEvil / doubleMoveSpeedEvil);
        biteDamage *= (1 + DayData.Instance.DayEvil / doubleBiteDamageEvil);
    }
}
