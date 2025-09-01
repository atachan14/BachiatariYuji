using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public enum InputMode
{
    TopDown, SideScroll, Dialog
}
public class InputReceiver : SingletonMonoBehaviour<InputReceiver>
{
    private InputMode Mode;

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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RefreshMode;
    }


    public void SwitchMode(InputMode newMode)
    {
        Mode = newMode;
        ResetInputs();
    }

    public void RefreshMode()
    {
        SwitchMode(SceneData.Instance.GetInputMode());
    }

    public void RefreshMode(Scene s, LoadSceneMode m)
    {
        SwitchMode(SceneData.Instance.GetInputMode());
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
        Confirm = Input.GetKeyDown(KeyCode.E);
        Up = Input.GetKeyDown(KeyCode.W);
        Down = Input.GetKeyDown(KeyCode.S);
        Left = Input.GetKeyDown(KeyCode.A);
        Right = Input.GetKeyDown(KeyCode.D);
    }
}
