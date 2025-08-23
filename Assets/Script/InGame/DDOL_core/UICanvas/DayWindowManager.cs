using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayWindowManager : SingletonMonoBehaviour<DayWindowManager>
{


    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image dayTimeImage;
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite nightSprite;


    public void ChangeDay()
    {
        dayText.text = GameData.Instance.Day.ToString();
    }

    public void ChangeDayTime()
    {
        switch (GameData.Instance.DayTime)
        {
            case DayTime.Morning:
                dayTimeImage.sprite = morningSprite;
                break;
            case DayTime.Night:
                dayTimeImage.sprite = nightSprite;
                break;
        }
    }
}
