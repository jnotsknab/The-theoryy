using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscUtils
{


    public GameObject[] GetTaggedObjects(string tag)
    {
        // Find all objects, including inactive ones, by their type (GameObject).
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // Filter the objects by the specified tag
        GameObject[] taggedObjects = System.Array.FindAll(allObjects, obj => obj.CompareTag(tag));

        // If no objects are found, log a warning.
        if (taggedObjects.Length == 0)
        {
            Debug.LogWarning($"No objects with the tag {tag} were found.");
        }

        // Print the names of the found objects.
        foreach (GameObject obj in taggedObjects)
        {
            Debug.Log($"Found Object: {obj.name}");
        }

        return taggedObjects;
    }





    public Vector3 GetPlayerPosition(GameObject playerObj)
    {
        
        if (playerObj != null)
        {
            return playerObj.transform.position;
        }
        else
        {
            Debug.LogError("Player object not found!");
            return Vector3.zero;
        }
    }
}
