
public class ItemData
{
    public int itemID;
    public string itemName;
    public string pickupLayer;
    public string pickupAnimation;
    public string movementLayer;
    public string movementAnimation;
    public string armDropAnimation;
    public string itemDropAnimation;

    public bool inHotbar;

    public ItemData(int itemID, string itemName, string pickupLayer, string pickupAnimation, string movementLayer, string movementAnimation, string armDropAnim, string itemDropAnim, bool inHotbar = false)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.pickupLayer = pickupLayer;
        this.pickupAnimation = pickupAnimation;
        this.movementLayer = movementLayer;
        this.movementAnimation = movementAnimation;
        this.armDropAnimation = armDropAnim;
        this.itemDropAnimation = itemDropAnim;
        this.inHotbar = inHotbar;

    }
}
