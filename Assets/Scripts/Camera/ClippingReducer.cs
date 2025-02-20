using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingReducer : MonoBehaviour
{   
    [SerializeField] float clipRange = 10f;
    public PlayerMovement playerMovement;


    // Update is called once per frame
    void FixedUpdate()
    {
        //ClipCheck();
    }

    private void ClipCheck()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 2f, Color.red);
        if (Physics.Raycast(transform.position, fwd, out hit, 2f))
        {
            playerMovement.lockForwardMovement = true;
            Debug.DrawRay(transform.position, fwd * 2f, Color.green);

        }
        else
        {
            playerMovement.lockForwardMovement = false;
            //Debug.Log("Forward Movement Locked? : " + playerMovement.lockForwardMovement);
        }
        
    }
}
