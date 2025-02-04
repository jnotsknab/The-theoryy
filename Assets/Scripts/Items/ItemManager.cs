
using System.Collections.Generic;

public class ItemManager
{
    private static Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

    public static void InitializeItems()
    {
        itemDictionary.TryAdd(0, new ItemData(1, "SawedOff", "ItemPickupLayer", "FPSawedOffPickup", "ItemMovementLayer", "SawedOffMovement"));
    }

    public static ItemData GetItemData(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out ItemData itemData)) return itemData;
        return null;
    }
}
