// SnakeHead.cs
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private PunishParams para;
    [SerializeField] AnimationCurve curve;
    [SerializeField]  float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        YujiParams.Instance.TakeDamage((int)para.damage, Color.green);
        YujiState.Instance.TakePosion(new PoisonEffect(EffectType.Hallucinations, curve,para.effectValue, duration));
    }
}
