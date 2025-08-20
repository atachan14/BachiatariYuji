using UnityEngine;

public class CanAction : MonoBehaviour
{
    [SerializeField] EventBase[] events;
    public virtual void DoAction()
    {
        events[0].DoEvent();
    }
}