
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
///TRASH SPHAGHETTI CODE DONT USE!!!!
///

//using System.Collections;
//using UnityEngine;

//public class ScreenSpaceUIHandler : MonoBehaviour
//{
//    public GameObject uiPopup;
//    public GameObject shotgunUIPopup;
    
//    public GameObject targetGameObject; // Reference to the GameObject you want to check against
//    public GameObject shotgunGameObject;
//    public ComputerScreenHandler computerScreenHandler;
//    public ItemPickupHandler itemPickupHandler;
//    public float maxDistance; // Maximum distance within which the popup will show
//    public float fadeDuration = 1f; // Duration of fade in/out
//    private float distance; // Distance between the player and the target GameObject
//    public KeyCode interactKey = KeyCode.E;
//    private bool on = false;
//    private bool uiFilled = false;
>>>>>>> Stashed changes

=======
///TRASH SPHAGHETTI CODE DONT USE!!!!
///

//using System.Collections;
//using UnityEngine;

//public class ScreenSpaceUIHandler : MonoBehaviour
//{
//    public GameObject uiPopup;
//    public GameObject shotgunUIPopup;
    
//    public GameObject targetGameObject; // Reference to the GameObject you want to check against
//    public GameObject shotgunGameObject;
//    public ComputerScreenHandler computerScreenHandler;
//    public ItemPickupHandler itemPickupHandler;
//    public float maxDistance; // Maximum distance within which the popup will show
//    public float fadeDuration = 1f; // Duration of fade in/out
//    private float distance; // Distance between the player and the target GameObject
//    public KeyCode interactKey = KeyCode.E;
//    private bool on = false;
//    private bool uiFilled = false;

>>>>>>> Stashed changes
//    private Camera playerCamera;
//    private CanvasGroup canvasGroup;
//    private Coroutine fadeCoroutine;
    

//    // Start is called before the first frame update
//    void Start()
//    {
//        playerCamera = Camera.main;

//        // Ensure the uiPopup has a CanvasGroup
//        canvasGroup = uiPopup.GetComponent<CanvasGroup>();
//        if (canvasGroup == null)
//        {
//            canvasGroup = uiPopup.AddComponent<CanvasGroup>();
//        }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        canvasGroup.alpha = 0f; // Start as invisible
        uiPopup.SetActive(false);
=======
//        canvasGroup.alpha = 0f; // Start as invisible
//        uiPopup.SetActive(false);
//        shotgunUIPopup.SetActive(false);
>>>>>>> Stashed changes
=======
//        canvasGroup.alpha = 0f; // Start as invisible
//        uiPopup.SetActive(false);
//        shotgunUIPopup.SetActive(false);
>>>>>>> Stashed changes

//        if (targetGameObject == null)
//        {
//            Debug.LogError("UI: Target GameObject is not assigned!");
//        }
//    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
//    // Update is called once per frame

//    void Update()
//    {   

//        // Reset uiFilled before any checks
//        uiFilled = false;

//        // Check all UIs
//        ComputerUIPopupCheck();
//        StandardUIPopupCheck(shotgunGameObject);

//        //Debug.Log($"Update: uiFilled = {uiFilled}");
//    }

//    private void ComputerUIPopupCheck()
//    {
//        if (targetGameObject.activeSelf)
//        {
//            // Calculate distance
//            distance = Vector3.Distance(playerCamera.transform.position, targetGameObject.transform.position);
//        }
//        else
//        {
//            distance = maxDistance + 1f;
//        }

//        //Debug.Log($"ComputerUIPopupCheck: distance = {distance}, maxDistance = {maxDistance}, uiFilled = {uiFilled}");

//        if (distance <= maxDistance)
//        {
//            if (Input.GetKeyDown(KeyCode.E) && uiPopup.activeSelf && !on)
//            {
//                computerScreenHandler.TurnOnComputer();
//                uiPopup.SetActive(false);
//                on = true; // Computer is on
//                uiFilled = true; // Block other UIs
//                Debug.Log("Computer turned ON");
//            }
//            else if (Input.GetKeyDown(KeyCode.Tab) && on)
//            {
//                computerScreenHandler.TurnOffComputer();
//                uiPopup.SetActive(false);
//                on = false; // Computer is off
//                uiFilled = false; // Block other UIs
//                Debug.Log("Computer turned OFF");
//            }

