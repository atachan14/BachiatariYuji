using UnityEngine;


public abstract class DDOL_child<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // V‚µ‚­ì‚ç‚ê‚½•û‚ğ”jŠü
            return;
        }
        Instance = (T)(object)this;
    }

}