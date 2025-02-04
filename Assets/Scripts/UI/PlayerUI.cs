using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Sprint Meter UI")]
    public Image sprintMeterImage;  // Reference to the sprint meter UI image
    public PlayerMovement playerMovement;

    private void Start()
    {
        
    }

    private void Update()
    {
        // Update the sprint meter based on the player's sprint time
        UpdateSprintMeter();
    }

    private void UpdateSprintMeter()
    {
        if (playerMovement != null)
        {
            // Update the UI meter (fill amount based on the player's current sprint time)
            sprintMeterImage.fillAmount = playerMovement.currentSprintTime / playerMovement.sprintDuration;
        }
    }
}
