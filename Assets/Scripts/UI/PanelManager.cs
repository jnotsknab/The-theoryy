using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all UI Panels such as panels that are children of Canvases.
/// </summary>
public class PanelManager : MonoBehaviour
{
    private static PanelManager singletonInstance;

    [SerializeField] GameObject[] startingPanels;
    [SerializeField] GameObject[] panels;

    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();

    private List<GameObject> activePanels = new List<GameObject>();

    private void Awake()
    {
        singletonInstance = this;
        panelDict = new Dictionary<string, GameObject>();
        foreach (var panel in panels)
        {
            if (panel != null) panelDict[panel.name] = panel;
        }
    }

    public static GameObject GetPanel(string name)
    {
        if (singletonInstance.panelDict.TryGetValue(name, out GameObject panel))
            return panel;

        return null;
    }

    public static void Show(string name)
    {
        if (singletonInstance.panelDict.TryGetValue(name, out GameObject panel))
        {
            panel.SetActive(true);
            singletonInstance.AddToActive(panel);
        }
    }

    public static void Hide(string name)
    {
        if (singletonInstance.panelDict.TryGetValue(name, out GameObject panel))
        {
            panel.SetActive(false);
            singletonInstance.RemoveFromActive(panel);
        }
    }

    private void AddToActive(GameObject panel)
    {
        activePanels.Add(panel);
    }

    private void RemoveFromActive(GameObject panel)
    {
        activePanels.Remove(panel);
    }


}