//            // Fade in
//            if (canvasGroup.alpha < 1f && (fadeCoroutine == null || !uiPopup.activeSelf))
//            {
//                StartFade(true);
//            }

//            uiFilled = true; // Ensure uiFilled is true when this UI is active
//        }
//        else
//        {
//            // Fade out
//            if (canvasGroup.alpha > 0f && fadeCoroutine == null)
//            {
//                StartFade(false);
//            }
//        }

//        //Debug.Log($"ComputerUIPopupCheck END: uiFilled = {uiFilled}, on = {on}, uiPopup.activeSelf = {uiPopup.activeSelf}");
//    }

//    private void StandardUIPopupCheck(GameObject targetObject)
//    {
//        if (targetObject.activeSelf)
//        {
//            // Calculate distance
//            distance = Vector3.Distance(playerCamera.transform.position, targetObject.transform.position);
//        }
//        else
//        {
//            distance = maxDistance + 1f;
//        }

//        //Debug.Log($"StandardUIPopupCheck: distance = {distance}, maxDistance = {maxDistance}, targetObject = {targetObject.name}, uiFilled = {uiFilled}");

//        if (distance <= maxDistance)
//        {
//            if (!uiFilled) // Only show if no other UI is active
//            {
//                shotgunUIPopup.SetActive(true);
//                uiFilled = true; // Block other UIs
//                //Debug.Log($"Shotgun UI activated for {targetObject.name}");
//            }
//        }
//        else if (distance >= maxDistance)
//        {
//            shotgunUIPopup.SetActive(false);
//            uiFilled = false;
//            //Debug.Log($"Shotgun UI deactivated for {targetObject.name}");
//        }
//        else
//        {
//            if (uiFilled)
//            {
//                shotgunUIPopup.SetActive(false);
//                //Debug.Log($"Shotgun UI deactivated for {targetObject.name}");
//            }
//        }

//        // If item is picked up, hide the UI
//        if (itemPickupHandler.pickedUp)
//        {
//            shotgunUIPopup.SetActive(false);
//            uiFilled = false; // Allow other UIs
//            //Debug.Log($"Item picked up. Hiding shotgun UI for {targetObject.name}");
//        }

//        //Debug.Log($"StandardUIPopupCheck END: uiFilled = {uiFilled}, shotgunUIPopup.activeSelf = {shotgunUIPopup.activeSelf}");
//    }

//    private void StartFade(bool fadeIn)
//    {
//        // Stop the current coroutine if one is active
//        if (fadeCoroutine != null)
//        {
//            StopCoroutine(fadeCoroutine);
//        }
>>>>>>> Stashed changes

//        // Start the appropriate fade coroutine
//        fadeCoroutine = StartCoroutine(FadeCanvasGroup(fadeIn));
//    }

//    private IEnumerator FadeCanvasGroup(bool fadeIn)
//    {
//        float startAlpha = canvasGroup.alpha;
//        float endAlpha = fadeIn ? 1f : 0f;
//        float elapsedTime = 0f;

<<<<<<< Updated upstream
        if (fadeIn)
        {
            uiPopup.SetActive(true); // Ensure the popup is active during fade-in
        }
=======
=======
//    // Update is called once per frame

//    void Update()
//    {   

//        // Reset uiFilled before any checks
//        uiFilled = false;

//        // Check all UIs
//        ComputerUIPopupCheck();
//        StandardUIPopupCheck(shotgunGameObject);

//        //Debug.Log($"Update: uiFilled = {uiFilled}");
//    }

//    private void ComputerUIPopupCheck()
//    {
//        if (targetGameObject.activeSelf)
//        {
//            // Calculate distance
//            distance = Vector3.Distance(playerCamera.transform.position, targetGameObject.transform.position);
//        }
//        else
//        {
//            distance = maxDistance + 1f;
//        }

//        //Debug.Log($"ComputerUIPopupCheck: distance = {distance}, maxDistance = {maxDistance}, uiFilled = {uiFilled}");

