using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ChoiceData
{
    public string text;
    public List<LocalizedText> localizedTexts;
    public int rowIndex;
    public float fontSize = 50f;
    public TMP_FontAsset fontAsset;
    public BaseNode nextNode;
}

public class ChoiceContainerManager : SingletonMonoBehaviour<ChoiceContainerManager>
{
    [Header("UI Prefabs & Container")]
    [SerializeField] private Transform choiceContainer; // ChoiceContainer本体
    [SerializeField] private GameObject choiceRowPrefab; // ChoiceRow Prefab
    [SerializeField] private GameObject choiceOptionPrefab; // ChoiceOption Prefab

    [Header("Default Font")]
    [SerializeField] private TMP_FontAsset defaultFontAsset;



    private List<GameObject> choiceRows = new List<GameObject>();
    private ChoiceData _selectedChoice; // フィールドで保持
    private int currentRow = 0;
    private int currentCol = 0;

    // メイン処理
    public IEnumerator PlayChoiceRoutine(ChoiceData[] datas, Action<ChoiceData> onDecided)
    {
        SpawnChoices(datas);

        //レイアウト調整用
        LayoutRebuilder.ForceRebuildLayoutImmediate(choiceContainer.GetComponent<RectTransform>());
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(choiceContainer.GetComponent<RectTransform>());
        yield return new WaitForSeconds(0.5f);
        

        ResetCursor();

        ChoiceData selected = null;
        yield return StartCoroutine(HandleInput(datas, result => selected = result));

        ClearChoices();

        if (selected != null)
            onDecided?.Invoke(selected);
    }

    #region === UI生成 ===
    private void SpawnChoices(ChoiceData[] datas)
    {
        ClearChoices();

        int lastRowIndex = -1;
        GameObject currentRowObj = null;

        for (int i = 0; i < datas.Length; i++)
        {
            var data = datas[i];

            // 新しい行を作る
            if (data.rowIndex != lastRowIndex)
            {
                currentRowObj = SpawnRow();
                lastRowIndex = data.rowIndex;
            }

            // ChoiceOptionを行に追加
            SpawnOption(currentRowObj.transform, data);
        }
    }

    private GameObject SpawnRow()
    {
        GameObject rowObj = Instantiate(choiceRowPrefab, choiceContainer);
        choiceRows.Add(rowObj);
        return rowObj;
    }

    private void SpawnOption(Transform rowTransform, ChoiceData data)
    {
        GameObject optionObj = Instantiate(choiceOptionPrefab, rowTransform);
        TMP_Text label = optionObj.GetComponentInChildren<TMP_Text>();
        if (label != null)
        {
            label.text = data.localizedTexts.GetText(OptionData.Instance.Language);
            label.fontSize = data.fontSize;
            label.font = data.fontAsset ?? defaultFontAsset;
        }

        // カーソルは最初の子の Image にしておく
        Image cursor = optionObj.transform.GetChild(0).GetComponent<Image>();
        if (cursor != null) cursor.enabled = false;
    }

    private void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
        choiceRows.Clear();
    }
    #endregion

    #region === 入力処理 ===
    private IEnumerator HandleInput(ChoiceData[] datas, Action<ChoiceData> onDecided)
    {
        bool decided = false;

        while (!decided)
        {
            HandleCursorMovement();

            if (InputReceiver.Instance.Confirm)
            {
                decided = true;
                var choice = GetCurrentChoice(datas);
                onDecided?.Invoke(choice);
            }

            yield return null;
        }
    }

    private void HandleCursorMovement()
    {
        if (InputReceiver.Instance.Up) MoveCursor(-1, 0);
        if (InputReceiver.Instance.Down) MoveCursor(1, 0);
        if (InputReceiver.Instance.Left) MoveCursor(0, -1);
        if (InputReceiver.Instance.Right) MoveCursor(0, 1);
    }

    private void MoveCursor(int rowDelta, int colDelta)
    {
        currentRow = Mathf.Clamp(currentRow + rowDelta, 0, choiceRows.Count - 1);

        var row = choiceRows[currentRow];
        int colCount = row.transform.childCount;
        currentCol = Mathf.Clamp(currentCol + colDelta, 0, colCount - 1);

        UpdateCursorVisual();
    }
    #endregion

    #region === カーソル制御 ===
    private void ResetCursor()
    {
        currentRow = 0;
        currentCol = 0;
        UpdateCursorVisual();
    }

    private void UpdateCursorVisual()
    {
        // 全カーソルOFF
        foreach (var row in choiceRows)
        {
            foreach (Transform option in row.transform)
            {
                Image cursor = option.transform.GetChild(0).GetComponent<Image>();
                if (cursor != null) cursor.enabled = false;
            }
        }

        // 選択中カーソルON
        Image activeCursor = choiceRows[currentRow].transform.GetChild(currentCol).GetChild(0).GetComponent<Image>();
        if (activeCursor != null) activeCursor.enabled = true;
    }
    #endregion

    #region === データ取得 ===
    private ChoiceData GetCurrentChoice(ChoiceData[] datas)
    {
        int index = 0;
        for (int r = 0; r < currentRow; r++)
            index += choiceRows[r].transform.childCount;
        index += currentCol;

        if (index >= 0 && index < datas.Length)
            return datas[index];

        return null;
    }
    #endregion
}
