using System.Collections;
using UnityEngine;

public class ScreenSpaceUIHandler : MonoBehaviour
{
    public GameObject uiPopup;
    
    public GameObject targetGameObject; // Reference to the GameObject you want to check against
    public ComputerScreenHandler computerScreenHandler;
    public float maxDistance; // Maximum distance within which the popup will show
    public float fadeDuration = 1f; // Duration of fade in/out
    private float distance; // Distance between the player and the target GameObject
    public KeyCode interactKey = KeyCode.E;
    private bool on = false;

    private Camera playerCamera;
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;
    

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;

        // Ensure the uiPopup has a CanvasGroup
        canvasGroup = uiPopup.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiPopup.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f; // Start as invisible
        uiPopup.SetActive(false);

        if (targetGameObject == null)
        {
            Debug.LogError("UI: Target GameObject is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ComputerUIPopupCheck();
    }

    private void ComputerUIPopupCheck()
    {
        if (targetGameObject.activeSelf)
        {
            // Calculate the distance between the player and the target GameObject
            distance = Vector3.Distance(playerCamera.transform.position, targetGameObject.transform.position);
        }
        else
        {
            distance = maxDistance + 1f;
        }
        

        // Check if the player is within the max distance of the target GameObject
        if (distance <= maxDistance)
        {
            if (Input.GetKeyDown(interactKey) && uiPopup.activeSelf && !on)
            {
                computerScreenHandler.TurnOnComputer();
               /* targetGameObject.SetActive(false);*/
                uiPopup.SetActive(false);
                on = true; // Set the computer state to "on"
                Debug.LogWarning("If statement turning on computer has been entered");
            }
            else if (Input.GetKeyDown(interactKey) && on)
            {
                computerScreenHandler.TurnOffComputer();
                targetGameObject.SetActive(true);
                uiPopup.SetActive(true);
                on = false; // Set the computer state to "off"
                Debug.LogWarning("If statement turning off computer has been entered");
            }

            // Fade in the popup
            if (canvasGroup.alpha < 1f && (fadeCoroutine == null || !uiPopup.activeSelf))
            {
                StartFade(true); // Start fade-in
            }
        }
        else
        {
            // Fade out the popup
            if (canvasGroup.alpha > 0f && fadeCoroutine == null)
            {
                StartFade(false); // Start fade-out
            }
        }
    }
    private void StartFade(bool fadeIn)
    {
        // Stop the current coroutine if one is active
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start the appropriate fade coroutine
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(fadeIn));
    }

    private IEnumerator FadeCanvasGroup(bool fadeIn)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        if (fadeIn)
        {
            uiPopup.SetActive(true); // Ensure the popup is active during fade-in
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly interpolate the alpha value
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = endAlpha; // Ensure the final alpha is exactly the target value

        if (!fadeIn)
        {
            uiPopup.SetActive(false); // Deactivate the popup after fade-out
        }

        fadeCoroutine = null; // Clear the coroutine reference
    }
}
