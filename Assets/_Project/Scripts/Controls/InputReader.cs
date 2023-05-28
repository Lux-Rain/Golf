using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader: Controls.ICameraActions, Controls.IGamesActions
{
    public Action<Vector2> OnCameraMoveEvent;
    public Action<bool> OnValidCameraMoveEvent;
    public Action<bool> OnClickEvent;

    private Controls _controls;
    
    public InputReader()
    {
        _controls = new Controls();
        _controls.Camera.SetCallbacks(this);
        _controls.Camera.Enable();
        _controls.Games.SetCallbacks(this);
        _controls.Games.Enable();
    }

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        OnCameraMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnValidCameraMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnValidCameraMoveEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            OnValidCameraMoveEvent?.Invoke(false);
        }
    }

    public Vector3 MousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnClickEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            OnClickEvent?.Invoke(false);
        }
    }
}
