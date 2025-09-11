using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 20f;
    [SerializeField] private int damage = 5;
    [SerializeField] private Color damageColor = Color.red;

    private Vector3 direction;

    public void Init(Transform target)
    {
        direction = (target.position - transform.position).normalized;
        Destroy(gameObject, lifeTime); // àÍíËéûä‘Ç≈é©ìÆè¡ñ≈
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
            YujiParams.Instance.TakeDamage(damage, damageColor);
            Destroy(gameObject);
    }
}
