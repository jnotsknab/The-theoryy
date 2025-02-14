using System.Collections;
using UnityEngine;

/// <summary>
/// Handles item animations, setting animation states, and managing transforms for picked-up items.
/// </summary>
public class NewItemPickupHandler : MonoBehaviour
{
    public static NewItemPickupHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Transform equipPos;
    public Camera playerCam;
    public float throwForce = 1.5f;
    public bool pickedUp = false;
    public int? currentItemIDGlobal;

    public InteractableObject interactable;

    [Header("Animation")]
    public Animator armAnimator; // Animator for the player's arms
    public Animator itemAnimator; // Animator for the gun/item
    private static readonly int PickupTrigger = Animator.StringToHash("EmptyPickup");
    private AnimationUtils animationUtils;

    private GameObject currentItem;
    private string currentItemName;

    [Header("Item Data")]
    public ItemData itemData;

    private void Start()
    {
        animationUtils = gameObject.AddComponent<AnimationUtils>();
        ItemManager.InitializeItems();
    }

    public void SetItemAnimator(Animator newItemAnimator)
    {
        itemAnimator = newItemAnimator;
    }

    //Remember there are two seperate items, the animated one which is always in the correct position its just inactive, the second object is the one that is on the ground and is set inactive after we pick it up, then the animated item is set active, so remember, in order to retrieve correct item ID, the grounedItem, and the animatedItem must have the same name. 
    public void PickupItem(GameObject item, Animator newItemAnimator = null)
    {
        currentItem = item;
        interactable = currentItem.GetComponent<InteractableObject>();

        currentItemName = currentItem.transform.name;
        Debug.Log($"Picked up {currentItemName}");

        
        armAnimator.SetTrigger(PickupTrigger);

        // Set the item animator dynamically here
        if (newItemAnimator != null)
        {
            SetItemAnimator(newItemAnimator);
        }

        StartCoroutine(HandlePickupAnimation());
    }

    public void DropItem(string armAnimName, string itemAnimName)
    {
        if (currentItem == null)
        {
            Debug.Log("Cannot Drop Item is null");
            return;
        }

        StartCoroutine(DropItemRoutine(armAnimator, itemAnimator, armAnimName, itemAnimName));
    }

    private IEnumerator HandlePickupAnimation()
    {
        while (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        currentItem.SetActive(false);
        SetLayers(currentItem, "Items");
        currentItem.transform.position = equipPos.position;
        currentItem.transform.parent = equipPos;

        EmptyPickupEventHandlers();
    }



    private IEnumerator DropItemRoutine(Animator armAnimator, Animator itemAnimator, string armAnimName, string itemAnimName)
    {
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, "ItemDropLayer", armAnimName));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(itemAnimator, "ItemDropLayer", itemAnimName));

        //Wait before deactivating the item
        AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(armAnimator.GetLayerIndex("ItemDropLayer"));
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        //Reset the animated item's animator and the animators of its children to ensure were in the correct state when the item is picked up again, then disable the item.
        if (itemAnimator.gameObject.activeSelf)
        {
            animationUtils.ResetAnimator(itemAnimator);
            foreach (Animator childAnimator in itemAnimator.GetComponentsInChildren<Animator>())
            {
                animationUtils.ResetAnimator(childAnimator);
            }
            itemAnimator.gameObject.SetActive(false);
        }

        //Reinstantiate the item we picked up off the ground
        currentItem.transform.parent = null;
        currentItem.SetActive(true);

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerCam.transform.forward * throwForce, ForceMode.Impulse);
        }
        SetLayers(currentItem, "GroundedItems");

        currentItem = null;
        pickedUp = false;

        //THIS IS UHHHH FUCKED...
        armAnimator.gameObject.SetActive(false);
        armAnimator.gameObject.SetActive(true);

        Debug.Log("Dropped item");

    }

    private void SetLayers(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in obj.transform)
        {
            SetLayers(child.gameObject, layerName);
        }
    }

    private void EmptyPickupEventHandlers()
    {
        int? currentItemID = ItemManager.GetItemID(currentItemName);
        if (currentItemID.HasValue)
        {
            currentItemIDGlobal = currentItemID.Value;
            itemData = ItemManager.GetItemData(currentItemID.Value);
            Debug.Log($"Item id is {currentItemID}");
        }
        else
        {   
            Debug.LogError($"Invalid item name: {currentItemName}");
            currentItemIDGlobal = null;
        }
        if (itemData == null)
        {
            Debug.LogWarning($"Failed to get ItemData for ID {currentItemID}");
            return;
        }
        StartCoroutine(OnItemPickupComplete(
            armAnimator,
            itemAnimator,
            itemData.pickupLayer,
            itemData.pickupAnimation,
            itemData.movementLayer,
            itemData.movementAnimation
        ));
    }

    public IEnumerator OnItemPickupComplete(Animator armAnimator, Animator itemAnimator, string pickupLayer, string pickupAnimation, string movementLayer, string movementAnimation)
    {
        pickedUp = true;
        
        //Remember to ensure that entry layer contains the pickup anim as entry because setting it as active here plays that animation since were on always animate.
        itemAnimator.gameObject.SetActive(true);
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, pickupLayer, pickupAnimation));

        //Wait for arm pickup anim to complete, inadvertanly makes us wait for the item pickup anim too.
        AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(armAnimator.GetLayerIndex(pickupLayer));
        yield return new WaitForSeconds(stateInfo.length);

        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, movementLayer, movementAnimation));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(itemAnimator, movementLayer, movementAnimation));

        Debug.Log($"Animation event triggered: {pickupAnimation} equipped!");
    }

}
