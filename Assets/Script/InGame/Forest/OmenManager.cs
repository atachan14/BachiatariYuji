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
        System.Random rng = new(GameData.Instance.DaySeed);

        float dayEvil = DayData.Instance.DayEvil;
        float totalWeight = 0f;
        List<float> weights = new();

        foreach (var prefab in prefabs)
        {
            var punish = prefab.GetComponentInChildren<Punish>();
            if (punish == null || punish.destiny == null)
            {
                weights.Add(0f);
                continue;
            }

            float weight = punish.destiny.GetWeight(dayEvil);
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return prefabs[^1];

        float pick = (float)(rng.NextDouble() * totalWeight);
        float accum = 0f;

        for (int i = 0; i < prefabs.Count; i++)
        {
            accum += weights[i];
            if (pick <= accum) return prefabs[i];
        }

        return prefabs[^1];
    }

}
