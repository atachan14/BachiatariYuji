using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaisenStealNode : BaseNode
{
    [SerializeField] private BaseNode nextNode;

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
            // --- 1. StealSize ëIë ---
            ChoiceData selectedSize = null;
            yield return StealSizeSelection(r => selectedSize = r);

            // --- 2. ìDñ_äzåàíË ---
            var selectedSO = yujiParams.UnlockedStealSizes.First(s => s.id == selectedSize.id);
            int stealAmount = CalcStealAmount(selectedSO);
            results.Add(stealAmount);

            // --- 3. ç°âÒÇÃïîï™äzï\é¶Çê∂ê¨ÇµÇƒï€éù ---
            string partial = GeneratePartialAmount(stealAmount);
            partialResults.Add(partial);

            // --- 4. ó›åvïîï™äzï\é¶ÅiëOâÒï™Ç‡ä‹ÇﬁÅj ---
            string cumulativeText = string.Join(" + ", partialResults);
            yield return DialogTextManager.Instance.PlayTextRoutine(cumulativeText);
            yield return DialogTextManager.Instance.WaitNextPress();

            streak++;
            if (streak >= yujiParams.MaxStealStreak)
                break;

            // --- 5. ë±ÇØÇÈ or Ç‚ÇﬂÇÈ ---
            ChoiceData selected = null;
            yield return ContinueOrStopSelection(yujiParams.MaxStealStreak - streak, r => selected = r);
            if (selected.id == -1) // stopLabel ÇÃ id Ç -1 Ç…ÇµÇƒÇ®Ç≠
                break;
        }

        // --- 6. çáåvåãâ ï\é¶ ---
        yield return ShowTotalResult(results);

        DialogWindowManager.Instance.ExitDialogMode();

        DayData.Instance.DayTime = DayTime.Night;
        nextNode?.PlayNode();
    }

    // --- ëIëéàê∂ê¨ & èàóù ---
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
                id = -1, // í‚é~óp
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
        DayData.Instance.DayEvil += total;
        GameData.Instance.TotalEvil += total;
    }

    private int CalcStealAmount(StealSizeSO data)
    {
        int maxSteal = YujiParams.Instance.MaxSteal;
        float median = maxSteal * data.medianRate;
        float sigma = median * YujiParams.Instance.SigmaRatio;

        int value;
        do
        {
            value = Mathf.RoundToInt(NextGaussian(median, sigma));
        } while (value < 1 || value > maxSteal);

        return value;
    }

    private float NextGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    private string GeneratePartialAmount(int amount)
    {
        float hideRate = 1f - (yujiParams.StealOpenPower / 100f);
        char[] chars = amount.ToString().Select(c => Random.value < hideRate ? '?' : c).ToArray();
        return new string(chars);
    }
}
