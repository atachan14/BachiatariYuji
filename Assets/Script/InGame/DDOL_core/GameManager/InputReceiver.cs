using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public enum InputMode
{
    TopDown, SideScroll, Dialog
}
public class InputReceiver : SingletonMonoBehaviour<InputReceiver>
{
    private InputMode Mode;


    public Vector2 MoveAxis { get; private set; }
    public bool Action { get; private set; }

    public float MoveAxisX { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    public bool NextTalk { get; set; }

    public void SwitchMode(InputMode newMode)
    {
        Mode = newMode;
        ResetInputs();
    }


    void ResetInputs()
    {
        MoveAxis = Vector2.zero;
        Action = false;

        MoveAxisX = 0;
        JumpPressed = false;
        CrouchHeld = false;

        NextTalk = false;
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
                TalkInput();
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

    void TalkInput()
    {
        NextTalk = Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0);
    }
}
