using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;

    private void OnEnable() => ArcherMultiPunish.OnMultiShot += Shoot;
    private void OnDisable() => ArcherMultiPunish.OnMultiShot -= Shoot;

    public void Shoot()
    {
        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity,transform);
        // –î‚ÉuYuji‚ğ’Ç”ö‚·‚éˆ—v•t‚¯‚é or Rigidbody‚É—Í‰Á‚¦‚é‚È‚Ç
        arrow.GetComponent<Arrow>().Init(Yuji.Instance.transform);
    }
}