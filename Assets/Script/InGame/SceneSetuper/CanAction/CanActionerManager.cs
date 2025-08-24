using UnityEngine;

public class CanActionerManager : SingletonMonoBehaviour<CanActionerManager>
{
    public void RefreshAll()
    {
        var actions = GetComponentsInChildren<CanAction>();
        foreach (var action in actions)
        {
            action.ChooseNode();
        }
    }
}