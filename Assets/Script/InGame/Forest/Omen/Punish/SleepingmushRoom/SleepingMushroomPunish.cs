using System.Collections;
using UnityEngine;

public class SleepMushroomPunish : Punish
{
    [SerializeField] private GameObject sporePrefab;

    // ���̔����܂ł̃N�[���_�E���i���Β���ɃJ�E���g�J�n�j
    [SerializeField] private float cooldown = 5f;

    [SerializeField] private int shotsPerRound = 36;  // 360�x����
    [SerializeField] private int totalShots = 72;     // ���v���ː��ishotsPerRound�̔{����2���Ƃ��j
    [SerializeField] private float shotInterval = 0.1f; // 1�����Ƃ̊Ԋu

    private float timer = 0f;

    private void Update()
    {
        // ��ɃN�[���_�E����i�߂�i���˒��ł��i�ށj
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        // ���˒��͓�d�N�����Ȃ��B�N�[���_�E���� Update �Ői��ł���̂ł����ł͊J�n����̂�
        if (timer <= 0f)
        {
            // ���Β���ɃN�[���_�E�����J�n�������̂Ő�ɃZ�b�g
            timer = cooldown;
            StartCoroutine(FireSporesRoutine());
        }
    }

    private IEnumerator FireSporesRoutine()
    {
        float currentShot = 0;
        while (currentShot < totalShots)
        {
            float angleStep = 360f / shotsPerRound;
            float angle = (currentShot % shotsPerRound) * angleStep;

            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
                                      Mathf.Sin(angle * Mathf.Deg2Rad),
                                      0f);

            var spore = Instantiate(sporePrefab, transform.position, Quaternion.identity);
            var sporeComp = spore.GetComponent<SleepSpore>();
            if (sporeComp != null)
            {
                sporeComp.Init(dir);
            }

            currentShot++;
            yield return new WaitForSeconds(shotInterval);
        }
    }
}
