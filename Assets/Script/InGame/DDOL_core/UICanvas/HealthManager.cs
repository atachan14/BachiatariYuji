using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class HealthManager : SingletonMonoBehaviour<HealthManager>
{
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text maxText;
    [SerializeField] private TMP_Text slashText;
    [SerializeField] GameObject healthEffect;

    private void Start()
    {
        YujiParams.Instance.OnDamaged += HandleDamaged;
        YujiParams.Instance.OnHealed += HandleHealed;
    }

    private void OnDisable()
    {
        YujiParams.Instance.OnDamaged -= HandleDamaged;
        YujiParams.Instance.OnHealed -= HandleHealed;

    }

    private void HandleDamaged(int damage, Color color)
    {
        maxText.text = YujiParams.Instance.MaxHelth.ToString();
        valueText.text = YujiParams.Instance.Health.ToString();
        Debug.Log($"受け取った！ {damage} ダメージ 色:{color}");
        var go= Instantiate(healthEffect, transform);
        go.GetComponent<HealthEffect>().SetColor(damage,color);
    }

    private void HandleHealed(int heal)
    {
        Color color = Color.white;
        maxText.text = YujiParams.Instance.MaxHelth.ToString();
        valueText.text = YujiParams.Instance.Health.ToString();
        Debug.Log($"受け取った！ {heal} heal 色:{color}");
        var go = Instantiate(healthEffect, transform);
        go.GetComponent<HealthEffect>().SetColor(heal, color);
    }
}
