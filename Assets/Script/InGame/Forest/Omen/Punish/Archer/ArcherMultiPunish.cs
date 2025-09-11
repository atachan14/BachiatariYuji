using System;
using UnityEngine;

public class ArcherMultiPunish : Punish
{
    public static event Action OnMultiShot;

    [SerializeField] private float cooldown = 3f;
    private float timer;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnStay");
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = cooldown;
            OnMultiShot?.Invoke();
            Debug.Log("shoot");
        }
    }
}
