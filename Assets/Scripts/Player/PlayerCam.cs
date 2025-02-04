using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public PlayerMovement playerMovement;

    public Transform camTrans;
    public Vector3 crouchingCamPos;
    public Vector3 standingCamPos;
    public float crouchingTranSpeed;
    public bool stopFollowingPlayer = false; // Add this bool to toggle camera follow

    public float xRotation;
    public float yRotation;

    public bool canRotate = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (standingCamPos == Vector3.zero)
        {
            standingCamPos = camTrans.localPosition;
        }
        
        crouchingCamPos = new Vector3(standingCamPos.x, standingCamPos.y - 2f, standingCamPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopFollowingPlayer)
        {
            camTrans.parent = transform; // Ensure camera follows player
        }
        else
        {
            camTrans.parent = null; // Detach camera from player
        }

        if (canRotate)
        {
            RotatePlayerCam();
        }
        

        if (Input.GetKey(playerMovement.crouchKey))
        {
            camTrans.localPosition = Vector3.Lerp(
                camTrans.localPosition,
                crouchingCamPos,
                crouchingTranSpeed * Time.deltaTime
            );
        }
        else
        {
            camTrans.localPosition = Vector3.Lerp(
                camTrans.localPosition,
                standingCamPos,
                crouchingTranSpeed * Time.deltaTime
            );
        }
    }

    private void RotatePlayerCam()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        //Clamp xRotation so the player can't look behind themselves
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate the camera and the orientation to the new rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
