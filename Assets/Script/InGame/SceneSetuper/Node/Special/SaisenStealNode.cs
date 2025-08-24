using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SaisenStealNode : BaseNode
{
    [SerializeField] private BaseNode nextNode; // �I����ɐi�ސ�
    private YujiParams yujiParams;

    // �e��̕����z�\����ێ�
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
            // --- 1. StealSize �I�� ---
            ChoiceData selectedSize = null;
            yield return StealSizeSelection(r => selectedSize = r);

            // --- 2. �D�_�z���� ---
            int stealAmount = CalcStealAmount(sizeLookup[selectedSize.text]);
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
            if (selected.text.Contains("��߂�"))
                break;
        }

        // --- 6. ���v���ʕ\�� ---
        yield return ShowTotalResult(results);

        DialogWindowManager.Instance.ExitDialogMode();

        GameData.Instance.DayTime = DayTime.Night;
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

    // --- ���U���g�\�� ---
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

        string resultText = $"{breakdown}�~���񂾁I";
        yield return DialogTextManager.Instance.PlayTextRoutine(resultText);
        yield return DialogTextManager.Instance.WaitNextPress();

        GameData.Instance.Cash += total;
        GameData.Instance.DayEvil += total;
        GameData.Instance.TotalEvil += total;
    }

    // --- ChoiceData���� ---
    private Dictionary<string, StealSizeSO> sizeLookup;

    private ChoiceData[] BuildStealChoices()
    {
        sizeLookup = new Dictionary<string, StealSizeSO>();
        var list = new List<ChoiceData>();
        foreach (var size in yujiParams.UnlockedStealSizes) // �� List<StealSizeData>
        {
            list.Add(new ChoiceData()
            {
                text = size.displayName,
                rowIndex = 0
            });

            // �t���������ɓo�^
            sizeLookup[size.displayName] = size;
        }
        return list.ToArray();
    }


    private ChoiceData[] BuildContinueChoices(int remain)
    {
        return new ChoiceData[]
        {
            new ChoiceData(){ text = $"������ (�c��{remain}��)", rowIndex = 0 },
            new ChoiceData(){ text = "��߂�", rowIndex = 0 }
        };
    }

    // --- �v�Z���W�b�N�i�_�~�[�j ---
    private int CalcStealAmount(StealSizeSO data)
    {
        int maxSteal = YujiParams.Instance.MaxSteal;
        float median = maxSteal * data.medianRate; // ��: 0.05f �� 500
        float sigma = median * YujiParams.Instance.SigmaRatio;               // �����t�߂Ɋ񂹂�

        int value;
        do
        {
            value = Mathf.RoundToInt(NextGaussian(median, sigma));
        }
        while (value < 1 || value > maxSteal); // �͈͊O�Ȃ��������

        return value;
    }

    private float NextGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; // (0,1]�̗���
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2); // ~N(0,1)
        return mean + stdDev * randStdNormal;
    }



    // --- �����_�������� ---
    private string GeneratePartialAmount(int amount)
    {
        float hideRate = 1f - (yujiParams.StealOpenPower / 100f); // 50%�Ȃ�50f -> 0.5 -> 50%�B��
        string amountStr = amount.ToString();
        char[] chars = new char[amountStr.Length];

        for (int i = 0; i < amountStr.Length; i++)
        {
            chars[i] = Random.value < hideRate ? '?' : amountStr[i];
        }

        return new string(chars);
    }

}
