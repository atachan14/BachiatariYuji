using UnityEngine;

public class DialogWindowManager : SingletonMonoBehaviour<DialogWindowManager>
{
    [SerializeField] private GameObject dialogWindow;

    public void EnterDialogMode()
    {
        dialogWindow.SetActive(true);
        InputReceiver.Instance.SwitchMode(InputMode.Dialog);
    }

    public void ExitDialogMode()
    {
        dialogWindow.SetActive(false);
        InputReceiver.Instance.SwitchMode(SceneData.Instance.GetInputMode());
    }
}
