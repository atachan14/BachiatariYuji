using UnityEngine;

public class OmenManager : SingletonMonoBehaviour<OmenManager>
{
    public void Setup()
    {
        var omens = GetComponentsInChildren<Omen>();
        foreach (var om in omens)
        { 
        
        }
            
    }
}
