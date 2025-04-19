using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecurityCamController : MonoBehaviour
{
    public GameObject securityCamTransform;
    private ComputerScreenHandler computerScreenHandler;

    public bool camControlsEnabled = false;

    private void Start()
    {
        computerScreenHandler = GameObject.FindGameObjectWithTag("MainComputerScreen").GetComponent<ComputerScreenHandler>();
    }
    private void Update()
    {
        if (computerScreenHandler.isTurnedOn)
        {
            camControlsEnabled = true;
        }
        else
        {
            camControlsEnabled = false;
        }
    }

    public void HandleCamControls(InputAction.CallbackContext context)
    {
        if (camControlsEnabled)
        {
            Vector2 input = context.ReadValue<Vector2>();
            float rotationSpeed = 100f;

            float deltaRotation = input.x * rotationSpeed * Time.deltaTime;

            float currentYRotation = securityCamTransform.transform.localEulerAngles.y;
            currentYRotation = (currentYRotation > 180) ? currentYRotation - 360 : currentYRotation; // Convert to -180 to 180 range

            bool rotatingRight = deltaRotation > 0;
            bool rotatingLeft = deltaRotation < 0;

            bool canRotate =
                (currentYRotation < 80f && rotatingRight) ||
                (currentYRotation > -60f && rotatingLeft);

            if (canRotate)
            {
                securityCamTransform.transform.Rotate(Vector3.up, deltaRotation, Space.Self);
            }
        }
    }


}
