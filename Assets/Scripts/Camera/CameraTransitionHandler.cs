using UnityEngine;
using System.Collections;

public class CameraTransitionHandler : MonoBehaviour
{
    public Camera playerCam;
    public Camera targetCam;
    public Player player;
    public float transitionSpeed;
    public PlayerMovement playerMovement;
    public GameObject playerModel;

    //Placeholder until arms are combined with full character model.
    public GameObject pill;

    private Vector3 originalPlayerCamPosition;
    private Quaternion originalPlayerCamRotation;

    private Vector3 originalTargetCamPosition;
    private Quaternion originalTargetCamRotation;

    private void Start()
    {
        // Store original positions at the beginning
        
    }

    IEnumerator SwitchToTargetCam(float transitionDuration)
    {   
        StoreInitialTransforms();
        player.DisablePlayerMovement();
        playerCam.GetComponent<PlayerCam>().canRotate = false;
        targetCam.GetComponent<StaticCamController>().canRotateStatic = false;


        Vector3 startPosition = playerCam.transform.position;
        Quaternion startRotation = playerCam.transform.rotation;

        Vector3 targetPosition = originalTargetCamPosition;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

        float elapsedTime = 0f;

        // Ensure the target camera starts at the player camera's current rotation
        targetCam.transform.rotation = startRotation;

        // Enable the target camera BEFORE transitioning
        targetCam.enabled = true;
        targetCam.tag = "MainCamera";
        playerCam.enabled = false;
        playerCam.tag = "Untagged";

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = t * t * (3f - 2f * t); // Smoothstep for smoother transition

            targetCam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            targetCam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetCam.transform.position = originalTargetCamPosition;
        targetCam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //playerCam.GetComponent<PlayerCam>().canRotate = true;
        targetCam.GetComponent<StaticCamController>().canRotateStatic = true;
        playerModel.SetActive(false);
        pill.GetComponent<MeshRenderer>().enabled = false;
    }



    IEnumerator SwitchToPlayerCam(float transitionDuration)
    {
        playerCam.GetComponent<PlayerCam>().canRotate = false;
        targetCam.GetComponent<StaticCamController>().canRotateStatic = false; // Ensure target cam doesn't keep rotation input

        Vector3 startPosition = targetCam.transform.position;
        Quaternion startRotation = targetCam.transform.rotation;

        Vector3 targetPosition = originalPlayerCamPosition;
        Quaternion targetRotation = originalPlayerCamRotation;

        float elapsedTime = 0f;

        // Reset player camera rotation before enabling
        playerCam.transform.rotation = startRotation;

        // Enable the player camera at the beginning of the transition
        playerCam.enabled = true;
        playerCam.tag = "MainCamera";
        targetCam.enabled = false;
        targetCam.tag = "Untagged";

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = t * t * (3f - 2f * t); // Smoothstep for smoother transition

            playerCam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            playerCam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCam.transform.position = targetPosition;
        playerCam.transform.rotation = targetRotation;

        // Restore target cam's original position and reset its rotation so it doesn't retain input changes
        targetCam.transform.position = originalTargetCamPosition;
        targetCam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        playerCam.GetComponent<PlayerCam>().canRotate = true;
        player.EnablePlayerMovement();
        playerModel.SetActive(true);
        pill.GetComponent<MeshRenderer>().enabled = true;
    }




    public void StartSwitchToTargetCam(float transitionDuration)
    {
        StartCoroutine(SwitchToTargetCam(transitionDuration));
    }

    public void StartSwitchToPlayerCam(float transitionDuration)
    {
        StartCoroutine(SwitchToPlayerCam(transitionDuration));
    }

    private void StoreInitialTransforms()
    {
        originalPlayerCamPosition = playerCam.transform.position;
        originalPlayerCamRotation = playerCam.transform.rotation;

        originalTargetCamPosition = targetCam.transform.position;
        originalTargetCamRotation = targetCam.transform.rotation;
    }

}
