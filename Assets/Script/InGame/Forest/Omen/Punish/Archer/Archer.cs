using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;

    private void OnEnable() => ArcherMultiPunish.OnMultiShot += Shoot;
    private void OnDisable() => ArcherMultiPunish.OnMultiShot -= Shoot;

    public void Shoot()
    {
        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity,transform);
        // ��ɁuYuji��ǔ����鏈���v�t���� or Rigidbody�ɗ͉�����Ȃ�
        arrow.GetComponent<Arrow>().Init(Yuji.Instance.transform);
    }
}