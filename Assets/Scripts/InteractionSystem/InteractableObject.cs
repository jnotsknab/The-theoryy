using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    void Interact();
    void AlternateInteract();

    void TertiaryInteract();
}

public class InteractableObject : MonoBehaviour, IInteractable
{
    public UnityEvent onInteract;
    public UnityEvent onAlternateInteract;
    public UnityEvent onTertiaryInteract;

    private void Awake()
    {
        if (onInteract == null)
            onInteract = new UnityEvent();
        if (onAlternateInteract == null)
            onAlternateInteract = new UnityEvent();
    }

    /// <summary>
    /// Serves as the primary event binding for interactions.
    /// </summary>
    public void Interact()
    {
        Debug.Log($"{gameObject.name} Interacted!");
        onInteract?.Invoke();
    }

    /// <summary>
    /// Serves as a second event binding for interactions.
    /// </summary>
    public void AlternateInteract()
    {
        Debug.Log($"{gameObject.name} Alternate Interacted!");
        onAlternateInteract?.Invoke();
    }

    /// <summary>
    /// Serves as a third event binding for interactions.
    /// </summary>
    public void TertiaryInteract()
    {
        onTertiaryInteract?.Invoke();
    }
}
