
public class ItemData
{
    public int itemID;
    public string itemName;
    public string pickupLayer;
    public string pickupAnimation;
    public string movementLayer;
    public string movementAnimation;

    public ItemData(int itemID, string itemName, string pickupLayer, string pickupAnimation, string movementLayer, string movementAnimation)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.pickupLayer = pickupLayer;
        this.pickupAnimation = pickupAnimation;
        this.movementLayer = movementLayer;
        this.movementAnimation = movementAnimation;
    }
}
