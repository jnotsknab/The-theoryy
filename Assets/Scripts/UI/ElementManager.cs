using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all UI elements such as elements that are children of UI panels.
/// </summary>
public class ElementManager : MonoBehaviour
{
    public static ElementManager singletonInstance;

    [SerializeField] private GameObject[] startingElements;
    [SerializeField] private GameObject[] elements;
    [SerializeField] private Queue<GameObject> elementQueue = new Queue<GameObject>();

    private Dictionary<string, GameObject> elementDict;
    private List<GameObject> activeElements = new List<GameObject>();

    private void Awake()
    {   
        singletonInstance = this;
        elementDict = new Dictionary<string, GameObject>();

        //Elements are assigned in the inspector
        if (elements != null)
        {   
            //Populate the dict with elements.
            foreach (var element in elements)
            {   
                if (element != null)
                    elementDict[element.name] = element;
                    Debug.Log($"Added {element.name} to the dictionary.");
            }
        }
    }

    //May need to create a method to get all elements but for now this should do.
    public static GameObject GetElement(string name)
    {
        if (singletonInstance == null) return null;

        if (singletonInstance.elementDict.TryGetValue(name, out GameObject element))
        {
            return element;
        }
        return null;
    }

    private static void Show(string name)
    {
        if (singletonInstance == null) return;

        if (singletonInstance.elementDict.TryGetValue(name, out GameObject element))
        {
            if (!element.activeSelf) // Prevent redundant SetActive calls
            {
                element.SetActive(true);
                singletonInstance.AddToActive(element);
            }
        }
    }

    private static void Hide(string name)
    {
        if (singletonInstance == null) return;

        if (singletonInstance.elementDict.TryGetValue(name, out GameObject element))
        {
            if (element.activeSelf) // Prevent redundant SetActive calls
            {
                element.SetActive(false);
                singletonInstance.RemoveFromActive(element);
            }
        }
    }

    private void AddToActive(GameObject element)
    {
        if (!activeElements.Contains(element))
        {
            activeElements.Add(element);
        }
    }

    private void RemoveFromActive(GameObject element)
    {
        activeElements.Remove(element);
    }

    /// <summary>
    /// Adds the given UI element to the UI Queue and shows the head UI element of that queue. 
    /// </summary>
    /// <param name="element"></param>
    public static void EnqueueElement(GameObject element)
    {
        if (!singletonInstance.elementQueue.Contains(element))
        {
            singletonInstance.elementQueue.Enqueue(element);
            singletonInstance.ShowHeadElement(); // Only update when queue changes
        }
    }

    /// <summary>
    /// Removes the given UI element from the UI Queue and shows the head UI element of that queue.
    /// </summary>
    /// <param name="element"></param>
    public static void DequeueElement(GameObject element)
    {
        if (singletonInstance.elementQueue.Count == 0) return;

        if (singletonInstance.elementQueue.Peek() == element)
        {
            Hide(element.name);
            singletonInstance.elementQueue.Dequeue();
            singletonInstance.ShowHeadElement();
        }
     
    }


    /// <summary>
    /// Shows only the element at the front of the queue and hides all others.
    /// </summary>
    private void ShowHeadElement()
    {
        if (singletonInstance == null || singletonInstance.elementQueue.Count == 0)
            return;

        GameObject headElement = singletonInstance.elementQueue.Peek();

        //Only enter if the head element is inactive.
        if (!headElement.activeSelf)
        {
            // Hide all actve elements except the head
            foreach (var element in singletonInstance.activeElements)
            {
                if (element != headElement)
                {
                    Hide(element.name);
                }
            }
            Show(headElement.name);
        }
    }
}
