using System;
using System.Text;
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

    public string RandomString(int length, bool allCaps = false, bool allLowercase = false, bool includeNums = true, bool includeSpecial = false)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Length must be a positive integer.");
        }

        string lower = "abcdefghijklmnopqrstuvwxyz";
        string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string digits = "0123456789";
        string special = "!@#$%^&*()-_=+[]{}|;:,.<>?/";

        StringBuilder characterSet = new StringBuilder();

        if (allCaps) characterSet.Append(upper);
        if (allLowercase) characterSet.Append(lower);
        if (includeNums) characterSet.Append(digits);
        if (includeSpecial) characterSet.Append(special);

        if (characterSet.Length == 0)
        {
            throw new ArgumentException("At least one character set (lowercase, uppercase, numbers, special) must be enabled.");
        }

        System.Random rand = new System.Random();
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            result.Append(characterSet[rand.Next(characterSet.Length)]);
        }

        return result.ToString();
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

    public bool DistCheck(Camera playerCam, GameObject item, int maxDist, int fieldOfView)
    {
        Vector3 directionToItem = item.transform.position - playerCam.transform.position;

        if (directionToItem.sqrMagnitude > maxDist * maxDist)
        {
            return false;
        }

        Vector3 forward = playerCam.transform.forward;
        float dot = Vector3.Dot(forward, directionToItem.normalized);

        float maxDot = Mathf.Cos(Mathf.Deg2Rad * fieldOfView);
        if (dot < maxDot)
        {
            return false;
        }

        return true;
    }
}

