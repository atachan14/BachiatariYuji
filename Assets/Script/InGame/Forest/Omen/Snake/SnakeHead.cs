// SnakeHead.cs
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private PunishParams para;

    private void OnTriggerEnter2D(Collider2D other)
    {
        YujiParams.Instance.TakeDamage((int)para.damage, Color.green);
        YujiState.Instance.TakeTimeEffect(new TimeEffect(EffectType.Hallucinations, para.effectValue, 10));
    }
}
