using UnityEngine;
using UnityEngine.SceneManagement;

public class Yuji : SingletonMonoBehaviour<Yuji>
{

    [SerializeField] GameObject TopDown;
    [SerializeField] GameObject SideScroll;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += RefreshMode;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshMode;
    }

    public void SwitchMode(SceneViewMode newMode)
    {

        TopDown.SetActive(newMode == SceneViewMode.TopDown);
        SideScroll.SetActive(newMode == SceneViewMode.SideScroll);
    }

    public void RefreshMode(Scene s,LoadSceneMode m) 
    {
        SwitchMode(SceneData.Instance.SceneViewMode);
    }


}
