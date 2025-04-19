using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles states attributed to picking up items such as item animations, setting animation states, and managing transforms for picked-up items.
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

    public List<GameObject> groundedItems = new List<GameObject>();
    public Transform equipPos;
    public Camera playerCam;
    public bool pickedUp = false;
    public int? currentItemIDGlobal;
    public HotbarSlot hotBar;

    public bool isSwitchingItem = false;

    [Header("Drop Force")]
    public float throwForce = 1.5f;
    
    

    [Header("Animation")]
    public Animator armAnimator;
    public Animator itemAnimator;
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

    /// <summary>
    /// Handles the logic for picking up interactable items. This method should only be invoked by the item's interaction script.
    /// 
    /// <para>Each interactable item consists of two objects:</para>
    /// <para>1. The grounded item, which manages UI interactions and pickup detection.</para>
    /// <para>2. The animated item, which remains parented to the equip position but stays inactive until required.</para>
    /// 
    /// The method takes the grounded item's GameObject and an optional Animator reference. The animated item is activated in the 
    /// <see cref="PickupAnimRoutine(Animator, Animator, string, string, string, string)"/> coroutine.
    /// </summary>
    /// <param name="item">The GameObject representing the grounded item.</param>
    /// <param name="newItemAnimator">Optional Animator for the picked-up item. If provided, it will be assigned dynamically.</param>
    public void PickupItem(GameObject item, Animator newItemAnimator = null)
    {
        if (currentItem != null)
        {
            currentItem.SetActive(false);
            currentItem = null;
        }
        currentItem = item;
        //interactable = currentItem.GetComponent<InteractableObject>();

        currentItemName = currentItem.transform.name;
        Debug.Log($"Picked up {currentItemName}");

        
        armAnimator.SetTrigger(PickupTrigger);

        if (itemAnimator != null)
        {
            animationUtils.ResetAnimator(itemAnimator);
            foreach (Animator childAnimator in itemAnimator.GetComponentsInChildren<Animator>())
            {
                animationUtils.ResetAnimator(childAnimator);
            }
            itemAnimator.gameObject.SetActive(false);
        }
        // Set the item animator dynamically here
        if (newItemAnimator != null)
        {
            SetItemAnimator(newItemAnimator);
        }

        StartCoroutine(HandlePickupAnimation());
    }

    /// <summary>
    /// Handles the logic for dropping the currently held item. This method triggers the appropriate animations for both 
    /// the player's arm and the item being dropped.
    /// 
    /// If no item is currently held, the method logs a message and exits.
    /// </summary>
    /// <param name="armAnimName">The name of the animation to play for the arm.</param>
    /// <param name="itemAnimName">The name of the animation to play for the dropped item.</param>
    public void DropItem(string armAnimName, string itemAnimName)
    {   
        currentItem = groundedItems[currentItemIDGlobal.Value];
        if (currentItem == null)
        {
            Debug.Log("Cannot Drop Item is null");
            return;
        }

        StartCoroutine(DropItemRoutine(armAnimator, itemAnimator, armAnimName, itemAnimName));
    }

    /// <summary>
    /// Waits for the arm rig's pickup animation to complete before initializing the animated item's pickup process.  
    /// </summary>
    /// <returns>IEnumerator for coroutine execution.</returns>
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

        StartItemPickupSequence();
    }

    /// <summary>
    /// Handles the item drop process by playing the appropriate drop animations, resetting the item's state,  
    /// and reactivating the grounded item. This method ensures that the item's animator and all child animators  
    /// are reset before the item is deactivated, allowing it to be properly reinitialized when picked up again.  
    /// Additionally, it restores physics interactions and updates the item's layer assignment.  
    /// </summary>
    /// <param name="armAnimator">Animator for the player's arm rig.</param>
    /// <param name="itemAnimator">Animator for the dropped item.</param>
    /// <param name="armAnimName">Name of the arm animation to play during the drop.</param>
    /// <param name="itemAnimName">Name of the item's drop animation.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
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
            Debug.Log($"Animator reset with name {itemAnimator.gameObject.name}");
            animationUtils.ResetAnimator(itemAnimator);
            foreach (Animator childAnimator in itemAnimator.GetComponentsInChildren<Animator>())
            {
                animationUtils.ResetAnimator(childAnimator);
            }
            itemAnimator.gameObject.SetActive(false);
        }

        //Reinstantiate the item we picked up off the ground
        currentItem = groundedItems[currentItemIDGlobal.Value];
        currentItem.SetActive(true);
        currentItem.transform.parent = null;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            //rb.AddForce(playerCam.transform.forward * throwForce, ForceMode.Impulse);
        }
        SetLayers(currentItem, "GroundedItems");

        currentItem = null;
        pickedUp = false;

        //THIS IS UHHHH FUCKED...
        armAnimator.gameObject.SetActive(false);
        armAnimator.gameObject.SetActive(true);

        hotBar.SetSlotPopulated(hotBar.selectedIndex, false, 9999);
        currentItemIDGlobal = 9999;
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

    /// <summary>
    /// Fetches data for the item being picked up based on the current item ID, afterwards it start the coroutine which handles playing the animations for the item.
    /// </summary>
    private void StartItemPickupSequence()
    {   

        int? currentItemID = ItemManager.GetItemID(currentItemName);
        if (currentItemID.HasValue)
        {   
            hotBar.SelectFirstEmptySlot();
            currentItemIDGlobal = currentItemID.Value;
            itemData = ItemManager.GetItemData(currentItemID.Value);
            hotBar.SetSlotPopulated(hotBar.selectedIndex, true, currentItemIDGlobal.Value);
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
        StartCoroutine(PickupAnimRoutine(
            armAnimator,
            itemAnimator,
            itemData.pickupLayer,
            itemData.pickupAnimation,
            itemData.movementLayer,
            itemData.movementAnimation
        ));
    }

    private IEnumerator waitForArms()
    {
        while (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Similar to pickup item, except the method handles only the animated item, ideal for swapping items in a hotbar.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="newItemAnimator"></param>
    public void SwitchItem(int itemID, Animator newItemAnimator)
    {
        if (isSwitchingItem || itemID == currentItemIDGlobal) return; // prevent switching during animation

        isSwitchingItem = true;

        StartCoroutine(SwitchItemRoutine(itemID, newItemAnimator));
    }


    private IEnumerator SwitchItemRoutine(int itemID, Animator newItemAnimator)
    {
        yield return StartCoroutine(waitForArms());

        itemData = ItemManager.GetItemData(itemID);
        armAnimator.SetTrigger(PickupTrigger);

        Debug.Log($"Switch Item invoked with item id {itemID}");
        if (itemData != null)
        {
            currentItemIDGlobal = itemID;
            Debug.Log("Animator reset");
            animationUtils.ResetAnimator(itemAnimator);
            foreach (Animator childAnimator in itemAnimator.GetComponentsInChildren<Animator>())
            {
                animationUtils.ResetAnimator(childAnimator);
            }
            itemAnimator.gameObject.SetActive(false);

            if (newItemAnimator != null)
            {
                SetItemAnimator(newItemAnimator);
                Debug.Log($"New Item animator set as {newItemAnimator.name}");
            }

            Debug.Log($"Starting movement with movement layer {itemData.movementLayer}, and movement animation {itemData.movementAnimation}, and current item id is {currentItemIDGlobal.Value}");

            yield return StartCoroutine(PickupAnimRoutine(
                armAnimator,
                itemAnimator,
                itemData.pickupLayer,
                itemData.pickupAnimation,
                itemData.movementLayer,
                itemData.movementAnimation
            ));
        }

        isSwitchingItem = false; // allow switching again
    }


    /// <summary>
    /// This method completes the item pickup sequence and enters into the final animation state (movement) for the item.
    /// </summary>
    /// <param name="armAnimator"></param>
    /// <param name="itemAnimator"></param>
    /// <param name="pickupLayer"></param>
    /// <param name="pickupAnimation"></param>
    /// <param name="movementLayer"></param>
    /// <param name="movementAnimation"></param>
    /// <returns></returns>
    private IEnumerator PickupAnimRoutine(Animator armAnimator, Animator itemAnimator, string pickupLayer, string pickupAnimation, string movementLayer, string movementAnimation)
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
