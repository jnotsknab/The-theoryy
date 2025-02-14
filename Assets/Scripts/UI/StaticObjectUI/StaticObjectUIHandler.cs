using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class StaticObjectUIHandler : MonoBehaviour
{
    public Camera playerCam;

    //Static Object component References we may need
    public ComputerScreenHandler computerScreenHandler;
    private MiscUtils miscUtils = new MiscUtils();


    public GameObject[] staticObjs;

    //Static Obj UI Dictionary
    private Dictionary<GameObject, GameObject> staticObjUIDict = new Dictionary<GameObject, GameObject>();

    [SerializeField] private int maxDist = 5;
    private void Awake() => Setup();


    void Update()
    {
        if (staticObjs != null)
        {
            CheckStaticObjUI();
        }
    }

    private void Setup()
    {
        foreach (var obj in staticObjs)
        {
            if (obj != null)
            {
                GameObject staticObjUI = ElementManager.GetElement(obj.name + "UI");
                staticObjUIDict[obj] = staticObjUI;
            }
        }
    }

    private void CheckStaticObjUI()
    {
        foreach (var obj in staticObjs)
        {
            if (obj != null && staticObjUIDict.ContainsKey(obj))
            {
                GameObject staticObjUI = ElementManager.GetElement(obj.name + "UI");
                if (staticObjUI == null)
                {
                    Debug.LogError("UI element not found for static Object: " + obj.name);
                    continue;
                }

                if (!computerScreenHandler.isTurnedOn && miscUtils.DistCheck(playerCam, obj, maxDist, 70))
                {
                    ElementManager.EnqueueElement(staticObjUI);
                }
                else
                {
                    ElementManager.DequeueElement(staticObjUI);
                }
            }
        }
    }
    
}


