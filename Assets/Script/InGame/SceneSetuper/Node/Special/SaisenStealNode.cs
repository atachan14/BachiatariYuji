using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SaisenStealNode : BaseNode
{
    [SerializeField] private BaseNode nextNode; // 終了後に進む先
    private YujiParams yujiParams;

    // 各回の部分額表示を保持
    private List<string> partialResults = new List<string>();

    public override void PlayNode()
    {
        StopAllCoroutines();
        StartCoroutine(SaisenStealRoutine());
    }

    private IEnumerator SaisenStealRoutine()
    {
        yujiParams = YujiParams.Instance;
        int streak = 0;
        List<int> results = new List<int>();
        partialResults.Clear();

        DialogWindowManager.Instance.EnterDialogMode();

        while (streak < yujiParams.MaxStealStreak)
        {
            // --- 1. StealSize 選択 ---
            ChoiceData selectedSize = null;
            yield return StealSizeSelection(r => selectedSize = r);

            // --- 2. 泥棒額決定 ---
            int stealAmount = CalcStealAmount(sizeLookup[selectedSize.text]);
            results.Add(stealAmount);

            // --- 3. 今回の部分額表示を生成して保持 ---
            string partial = GeneratePartialAmount(stealAmount);
            partialResults.Add(partial);

            // --- 4. 累計部分額表示（前回分も含む） ---
            string cumulativeText = string.Join(" + ", partialResults);
            yield return DialogTextManager.Instance.PlayTextRoutine(cumulativeText);
            yield return DialogTextManager.Instance.WaitNextPress();

            streak++;
            if (streak >= yujiParams.MaxStealStreak)
                break;

            // --- 5. 続ける or やめる ---
            ChoiceData selected = null;
            yield return ContinueOrStopSelection(yujiParams.MaxStealStreak - streak, r => selected = r);
            if (selected.text.Contains("やめる"))
                break;
        }

        // --- 6. 合計結果表示 ---
        yield return ShowTotalResult(results);

        DialogWindowManager.Instance.ExitDialogMode();

        GameData.Instance.DayTime = DayTime.Night;
        nextNode?.PlayNode();
    }

    // --- 選択肢生成 & 処理 ---
    private IEnumerator StealSizeSelection(System.Action<ChoiceData> callback)
    {
        var stealChoices = BuildStealChoices();
        yield return ChoiceContainerManager.Instance.PlayChoiceRoutine(stealChoices, callback);
    }

    private IEnumerator ContinueOrStopSelection(int remain, System.Action<ChoiceData> callback)
    {
        var choices = BuildContinueChoices(remain);
        yield return ChoiceContainerManager.Instance.PlayChoiceRoutine(choices, callback);
    }

    // --- リザルト表示 ---
    private IEnumerator ShowTotalResult(List<int> results)
    {
        int total = 0;
        string breakdown = "";
        foreach (var v in results)
        {
            total += v;
            breakdown += v + " + ";
        }
        breakdown = breakdown.TrimEnd(' ', '+');

        string resultText = $"{breakdown}円盗んだ！";
        yield return DialogTextManager.Instance.PlayTextRoutine(resultText);
        yield return DialogTextManager.Instance.WaitNextPress();

        GameData.Instance.Cash += total;
        GameData.Instance.DayEvil += total;
        GameData.Instance.TotalEvil += total;
    }

    // --- ChoiceData生成 ---
    private Dictionary<string, StealSizeSO> sizeLookup;

    private ChoiceData[] BuildStealChoices()
    {
        sizeLookup = new Dictionary<string, StealSizeSO>();
        var list = new List<ChoiceData>();
        foreach (var size in yujiParams.UnlockedStealSizes) // ← List<StealSizeData>
        {
            list.Add(new ChoiceData()
            {
                text = size.displayName,
                rowIndex = 0
            });

            // 逆引き辞書に登録
            sizeLookup[size.displayName] = size;
        }
        return list.ToArray();
    }


    private ChoiceData[] BuildContinueChoices(int remain)
    {
        return new ChoiceData[]
        {
            new ChoiceData(){ text = $"続ける (残り{remain}回)", rowIndex = 0 },
            new ChoiceData(){ text = "やめる", rowIndex = 0 }
        };
    }

    // --- 計算ロジック（ダミー） ---
    private int CalcStealAmount(StealSizeSO data)
    {
        int maxSteal = YujiParams.Instance.MaxSteal;
        float median = maxSteal * data.medianRate; // 例: 0.05f → 500
        float sigma = median * YujiParams.Instance.SigmaRatio;               // 中央付近に寄せる

        int value;
        do
        {
            value = Mathf.RoundToInt(NextGaussian(median, sigma));
        }
        while (value < 1 || value > maxSteal); // 範囲外なら引き直し

        return value;
    }

    private float NextGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; // (0,1]の乱数
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2); // ~N(0,1)
        return mean + stdDev * randStdNormal;
    }



    // --- ランダム桁生成 ---
    private string GeneratePartialAmount(int amount)
    {
        float hideRate = 1f - (yujiParams.StealOpenPower / 100f); // 50%なら50f -> 0.5 -> 50%隠す
        string amountStr = amount.ToString();
        char[] chars = new char[amountStr.Length];

        for (int i = 0; i < amountStr.Length; i++)
        {
            chars[i] = Random.value < hideRate ? '?' : amountStr[i];
        }

        return new string(chars);
    }

}
