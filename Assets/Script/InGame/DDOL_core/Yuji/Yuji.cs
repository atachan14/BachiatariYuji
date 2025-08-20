using UnityEngine;

public class Yuji : SingletonMonoBehaviour<Yuji>
{

    [SerializeField] GameObject TopDown;
    [SerializeField] GameObject SideScroll;

    //private void OnEnable()
    //{
    //    SceneChanger.Instance.OnModeChanged += SwitchMode;
    //}

    //private void OnDisable()
    //{
    //    SceneChanger.Instance.OnModeChanged -= SwitchMode;
    //}

    public void SwitchMode(SceneViewMode newMode)
    {
        TopDown.transform.localPosition = Vector2.zero;
        SideScroll.transform.localPosition = Vector2.zero;

        TopDown.SetActive(newMode == SceneViewMode.TopDown);
        SideScroll.SetActive(newMode == SceneViewMode.SideScroll);
    }
}
