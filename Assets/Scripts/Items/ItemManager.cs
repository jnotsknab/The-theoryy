
using System.Collections.Generic;

public class ItemManager
{
    private static Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

    public static void InitializeItems()
    {
        //Items have their entry animation set as the pickup anim that aligns with the FP/arm pickup animation, which is why we only have a parameter for the arm rig pickup animation.
        itemDictionary.TryAdd(0, new ItemData(0, "Shotgun", "ItemPickupLayer", "FPSawedOffPickup", "ItemMovementLayer", "SawedOffMovement"));
        itemDictionary.TryAdd(1, new ItemData(1, "TimefluxDevice", "ItemPickupLayer", "FPTimefluxPickup", "ItemMovementLayer", "TimefluxMovement"));
    }

    public static ItemData GetItemData(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out ItemData itemData)) return itemData;
        return null;
    }

    public static int? GetItemID(string itemName)
    {
        foreach (var item in itemDictionary)
        {
            if (item.Value.itemName == itemName)
                return item.Key;
        }
        return null;
    }
}
