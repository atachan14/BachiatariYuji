using UnityEngine;

public class YTDMove : SingletonMonoBehaviour<YTDMove>
{
 
    [SerializeField] Transform parentTransform;

    private void Update()
    {
        Vector2 dir = InputReceiver.Instance.MoveAxis; // éŒÇﬂï‚ê≥çœÇ›
        float speed = YujiState.Instance.MoveSpeed ;
        parentTransform.position += (Vector3)dir * speed * Time.deltaTime;
    }
}
