using UnityEngine;

public class ArcherSinglePunish : Punish
{
    [SerializeField] private Archer archer;
    [SerializeField] private float cooldown = 2f;
    private float timer;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnStay");
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = cooldown;
            archer.Shoot();
            Debug.Log("shoot");
        }
    }
}
