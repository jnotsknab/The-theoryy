using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimefluxLogic : MonoBehaviour
{
    public TimeBody body;
    public void OnStartRewind(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            body.StartRewind();
        }
    }

    public void OnStopRewind(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            body.StopRewind();
        }
    }
}
