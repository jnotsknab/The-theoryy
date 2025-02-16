using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all Canvases in a scene
/// </summary>
public class CanvasManager : MonoBehaviour
{
    private static CanvasManager singletonInstance;

    [SerializeField] private Canvas[] startingCanvases;
    [SerializeField] private Canvas[] canvases;

    private Dictionary<string, Canvas> canvasDict = new Dictionary<string, Canvas>();

    private List<Canvas> activeCanvases;

    private void Awake()
    {
        singletonInstance = this;
        canvasDict = new Dictionary<string, Canvas>();
        foreach (var canvas in canvases)
        {
            if (canvas != null) canvasDict[canvas.name] = canvas;
        }
    }

    public static Canvas GetCanvas(string name)
    {
        if (singletonInstance.canvasDict.TryGetValue(name, out Canvas canvas))
        {
            return canvas;
        }
        return null;
    }

    public static void Show(string name)
    {
        if (singletonInstance.canvasDict.TryGetValue(name, out Canvas canvas))
        {
            canvas.enabled = true;
            singletonInstance.AddToActive(canvas);
        }
    }

    public static void Hide(string name)
    {
        if (singletonInstance.canvasDict.TryGetValue(name, out Canvas canvas))
        {
            canvas.enabled = false;
            singletonInstance.RemoveFromActive(canvas);
        }
    }

    private void AddToActive(Canvas canvas)
    {
        activeCanvases.Add(canvas);
    }

    private void RemoveFromActive(Canvas canvas)
    {
        activeCanvases.Remove(canvas);
    }



}
