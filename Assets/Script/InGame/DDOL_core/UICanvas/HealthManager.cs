using TMPro;
using UnityEngine;

public class HealthManager : SingletonMonoBehaviour<HealthManager>
{
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text maxText;
    [SerializeField] private TMP_Text slashText;
    [SerializeField] GameObject healthEffect;

    private void Start()
    {
        YujiParams.Instance.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        YujiParams.Instance.OnDamaged -= HandleDamaged;
    }

    private void HandleDamaged(int damage, Color color)
    {
        maxText.text = YujiParams.Instance.MaxHelth.ToString();
        valueText.text = YujiParams.Instance.Health.ToString();
        Debug.Log($"受け取った！ {damage} ダメージ 色:{color}");
        var go= Instantiate(healthEffect, transform);
        go.GetComponent<HealthEffect>().SetColor(damage,color);
    }
}
