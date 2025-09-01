using System.Collections.Generic;
using UnityEngine;

public class OmenManager : SingletonMonoBehaviour<OmenManager>
{
    public void Setup()
    {
        var omens = GetComponentsInChildren<Omen>();
        foreach (var om in omens)
        {
            om.SelectPunish();
        }
            
    }

    public GameObject PunishDestinyPick(List<GameObject> prefabs)
    {
        System.Random Rng = new(GameData.Instance.DaySeed);

        float dayEvil = DayData.Instance.DayEvil;
        float totalWeight = 0f;
        List<float> weights = new();

        foreach (var prefab in prefabs)
        {
            var punish = prefab.GetComponentInChildren<Punish>();
            if (punish == null || punish.destiny == null) { weights.Add(0f); continue; }

            var so = punish.destiny;
            float diff = dayEvil - so.peak;
            float gauss = Mathf.Exp(-(diff * diff) / (2f * so.sigma * so.sigma));
            float weight = so.baseWeight + so.rarity * gauss;
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return prefabs[^1];

        float pick = (float)(Rng.NextDouble() * totalWeight);
        float accum = 0f;

        for (int i = 0; i < prefabs.Count; i++)
        {
            accum += weights[i];
            if (pick <= accum) return prefabs[i];
        }

        return prefabs[^1];
    }
}
