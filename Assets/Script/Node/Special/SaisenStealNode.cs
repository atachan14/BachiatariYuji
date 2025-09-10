using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaisenStealNode : BaseNode
{

    [Header("Localized Texts")]
    [SerializeField] private List<LocalizedText> continueLabel;
    [SerializeField] private List<LocalizedText> stopLabel;
    [SerializeField] private List<LocalizedText> resultFormat;

    private YujiParams yujiParams;
    private List<string> partialResults = new();

    public override void PlayNode()
    {
        StopAllCoroutines();
        StartCoroutine(SaisenStealRoutine());
    }

    private IEnumerator SaisenStealRoutine()
    {
        yujiParams = YujiParams.Instance;
        int streak = 0;
        List<int> results = new();
        partialResults.Clear();

        DialogWindowManager.Instance.EnterDialogMode();

        while (streak < yujiParams.MaxStealStreak)
        {
            // --- 1. StealSize �I�� ---
            ChoiceData selectedSize = null;
            yield return StealSizeSelection(r => selectedSize = r);

            // --- 2. �D�_�z���� ---
            var selectedSO = yujiParams.UnlockedStealSizes.First(s => s.id == selectedSize.id);
            int stealAmount = CalcStealAmount(selectedSO);
            results.Add(stealAmount);

            // --- 3. ����̕����z�\���𐶐����ĕێ� ---
            string partial = GeneratePartialAmount(stealAmount);
            partialResults.Add(partial);

            // --- 4. �݌v�����z�\���i�O�񕪂��܂ށj ---
            string cumulativeText = string.Join(" + ", partialResults);
            yield return DialogTextManager.Instance.PlayTextRoutine(cumulativeText);
            yield return DialogTextManager.Instance.WaitNextPress();

            streak++;
            if (streak >= yujiParams.MaxStealStreak)
                break;

            // --- 5. ������ or ��߂� ---
            ChoiceData selected = null;
            yield return ContinueOrStopSelection(yujiParams.MaxStealStreak - streak, r => selected = r);
            if (selected.id == -1) // stopLabel �� id �� -1 �ɂ��Ă���
                break;
        }

        // --- 6. ���v���ʕ\�� ---
        yield return ShowTotalResult(results);

        DialogWindowManager.Instance.ExitDialogMode();
        nextNode?.PlayNode();
    }

    // --- �I�������� & ���� ---
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

    private ChoiceData[] BuildStealChoices()
    {
        return yujiParams.UnlockedStealSizes
            .Select(s => new ChoiceData
            {
                localizedTexts = s.localizedTexts,
                id = s.id,
                rowIndex = 0
            }).ToArray();
    }

    private ChoiceData[] BuildContinueChoices(int remain)
    {
        return new ChoiceData[]
        {
            new()
            {
                localizedTexts = new List<LocalizedText>
                {
                    new(Language.JP, string.Format(continueLabel.GetText(Language.JP), remain)),
                    new(Language.EN, string.Format(continueLabel.GetText(Language.EN), remain))
                },
                id = 0,
                rowIndex = 0
            },
            new()
            {
                localizedTexts = stopLabel,
                id = -1, // ��~�p
                rowIndex = 0
            }
        };
    }

    private IEnumerator ShowTotalResult(List<int> results)
    {
        int total = 0;
        string breakdown = string.Join(" + ", results);
        total = results.Sum();

        string format = resultFormat.GetText(OptionData.Instance.Language);
        string resultText = string.Format(format, breakdown);

        yield return DialogTextManager.Instance.PlayTextRoutine(resultText);
        yield return DialogTextManager.Instance.WaitNextPress();

        DayData.Instance.Cash += total;
        DayData.Instance.AddDayEvil(total);
    }

    private int CalcStealAmount(StealSizeSO data)
    {
        int maxSteal = YujiParams.Instance.MaxSteal;
        float pick = Random.value; // 0~1
        float accum = 0f;
        const int resolution = 1000; // �T���v�����O�𑜓x

        // �m�����z�Ƃ��Đ��K�����Ďg��
        float[] weights = new float[resolution];
        float totalWeight = 0f;
        for (int i = 0; i < resolution; i++)
        {
            float x = i / (float)(resolution - 1); // 0~1
            float w = data.distributionCurve.Evaluate(x);
            weights[i] = w;
            totalWeight += w;
        }

        float threshold = pick * totalWeight;
        for (int i = 0; i < resolution; i++)
        {
            accum += weights[i];
            if (accum >= threshold)
            {
                float ratio = i / (float)(resolution - 1);
                return Mathf.Clamp(Mathf.RoundToInt(maxSteal * ratio), 1, maxSteal);
            }
        }

        return maxSteal;
    }




    private string GeneratePartialAmount(int amount)
    {
        float hideRate = 1f - (yujiParams.StealOpenPower / 100f);
        char[] chars = amount.ToString().Select(c => Random.value < hideRate ? '?' : c).ToArray();
        return new string(chars);
    }
}
