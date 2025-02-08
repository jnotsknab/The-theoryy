using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHelper
{
    public void SetMousePos(Vector3 worldPos)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
        Mouse.current.WarpCursorPosition(screenPoint);
    }
}
