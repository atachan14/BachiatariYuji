// === NoneOmen.cs ===
using UnityEngine;

public class NoneOmen : Omen
{
    public override void Spawn(GameObject prefabRoot)
    {
        Debug.Log($"[Omen] None: {prefabRoot.name}");
        // ‰½‚à‚µ‚È‚¢
    }
}
