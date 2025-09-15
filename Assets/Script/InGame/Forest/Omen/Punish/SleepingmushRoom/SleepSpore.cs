using System.Security.Cryptography;
using UnityEngine;

public class SleepSpore : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 4f;
    
    [SerializeField] AnimationCurve curve;
    [SerializeField] int effectValue = 10;
    [SerializeField] float duration;

    private Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        YujiState.Instance.TakePosion(new PoisonEffect(EffectType.Drowsiness, curve, effectValue, duration));
        Destroy(gameObject);
    }
}
