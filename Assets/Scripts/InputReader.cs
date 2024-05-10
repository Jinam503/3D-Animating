using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IGamePlayActions
{
    private PlayerInput _playerInput;
    
    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            
            _playerInput.GamePlay.SetCallbacks(this);

            SetGamePlay();
        }
    }
    private void SetGamePlay()
    {
        _playerInput.Enable();
    }
    
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    
    public event Action StartRunEvent;
    public event Action StopRunEvent;

    public event Action StartAimEvent;
    public event Action StopAimEvent;
    public event Action StartFireEvent;
    public event Action StopFireEvent;

    public event Action ReloadEvent;
    public event Action ThrowGrenadeEvent;

    public event Action MainWeaponEvent;
    public event Action SubWeaponEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartRunEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                StopRunEvent?.Invoke();
                break;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ReloadEvent?.Invoke();
        }
    }

    public void OnGrenade(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ThrowGrenadeEvent?.Invoke();
        }
    }

    public void OnMainWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MainWeaponEvent?.Invoke();
        }
    }

    public void OnSubWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SubWeaponEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartFireEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                StopFireEvent?.Invoke();
                break;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartAimEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                StopAimEvent?.Invoke();
                break;
        }
    }
}
