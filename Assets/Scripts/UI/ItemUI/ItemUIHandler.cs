using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIHandler : MonoBehaviour
{
    public Camera playerCam;
    //public ItemPickupHandler itemPickupHandler;
    private MiscUtils miscUtils = new MiscUtils();

    //Assign all items in game in inspector, there wont be too many maybe 15-20 max.
    public GameObject[] items;

    //UI List
    private Dictionary<GameObject, GameObject> itemUIDict = new Dictionary<GameObject, GameObject>();


    [SerializeField] private int maxDist = 5;

    private void Awake() => Setup();

    private void Update()
    {
        if (items != null)
        {
            CheckItemUI();
        }
    }

    private void Setup()
    {
        foreach (var item in items)
        {
            if (item != null)
            {
                GameObject itemUI = ElementManager.GetElement(item.name + "UI");
                itemUIDict[item] = itemUI;
            }
        }
    }
    private void CheckItemUI()
    {

        foreach (var item in items)
        {   
            if (item != null && itemUIDict.ContainsKey(item))
            {
                GameObject itemUI = ElementManager.GetElement(item.name + "UI");
                if (itemUI == null)
                {
                    Debug.LogError("UI element not found for item: " + item.name);
                    continue;
                }

                if (!NewItemPickupHandler.Instance.pickedUp && miscUtils.DistCheck(playerCam, item, maxDist, 70))
                {
                    ElementManager.EnqueueElement(itemUI);
                }
                else
                {
                    ElementManager.DequeueElement(itemUI);
                }
            }
        }
    }

}
