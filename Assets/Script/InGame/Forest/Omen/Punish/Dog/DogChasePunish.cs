using System.Collections;
using UnityEngine;

public class DogChasePunish : Punish
{
    [SerializeField] DogJumpOut jumpOut;
    [SerializeField] DogChase chase;
    bool isSearched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSearched) return;
        StartCoroutine(PunishFlow());
    }

    IEnumerator PunishFlow()
    {

        yield return StartCoroutine(jumpOut.Exe(colRange));
        isSearched = true;
    }

    private void Update()
    {
        if (isSearched)
        {
            chase.Exe();
        }
    }


}
