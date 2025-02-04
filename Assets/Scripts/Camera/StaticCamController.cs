using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamController : MonoBehaviour
{
    //public Transform orientation;

    public float sensX;
    public float sensY;
    float xRotation;
    float yRotation;

    public bool canRotateStatic = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotateStatic)
        {
            RotateStaticCam();
        }
        
    }

    private void RotateStaticCam()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

}
