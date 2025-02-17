
///DEPRACTED USE NEWITEMPICKUPHANDLER

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Handles picking and dropping items while playing their respective animations in sync with the player.
///// Notes: To make this more scalable for all items rename all arm and sawedoff layers to fit all items and have multiple animations of each type for each item in each layer,
///// Then ID all items or store them in a dictionary and start coroutines for specific items.
///// </summary>
//public class ItemPickupHandler : MonoBehaviour
//{   

//    public Camera playerCam;

//    public string ItemPickupLayer = "Items";
//    public string ItemGroundedLayer = "GroundedItems";
//    public Transform equipPos;
//    public float maxPickupDist;
//    public float throwForce = 1.5f;
//    public int currentItemID;
//    public ShotgunLogic sawedOffLogic;

//    [Header("Animation")]
//    public Animator armAnimator; // Animator for the player's arms
//    public Animator gunAnimator; // Animator for the gun/item
//    private static readonly int PickupTrigger = Animator.StringToHash("EmptyPickup");
//    private static readonly int EmptyState = Animator.StringToHash("Empty");
//    private static readonly int DropTrigger = Animator.StringToHash("Drop");
//    //private static readonly int EquipTrigger = Animator.StringToHash("Equip");
//    private static readonly int EquipBool = Animator.StringToHash("HasEquipped");

//    private AnimationUtils animationUtils;

//    GameObject currentItem;
//    GameObject item;

//    [Header("Item Data")]
//    public ItemData itemData;


//    bool canPickup;
//    public bool pickedUp = false;
//    public bool shotgunPickedUp = false;
//    public bool sawedoffPickedUp = false;
//    private string currentItemName;


//    private void Start()
//    {
//        animationUtils = gameObject.AddComponent<AnimationUtils>();

//        ItemManager.InitializeItems();
//    }
//    private void Update()
//    {
//        CheckItems();

//        if (canPickup && Input.GetKeyDown(KeyCode.E))
//        {
//            if (currentItem != null && currentItem.transform.name == "sawedoffshotgun")
//                StartCoroutine(DropItemRoutine(armAnimator, gunAnimator, "ItemDropLayer", "SawedOffDrop", "FPSawedOffDrop"));

//            StartCoroutine(PickupItemRoutine(armAnimator));
//        }

//        if (currentItem != null && Input.GetKeyDown(KeyCode.Q))
//        {   
//            if (currentItemID == 0 && !sawedOffLogic.isReloading)
//            {
//                StartCoroutine(DropItemRoutine(armAnimator, gunAnimator, "ItemDropLayer", "SawedOffDrop", "FPSawedOffDrop"));
//            }
            
//        }
//    }

//    private void CheckItems()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickupDist))
//        {
//           /* Debug.Log("Raycast hit: " + hit.transform.name);*/
//            if (hit.transform.tag == "Item")
//            {
//                Debug.Log("Item can be picked up.");
//                canPickup = true;
//                item = hit.transform.gameObject;
                
//            }
//            else
//            {
//                canPickup=false;
//            }
//        }
//    }

//    private IEnumerator PickupItemRoutine(Animator armAnimator)
//    {
//        currentItem = item;
//        currentItemName = currentItem.transform.name;
//        Debug.Log($"Pickup item {currentItemName}");
//        if (currentItemName == "sawedoffshotgun")
//        {
//            currentItemID = 0;
//            Debug.Log($"PickupItemRoutine called, current Item ID is {currentItemID}");
//            Debug.Log($"Picked up {currentItemName}");

//        }
//        else
//        {
//            currentItemID = 9999;
//        }

//        if (currentItemID != 9999)
//        {
//            armAnimator.SetTrigger(PickupTrigger);

//            while (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
//            {
//                yield return null;
//            }

//            currentItem.SetActive(false); // Hide the item on the ground
//            SetLayers(currentItem, "Items");
//            currentItem.transform.position = equipPos.position;
//            currentItem.transform.parent = equipPos;

//            pickedUp = true;

