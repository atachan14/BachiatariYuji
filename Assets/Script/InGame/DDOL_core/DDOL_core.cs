using UnityEngine;

public class DDOL_core : MonoBehaviour
{
    public static DDOL_core Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // q‚àˆê‚É¶‚«c‚é
    }
}