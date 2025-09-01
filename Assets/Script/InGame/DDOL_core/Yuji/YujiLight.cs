using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class YujiLight : SingletonMonoBehaviour<YujiLight>
{
    public Light2D yujiLight;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += RefreshLight;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshLight;
    }
    public void RefreshLight()
    {
        if (DayData.Instance.DayTime == DayTime.Night
            && SceneData.Instance.IsOutDoor)
        {
            yujiLight.gameObject.SetActive(true);
        }
        else
        {
            yujiLight.gameObject.SetActive(false);
        }
    }

    public void RefreshLight(Scene scene, LoadSceneMode mode)
    {
       RefreshLight();
    }
}


