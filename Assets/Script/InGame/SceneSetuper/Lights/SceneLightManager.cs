using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneLightManager : MonoBehaviour
{
    [SerializeField] private Light2D[] sceneLights;

    private void Start()
    {
        ApplyDayTime(DayData.Instance.DayTime);
    }

    public void ApplyDayTime(DayTime time)
    {
        bool enable = (time == DayTime.Night);

        foreach (var light in sceneLights)
        {
            if (light != null)
            {
                light.enabled = enable;
            }
        }
    }
}
