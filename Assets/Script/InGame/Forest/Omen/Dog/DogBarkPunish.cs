using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarkPunish : Punish
{
    [SerializeField] DogJumpOut jumpOut;
    bool isBarked = false;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBarked) return;
        StartCoroutine(BarkFlow());
    }

    IEnumerator BarkFlow()
    {
        yield return StartCoroutine(jumpOut.Exe(colRange));
        yield return StartCoroutine(Bark());
        isBarked = true;
    }

    IEnumerator Bark()
    {
        yield return null;
        Debug.Log("ÇŸÇ¶ÇΩÅI");
    }


}
