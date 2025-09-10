using UnityEngine;

public class YujiMove : SingletonMonoBehaviour<YujiMove>
{


    private void Update()
    {
        Vector2 dir = InputReceiver.Instance.MoveAxis; // �΂ߕ␳�ς�
        float speed = YujiState.Instance.MoveSpeed / 100;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }
}
