using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//When switching an item the selected animation should play for the selected slot, and we should tag it as selected and all others as deselected,
//when deselecting we should go back to the orignal idle for that slot, if the player has no items and picks one up the animations for that item will play, and
// the itemID will be populated into the first slot, afterwards, if the player picks up an item while holding onto their current item, nothing will happen,
//I.E. no animations except the grounded item getting disabled, and that items ID will be populated into the first avaliable slot, if the player swaps to an empty slot,
//We will reset the item state and the players enable to have the empty arm blend tree, then if the player picks up an item while in an empty slot the item will get populated to the first avaliable slot.

//All thats left to do is fix the scrolling and disable bugs, and then asssign items to slots when picked up and populate those slots, then when switching slots activate the respective item
public class HotbarSlot : MonoBehaviour
{
    public bool selected;
    public List<GameObject> slotList = new List<GameObject>();
    public List<Animator> potentialItemAnimators = new List<Animator>();
    public List<Sprite> itemSprites = new List<Sprite>();
    public int selectedIndex = 0; // Currently selected hotbar slot
    private Dictionary<int, (Animator animator, Animator itemAnimator, bool populated, int itemInSlotID)> slotData = new Dictionary<int, (Animator, Animator, bool, int)>();

    public AudioSource hudAudio;
    private NewItemPickupHandler pickupHandler;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerSelf");
        pickupHandler = playerObj.GetComponent<NewItemPickupHandler>();
        // Initialize slots and store Animator references
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i] != null)
            {
                Animator anim = slotList[i].GetComponent<Animator>();
                slotData[i] = (anim, null, false, 9999);
            }
        }

        //// Select the first slot by default if available
        //if (slotList.Count > 0)
        //{
        //    SelectSlot(1);
        //    Debug.Log("Select slot invoked in start");
        //}


    }

    //Remove later just for testing, using new input system
    //Switching slots works, when entering computer, ie hud view gets disbaled, then slots have some issues that need to be fixed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    }



    // Input handler for scrolling through slots
    //Scrolling doesnt work fix ts
    public void OnScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        Debug.Log($"Scrolling: {scrollValue}");

        if (Mathf.Abs(scrollValue) > 0.1f) // Ensures significant scroll input
        {
            int newIndex = (selectedIndex + (int)Mathf.Sign(scrollValue) + slotList.Count) % slotList.Count;
            Debug.Log($"New index is : {newIndex}");
            SelectSlot(newIndex);
        }
    }


    private void ScrollHotbar(float direction)
    {
        if (slotList.Count == 0) return;

        selectedIndex = (selectedIndex + (int)direction + slotList.Count) % slotList.Count;
        UpdateHotbarSelection();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slotList.Count) return;

        Debug.Log($"Selecting slot: {index}");


        DeselectAllSlots();

        if (slotData.TryGetValue(index, out var slotInfo))
        {
            slotInfo.animator.SetBool("Selected", true);
            slotData[index] = (slotInfo.animator, slotInfo.itemAnimator, slotInfo.populated, slotInfo.itemInSlotID);
            if (slotInfo.populated)
            {

                Debug.Log($"Hotbar Switch Statement enterered with item ID : {slotInfo.itemInSlotID}");
                switch (slotInfo.itemInSlotID)
                {   
                    case 0:
                        slotInfo.itemAnimator = potentialItemAnimators[0];
                        pickupHandler.SwitchItem(0, slotInfo.itemAnimator);
                        break;
                    case 1:
                        slotInfo.itemAnimator = potentialItemAnimators[1];
                        pickupHandler.SwitchItem(1, slotInfo.itemAnimator);
                        break;
                    case 2:
                        slotInfo.itemAnimator = potentialItemAnimators[2];
                        pickupHandler.SwitchItem(2, slotInfo.itemAnimator);
                        break;
                }
            }
            Debug.Log($"Slot {index} selected and animation set.");
        }

        hudAudio.volume = 0.1f;
        hudAudio.Play();
        selectedIndex = index;
    }

    private void OnDisable()
    {
        DeselectAllSlots();
        //if (selectedIndex >= 0 && selectedIndex < slotList.Count) SelectSlot(selectedIndex);
    }

    private void OnEnable()
    {
        DeselectAllSlots(); // Reset all slot animators fully

        //// Ensure the last selected slot is correctly highlighted
        //if (selectedIndex >= 0 && selectedIndex < slotList.Count)
        //{
        //    SelectSlot(selectedIndex);
        //}
    }

    public void DeselectAllSlots()
    {
        foreach (var key in new List<int>(slotData.Keys)) // Iterate over a copy of keys
        {
            if (slotData.TryGetValue(key, out var slotInfo))
            {
                slotInfo.animator.ResetTrigger("Selected"); // Reset any triggers
                slotInfo.animator.ResetTrigger("Idle");
                slotInfo.animator.SetBool("Selected", false);
                slotInfo.animator.Play("Idle", 0, 0f); // Force play idle state at start

                Image slotImage = slotList[key].GetComponent<Image>();
                if (slotImage != null)
                {
                    slotImage.color = new Color32(140, 100, 86, 30);
                }

                // Explicitly reassign updated animator state
                slotData[key] = (slotInfo.animator, slotInfo.itemAnimator, slotInfo.populated, slotInfo.itemInSlotID);
            }
        }
    }



    public void SetSlotPopulated(int index, bool isPopulated, int itemInSlotID)
    {
        if (!slotData.ContainsKey(index)) return;

        var slotInfo = slotData[index];

        if (slotInfo.populated != isPopulated)
        {
            int newItemID = isPopulated ? itemInSlotID : 9999;
            slotData[index] = (slotInfo.animator, slotInfo.itemAnimator, isPopulated, newItemID);

            // Get the first child of the slot GameObject
            if (slotList[index].transform.childCount > 0) // Ensure there is at least one child
            {
                Transform iconHolderTransform = slotList[index].transform.GetChild(0); // Get the first child
                Image itemIcon = iconHolderTransform.GetComponent<Image>();

                if (itemIcon != null)
                {
                    // Set the image based on the item ID
                    itemIcon.sprite = GetItemSprite(newItemID);
                    itemIcon.enabled = isPopulated; // Enable icon if populated, disable if not
                }
            }
        }
    }
    

    private Sprite GetItemSprite(int itemID)
    {
        switch (itemID)
        {
            case 0: return itemSprites[0];

            case 1: return itemSprites[1];

        }
        return null;
    }

    public void SelectFirstEmptySlot()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotData.TryGetValue(i, out var slotInfo) && !slotInfo.populated)
            {
                SelectSlot(i);
                return;
            }
        }
    }


    public bool IsSlotPopulated(int index)
    {
        return slotData.TryGetValue(index, out var slotInfo) && slotInfo.populated;
    }

    private void UpdateHotbarSelection()
    {
        SelectSlot(selectedIndex);
    }
}