//            EmptyPickupEventHandlers();
//            yield return null;
//        }
        
        


//    }


//    private IEnumerator DropItemRoutine(Animator armAnimator, Animator itemAnimator, string layer, string itemAnim, string armAnim)
//    {
//        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, "ItemDropLayer", "FPSawedOffDrop"));
//        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(itemAnimator, "ItemDropLayer", "SawedOffDrop"));
        
//        //Wait before deactivating the item
//        AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(armAnimator.GetLayerIndex("ItemDropLayer"));
//        float animationLength = stateInfo.length;
//        yield return new WaitForSeconds(animationLength);
        
//        //Reset the animated item's animator and the animators of its children to ensure were in the correct state when the item is picked up again, then disable the item.
//        if (itemAnimator.gameObject.activeSelf)
//        {
//            animationUtils.ResetAnimator(itemAnimator);
//            foreach (Animator childAnimator in itemAnimator.GetComponentsInChildren<Animator>())
//            {
//                animationUtils.ResetAnimator(childAnimator);
//            }
//            itemAnimator.gameObject.SetActive(false);
//        }

//        //Reinstantiate the item we picked up off the ground
//        currentItem.transform.parent = null;
//        currentItem.SetActive(true);

//        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.isKinematic = false;
//            rb.AddForce(playerCam.transform.forward * throwForce, ForceMode.Impulse);
//        }
//        SetLayers(currentItem, "GroundedItems");

//        currentItem = null;
//        pickedUp = false;

//        //THIS IS UHHHH FUCKED...
//        armAnimator.gameObject.SetActive(false);
//        armAnimator.gameObject.SetActive(true);

//        Debug.Log("Dropped item");
        
//    }

//    private void SetLayers(GameObject obj, string layerName)
//    {
//        obj.layer = LayerMask.NameToLayer(layerName);
//        foreach (Transform child in obj.transform)
//        {
//            SetLayers(child.gameObject, layerName);
//        }
//    }

//    public IEnumerator OnItemPickupComplete(Animator armAnimator, Animator itemAnimator, string pickupLayer, string pickupAnimation, string movementLayer, string movementAnimation)
//    {
//        // Enable the item gameobject and animator, also starts the entry animation so remember to make the item pickup animation the entry for the base layer, thus no need to reference it in the Pickup Event Handler
//        itemAnimator.gameObject.SetActive(true);
        
//        // Start the pickup animation on the armAnimator and wait for it to finish
//        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, pickupLayer, pickupAnimation));

//        // Wait for the pickup animation to finish
//        AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(armAnimator.GetLayerIndex(pickupLayer));
//        float animationLength = stateInfo.length;
//        yield return new WaitForSeconds(animationLength);

//        // Start the movement animations on both the armAnimator and gunAnimator
//        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, movementLayer, movementAnimation));
//        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(itemAnimator, movementLayer, movementAnimation));

//        Debug.Log($"Animation event triggered: {pickupAnimation} equipped!");
//    }

//    /// <summary>
//    /// Function is called after the arm rig has completed the initial empty pickup animation
//    /// Handles specific animations and layers based on the current Item ID.
//    /// </summary>
//    public void EmptyPickupEventHandlers()
//    {
//        currentItemName = currentItem.transform.name;
//        Debug.Log($"EmptyPickupEventHandler called. item Name = {currentItemName}");

//        //Fetch item data by the Item ID which is set based on the gameobject name in the PickupItemRoutine method.
//        itemData = ItemManager.GetItemData(currentItemID);
//        Debug.Log($"Current Item ID {currentItemID}");
//        if (itemData == null)
//        {
//            Debug.LogWarning($"Failed to get ItemData for ID {currentItemID}. The item data is null.");
//            return;
//        }
//        StartCoroutine(OnItemPickupComplete(
//            armAnimator,
//            gunAnimator,
//            itemData.pickupLayer,
//            itemData.pickupAnimation,
//            itemData.movementLayer,
//            itemData.movementAnimation
//            ));

//    }


/*
}*/
    
