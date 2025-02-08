using UnityEngine;
using System.Collections;

public class CameraTransitionHandler : MonoBehaviour
{   
    public Camera playerCam;
    public Camera targetCam;

    [Header("Player References")]
    public Player player;
    public float transitionSpeed;
    public PlayerMovement playerMovement;
    public GameObject playerModel;
    public GameObject playerArms;
    //Placeholder until arms are combined with full character model.
    public GameObject pill;

    [Header("Items and Item Logic")]
    private ItemPickupHandler itemPickupHandler;
    public GameObject sawedOff;
    private ShotgunLogic shotgunLogic;

    [SerializeField] private Vector3 originalPlayerCamPosition;
    [SerializeField] private Quaternion originalPlayerCamRotation;
    [SerializeField] private Vector3 originalTargetCamPosition;
    [SerializeField] private Quaternion originalTargetCamRotation;

    private void Start()
    {
        itemPickupHandler = player.GetComponent<ItemPickupHandler>();
        
        //Shotgun Logic needed to disable the renderer on the shotgun shells after we enable all renderers attached to the player.
        //Shitty solution should find a more scaleable fix in the future.
        shotgunLogic = sawedOff.GetComponent<ShotgunLogic>();
    }

    /// <summary>
    /// Smoothly transitions from the Playercam to the Targetcam.
    /// </summary>
    /// <param name="transitionDuration"></param>
    /// <returns></returns>
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
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / transitionDuration);

            targetCam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            targetCam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetCam.transform.position = originalTargetCamPosition;
        targetCam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //playerCam.GetComponent<PlayerCam>().canRotate = true;
        targetCam.GetComponent<StaticCamController>().canRotateStatic = false;

        DisablePlayerRender();
        pill.GetComponent<MeshRenderer>().enabled = false;
    }


    /// <summary>
    /// Smoothly transitions from the Targetcam to the Playercam.
    /// </summary>
    /// <param name="transitionDuration"></param>
    /// <returns></returns>
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
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / transitionDuration);

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


        EnablePlayerRender();

        //Disable shell renderers after enabling them from the EnablePlayerRender as they arent active until their animations are played.
        shotgunLogic.ResetShells();

        pill.GetComponent<MeshRenderer>().enabled = true;
    }




    /// <summary>
    /// Helper Function to start the targetCam couroutine transition.
    /// </summary>
    /// <param name="transitionDuration"></param>
    public void StartSwitchToTargetCam(float transitionDuration)
    {
        StartCoroutine(SwitchToTargetCam(transitionDuration));
    }

    /// <summary>
    /// Helper Function to start the playerCam couroutine transition.
    /// </summary>
    /// <param name="transitionDuration"></param>
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

    /// <summary>
    /// Disables the player renderer and all the renderers of the players child objects.
    /// </summary>
    private void DisablePlayerRender()
    {
        playerModel.GetComponent<Renderer>().enabled = false;
        foreach (Renderer renderer in playerModel.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Enables the player renderer and all the renderers of the players child objects.
    /// </summary>
    private void EnablePlayerRender()
    {
        playerModel.GetComponent<Renderer>().enabled = true;
        foreach (Renderer renderer in playerModel.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }
    }



}
