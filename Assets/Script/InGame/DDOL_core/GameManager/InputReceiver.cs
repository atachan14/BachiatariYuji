using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public enum InputMode
{
    TopDown, SideScroll, Dialog, Direction
}
public class InputReceiver : SingletonMonoBehaviour<InputReceiver>
{
    private InputMode Mode = InputMode.Dialog;

    [Header("TopDown")]
    public Vector2 MoveAxis { get; private set; }
    public bool Action { get; private set; }

    [Header("SideScroll")]
    public float MoveAxisX { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    [Header("Dialog")]
    public bool Confirm { get; set; }
    public bool Up { get; set; }
    public bool Down { get; set; }
    public bool Left { get; set; }
    public bool Right { get; set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += RefreshMode;
        TitleManager.OnTitleMenu += HandleDialogMode;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshMode;
        TitleManager.OnTitleMenu -= HandleDialogMode;
    }

    void HandleDialogMode()
    {
        SwitchMode(InputMode.Dialog);
    }

    public void SwitchMode(InputMode newMode)
    {
        Debug.Log(newMode.ToString());
        Mode = newMode;
        ResetInputs();
    }

    public void RefreshMode()
    {
        SwitchMode(SceneData.Instance.GetInputMode());
    }

    public void RefreshMode(Scene s, LoadSceneMode m)
    {
        RefreshMode();
    }

    void ResetInputs()
    {
        MoveAxis = Vector2.zero;
        Action = false;

        MoveAxisX = 0;
        JumpPressed = false;
        CrouchHeld = false;

        Confirm = false;
    }

    void Update()
    {
        switch (Mode)
        {
            case InputMode.TopDown:
                TopDownInput();
                break;
            case InputMode.SideScroll:
                SideScrollInput();
                break;
            case InputMode.Dialog:
                DialogInput();
                break;
            case InputMode.Direction:
                Direction();
                break;
        }
    }

    void TopDownInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveAxis = new Vector2(h, v).normalized;

        Action = Input.GetKeyDown(KeyCode.E);
    }

    void SideScrollInput()
    {
        MoveAxisX = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetKeyDown(KeyCode.W);
        CrouchHeld = Input.GetKey(KeyCode.S);
    }

    void DialogInput()
    {
        Confirm = Input.GetKeyDown(KeyCode.E)
       || Input.GetKeyDown(KeyCode.Space)
       || Input.GetKeyDown(KeyCode.Return);
        Up = Input.GetKeyDown(KeyCode.W);
        Down = Input.GetKeyDown(KeyCode.S);
        Left = Input.GetKeyDown(KeyCode.A);
        Right = Input.GetKeyDown(KeyCode.D);
    }

    void Direction()
    {

    }
}
