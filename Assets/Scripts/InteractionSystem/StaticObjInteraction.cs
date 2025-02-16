using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the rayCasts on layer 15, i.e. the static interacatble object layer.
/// </summary>
public class StaticObjInteraction : MonoBehaviour
{

    [SerializeField] float interactionRange = 3f;
    private LayerMask interactableLayer;
    private InteractableObject interactable;

    void Awake()
    {
        interactableLayer = (1 << 15);
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
            if (newInteractable != null && newInteractable != interactable)
            {
                interactable = newInteractable;
            }

            // Interaction logic
            if (interactable != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    interactable.AlternateInteract();
                }
            }
        }

    }
}