//        if (distance <= maxDistance)
//        {
//            if (Input.GetKeyDown(KeyCode.E) && uiPopup.activeSelf && !on)
//            {
//                computerScreenHandler.TurnOnComputer();
//                uiPopup.SetActive(false);
//                on = true; // Computer is on
//                uiFilled = true; // Block other UIs
//                Debug.Log("Computer turned ON");
//            }
//            else if (Input.GetKeyDown(KeyCode.Tab) && on)
//            {
//                computerScreenHandler.TurnOffComputer();
//                uiPopup.SetActive(false);
//                on = false; // Computer is off
//                uiFilled = false; // Block other UIs
//                Debug.Log("Computer turned OFF");
//            }

//            // Fade in
//            if (canvasGroup.alpha < 1f && (fadeCoroutine == null || !uiPopup.activeSelf))
//            {
//                StartFade(true);
//            }

//            uiFilled = true; // Ensure uiFilled is true when this UI is active
//        }
//        else
//        {
//            // Fade out
//            if (canvasGroup.alpha > 0f && fadeCoroutine == null)
//            {
//                StartFade(false);
//            }
//        }

//        //Debug.Log($"ComputerUIPopupCheck END: uiFilled = {uiFilled}, on = {on}, uiPopup.activeSelf = {uiPopup.activeSelf}");
//    }

//    private void StandardUIPopupCheck(GameObject targetObject)
//    {
//        if (targetObject.activeSelf)
//        {
//            // Calculate distance
//            distance = Vector3.Distance(playerCamera.transform.position, targetObject.transform.position);
//        }
//        else
//        {
//            distance = maxDistance + 1f;
//        }

//        //Debug.Log($"StandardUIPopupCheck: distance = {distance}, maxDistance = {maxDistance}, targetObject = {targetObject.name}, uiFilled = {uiFilled}");

//        if (distance <= maxDistance)
//        {
//            if (!uiFilled) // Only show if no other UI is active
//            {
//                shotgunUIPopup.SetActive(true);
//                uiFilled = true; // Block other UIs
//                //Debug.Log($"Shotgun UI activated for {targetObject.name}");
//            }
//        }
//        else if (distance >= maxDistance)
//        {
//            shotgunUIPopup.SetActive(false);
//            uiFilled = false;
//            //Debug.Log($"Shotgun UI deactivated for {targetObject.name}");
//        }
//        else
//        {
//            if (uiFilled)
//            {
//                shotgunUIPopup.SetActive(false);
//                //Debug.Log($"Shotgun UI deactivated for {targetObject.name}");
//            }
//        }

//        // If item is picked up, hide the UI
//        if (itemPickupHandler.pickedUp)
//        {
//            shotgunUIPopup.SetActive(false);
//            uiFilled = false; // Allow other UIs
//            //Debug.Log($"Item picked up. Hiding shotgun UI for {targetObject.name}");
//        }

//        //Debug.Log($"StandardUIPopupCheck END: uiFilled = {uiFilled}, shotgunUIPopup.activeSelf = {shotgunUIPopup.activeSelf}");
//    }

//    private void StartFade(bool fadeIn)
//    {
//        // Stop the current coroutine if one is active
//        if (fadeCoroutine != null)
//        {
//            StopCoroutine(fadeCoroutine);
//        }

//        // Start the appropriate fade coroutine
//        fadeCoroutine = StartCoroutine(FadeCanvasGroup(fadeIn));
//    }

//    private IEnumerator FadeCanvasGroup(bool fadeIn)
//    {
//        float startAlpha = canvasGroup.alpha;
//        float endAlpha = fadeIn ? 1f : 0f;
//        float elapsedTime = 0f;

>>>>>>> Stashed changes
//        if (fadeIn)
//        {
//            uiFilled = true;
//            uiPopup.SetActive(true); // Ensure the popup is active during fade-in
//        }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;

//            // Smoothly interpolate the alpha value
//            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

//            yield return null;
//        }

//        canvasGroup.alpha = endAlpha; // Ensure the final alpha is exactly the target value

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if (!fadeIn)
        {
            uiPopup.SetActive(false); // Deactivate the popup after fade-out
        }
=======
=======
>>>>>>> Stashed changes
//        if (!fadeIn)
//        {   
//            uiFilled = false;
//            uiPopup.SetActive(false); // Deactivate the popup after fade-out
//        }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

//        fadeCoroutine = null; // Clear the coroutine reference
//    }
//}

///// DEPRECATED