using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamEffects : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float walkBobSpeed = 8f;
    public float crouchBobSpeed = 3f;
    public float sprintBobSpeed = 14f; 
    public float bobAmount = 0.3f;
    public float returnSpeed = 5f; 
    public float landingBobAmount = 0.75f;
    public float landingBobSpeed = 10f; 

    private float defaultYPosition;
    private float timer = 0f;
    private float currentYPosition;
    public bool isLanding = false;
    

    void Start()
    {
        defaultYPosition = transform.localPosition.y;
        currentYPosition = defaultYPosition;
    }

    void Update()
    {
        if (IsPlayerMoving() && playerMovement.grounded)
        {
            float bobSpeed;

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

            if (isLanding)
            {
                currentYPosition = defaultYPosition - Mathf.PingPong(Time.time * landingBobSpeed, landingBobAmount);
                if (currentYPosition >= defaultYPosition - landingBobAmount * 0.1f) // Stop downward bob once it’s close enough to default
                {
                    isLanding = false; // Reset the landing state
                }
            }
            else
            {
                // Regular walking/sprinting/crouching bob
                timer += bobSpeed * Time.deltaTime;
                currentYPosition = defaultYPosition + Mathf.Sin(timer) * bobAmount;
            }
        }
        else
        {
            currentYPosition = Mathf.Lerp(currentYPosition, defaultYPosition, Time.deltaTime * returnSpeed);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, currentYPosition, transform.localPosition.z);
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    // Call this method from PlayerMovement when the player lands
    public void TriggerLandingEffect()
    {
        isLanding = true;
    }
}
