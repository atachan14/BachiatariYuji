using UnityEngine;

public class YujiSleeper : SingletonMonoBehaviour<YujiSleeper>
{
    [SerializeField] UnitSpriteController yujiSpriteController;
    [SerializeField] Sprite sleepingSprite;
    [SerializeField] Sprite wakingSprite;
    [SerializeField] private float drowsinessThreshold = 100f;
    [SerializeField] private float drowsinessDecayPerSecond = 5f;

    public bool IsSleeping { get; private set; }
    private bool forcedSleep = false;

    // 内部カウンタ（累積経過時間）
    private float sleepElapsed;

    public void Apply()
    {
        if (forcedSleep) return;

        if (IsSleeping)
        {
            DecayWhileSleeping();
        }

        CheckSleepState();
    }

    private void DecayWhileSleeping()
    {
        sleepElapsed += Time.deltaTime;
        float decay = drowsinessDecayPerSecond * Time.deltaTime;
        YujiState.Instance.DayDrowsiness -= decay;
    }

    private void CheckSleepState()
    {
        if (YujiState.Instance.Drowsiness >= drowsinessThreshold && !IsSleeping)
        {
            EnterSleep();
        }
        else if (YujiState.Instance.Drowsiness <= 0 && IsSleeping)
        {
            ExitSleep();
        }
    }

    private void EnterSleep()
    {
        IsSleeping = true;
        sleepElapsed = 0f; // リセット
        YujiController.Instance.enabled = false;
        yujiSpriteController.SetSprite(sleepingSprite);
        Debug.Log("Yuji fell asleep...");
    }

    private void ExitSleep()
    {
        IsSleeping = false;
        YujiController.Instance.enabled = true;
        yujiSpriteController.SetSprite(wakingSprite);
        Debug.Log("Yuji woke up!");
    }

    public void ForceSleep(bool enable)
    {
        forcedSleep = enable;
        if (enable) EnterSleep();
        else ExitSleep();
    }
}
