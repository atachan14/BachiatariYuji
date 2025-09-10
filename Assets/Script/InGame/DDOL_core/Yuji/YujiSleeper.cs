using UnityEngine;

public class YujiSleeper : SingletonMonoBehaviour<YujiSleeper>
{
    [SerializeField] UnitSpriteController yujiSpriteController;
    [SerializeField] Sprite sleepingSprite;
    [SerializeField] Sprite wakingSprite; 
    [SerializeField] private float sleepThreshold = 100f;
    [SerializeField] private float sleepDecayPerSecond = 5f;
    [SerializeField] private float wakeDamageFactor = 20f; // 1ダメージで何ポイント減るか
    
    public float SleepValue { get; private set; }
    public bool IsSleeping { get; private set; }

    // 強制Sleepフラグ（演出用）
    private bool forcedSleep = false;

    private void Update()
    {
        if (forcedSleep) return; // 強制Sleep中はSleepValue無視

        if (SleepValue > 0)
        {
            SleepValue -= sleepDecayPerSecond * Time.deltaTime;
            SleepValue = Mathf.Max(SleepValue, 0);
        }

        CheckSleepState();
    }

    public void AddSleep(float value)
    {
        if (forcedSleep) return;

        SleepValue += value;
        CheckSleepState();
    }

    public void TakeDamage(float damage)
    {
        if (forcedSleep) return;

        SleepValue -= damage * wakeDamageFactor;
        SleepValue = Mathf.Max(SleepValue, 0);
        CheckSleepState();
    }

    private void CheckSleepState()
    {
        if (SleepValue >= sleepThreshold && !IsSleeping)
        {
            EnterSleep();
        }
        else if (SleepValue <= 0 && IsSleeping)
        {
            ExitSleep();
        }
    }

    private void EnterSleep()
    {
        IsSleeping = true;
        YujiController.Instance.gameObject.SetActive(false);
        yujiSpriteController.SetSprite(sleepingSprite);
        Debug.Log("Yuji fell asleep...");
    }

    private void ExitSleep()
    {
        IsSleeping = false;
        YujiController.Instance.gameObject.SetActive(true);
        yujiSpriteController.SetSprite(wakingSprite);
        Debug.Log("Yuji woke up!");
    }

    // --- 演出用 ---
    public void ForceSleep(bool enable)
    {
        forcedSleep = enable;
        if (enable)
            EnterSleep();
        else
            ExitSleep();
    }
}
