using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDUI : MonoBehaviour
{
    [Header("Sprint Meter UI")]
    public Image sprintMeterImage;  // Reference to the sprint meter UI image
    public PlayerMovement playerMovement;


    private void Update()
    {
        // Update the sprint meter based on the player's sprint time
        UpdateSprintMeter();
        UpdateSprintUI();
    }

    private void UpdateSprintMeter()
    {
        if (playerMovement != null)
        {
            // Update the UI meter (fill amount based on the player's current sprint time)
            sprintMeterImage.fillAmount = playerMovement.currentSprintTime / playerMovement.sprintDuration;
        }
    }

    private void UpdateSprintUI()
    {
        if (playerMovement.IsSprinting() && playerMovement.currentSprintTime > 0f)
        {
            playerMovement.currentSprintTime -= Time.deltaTime; // Deplete sprint meter when sprinting
        }
        else if (!playerMovement.IsSprinting() && playerMovement.currentSprintTime < playerMovement.sprintDuration)
        {
            playerMovement.currentSprintTime += Time.deltaTime * playerMovement.regenSpeed; // Regenerate sprint meter when not sprinting
        }
    }
}
