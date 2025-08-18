using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public enum InputMode
{
    TopDown, SideScroll, Talk
}
public class InputReceiver : DDOL_child<InputReceiver>
{
    private InputMode Mode;


    public Vector2 MoveAxis { get; private set; }
    public bool PlayE {  get; private set; }

    public float MoveAxisX { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    public void SwitchMode(InputMode newMode)
    {
        Mode = newMode;
        ResetInputs();
    }


    void ResetInputs()
    {
        MoveAxis = Vector2.zero;
        MoveAxisX = 0;
        JumpPressed = false;
        CrouchHeld = false;
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
            case InputMode.Talk:
                TalkInput();
                break;
        }
    }

    void TopDownInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveAxis = new Vector2(h, v).normalized;

        PlayE = Input.GetKeyDown(KeyCode.E);
    }

    void SideScrollInput()
    {
        MoveAxisX = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetKeyDown(KeyCode.W);
        CrouchHeld = Input.GetKey(KeyCode.S);
    }

    void TalkInput()
    {

    }
}
