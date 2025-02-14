using UnityEngine;

/// <summary>
/// Handles raycasts on layers 13 and 14, i.e. the item interactable layers.
/// </summary>
public class ItemInteraction : MonoBehaviour
{
    [SerializeField] float interactionRange = 3f;
    private LayerMask interactableLayer;
    private InteractableObject interactable;

    int frameCounter = 0;

    void Awake()
    {
        // Using bit shifting to enable layers 13 and 14
        interactableLayer = (1 << 13) | (1 << 14);
    }

    void Update()
    {
       RayCheck();
    }

    private void RayCheck()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, interactionRange, interactableLayer))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            InteractableObject newInteractable = hit.collider.GetComponent<InteractableObject>();

            // Update interactable only if it's a new interactable object
            if (newInteractable != null && !NewItemPickupHandler.Instance.pickedUp && newInteractable != interactable)
            {
                interactable = newInteractable;
            }

            
            // Interaction logic
            if (interactable != null)
            {
                if (!NewItemPickupHandler.Instance.pickedUp && Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    interactable.AlternateInteract();
                }
            }
        }

        if (NewItemPickupHandler.Instance.pickedUp && Input.GetKeyDown(KeyCode.Q)) interactable.TertiaryInteract();
    }


}
