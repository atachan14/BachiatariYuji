using System.Collections;
using UnityEngine;

public class SleepMushroomPunish : Punish
{
    [SerializeField] private GameObject sporePrefab;

    // 次の発動までのクールダウン（発火直後にカウント開始）
    [SerializeField] private float cooldown = 5f;

    [SerializeField] private int shotsPerRound = 36;  // 360度分割
    [SerializeField] private int totalShots = 72;     // 合計発射数（shotsPerRoundの倍数で2周とか）
    [SerializeField] private float shotInterval = 0.1f; // 1発ごとの間隔

    private float timer = 0f;

    private void Update()
    {
        // 常にクールダウンを進める（発射中でも進む）
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        // 発射中は二重起動しない。クールダウンは Update で進んでいるのでここでは開始判定のみ
        if (timer <= 0f)
        {
            // 発火直後にクールダウンを開始したいので先にセット
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
