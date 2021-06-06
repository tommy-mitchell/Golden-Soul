using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInput : MonoBehaviour
{
    public event Action<string> controlsChanged;

    public event Action<float>   Player_onMove;
    public event Action<bool>    Player_onJump;
    public event Action          Player_onAttack;
    public event Action          Player_onThrow;
    public event Action          Player_onTeleport;
    public event Action          Player_onRecall;
    public event Action<Vector2> Player_onAim;

    public event Action<float> Hanging_onSwing;
    public event Action<bool>  Hanging_onJump;
    public event Action        Hanging_onDrop;
    public event Action        Hanging_onRecall;
    // UI events

    private PlayerInput PI;
    private int         movementCounter;

    private void Start()
    {
        PI              = GetComponent<PlayerInput>();
        movementCounter = 0;
    }

    public void OnControlsChanged() => controlsChanged?.Invoke(PI.currentControlScheme);

    // player controls

    public void OnMove(InputAction.CallbackContext context)
    {
        movementCounter++;

        if(movementCounter % 2 == 0) // skip every second input -> fixes movement on release bug
            return;

        Action<float> currEvent = PI.currentActionMap.name == "Player" ? Player_onMove : Hanging_onSwing;

        float value = context.ReadValue<float>();

        currEvent?.Invoke(value);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Action<bool> currEvent = PI.currentActionMap.name == "Player" ? Player_onJump : Hanging_onJump;
        
        currEvent?.Invoke(context.ReadValue<float>() != 0);
    }

    public void OnAttack() => Player_onAttack?.Invoke();
    
    public void OnThrow() => Player_onThrow?.Invoke();

    public void OnTeleport() => Player_onTeleport?.Invoke();

    public void OnRecall()
    {
        Action currEvent = PI.currentActionMap.name == "Player" ? Player_onRecall : Hanging_onRecall;

        currEvent?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Player_onAim?.Invoke(context.ReadValue<Vector2>());

        //if(PI.currentControlScheme != "Keyboard and Mouse")
          //  Debug.Log("aimInput: " + context.ReadValue<Vector2>());
    }

    // hanging controls

    public void OnDrop() => Hanging_onDrop?.Invoke();
}
