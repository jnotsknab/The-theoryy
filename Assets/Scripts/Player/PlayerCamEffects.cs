using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamEffects : MonoBehaviour
{
    [Header("Movement Reference")]
    public PlayerMovement playerMovement;
    public float walkBobSpeed = 8f; 
    public float crouchBobSpeed = 6f;
    public float sprintBobSpeed = 14f; 
    public float bobAmount = 0.325f; 
    public float returnSpeed = 5f;

    private float defaultYPosition;
    private float timer = 0f;
    private float currentYPosition;
    

    void Start()
    {
        defaultYPosition = transform.localPosition.y;
        currentYPosition = defaultYPosition;
    }

    void Update()
    {
        HandleCameraBob();
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void HandleCameraBob()
    {
        if (IsPlayerMoving() && playerMovement.grounded)
        {
            float bobSpeed;

            // Determine the bob speed based on the player's state
            if (playerMovement.IsCrouching())
            {
                bobSpeed = crouchBobSpeed;
            }
            else if (playerMovement.IsSprinting())
            {
                bobSpeed = sprintBobSpeed;
            }
            else
            {
                bobSpeed = walkBobSpeed;
            }

            // Regular walking/sprinting/crouching bob
            timer += bobSpeed * Time.deltaTime;
            currentYPosition = defaultYPosition + Mathf.Sin(timer) * bobAmount;
        }
        else
        {
            currentYPosition = Mathf.Lerp(currentYPosition, defaultYPosition, Time.deltaTime * returnSpeed);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, currentYPosition, transform.localPosition.z);
    }

}
