
using System.Collections.Generic;

public class ItemManager
{
    private static Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

    public static void InitializeItems()
    {   
        //Items have their entry animation set as the pickup anim that aligns with the FP/arm pickup animation, which is why we only have a parameter for the arm rig pickup animation.
        itemDictionary.TryAdd(0, new ItemData(1, "SawedOff", "ItemPickupLayer", "FPSawedOffPickup", "ItemMovementLayer", "SawedOffMovement"));
    }

    public static ItemData GetItemData(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out ItemData itemData)) return itemData;
        return null;
    }
}
