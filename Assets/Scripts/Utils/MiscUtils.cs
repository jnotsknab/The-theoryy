using System;
using System.Text;
using UnityEngine;

public class MiscUtils
{

    //This shit is horrible for performance apparently should used findall.
    public GameObject[] GetTaggedObjects(string tag)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        GameObject[] taggedObjects = System.Array.FindAll(allObjects, obj => obj.CompareTag(tag));

        if (taggedObjects.Length == 0)
        {
            Debug.LogWarning($"No objects with the tag {tag} were found.");
        }

        foreach (GameObject obj in taggedObjects)
        {
            Debug.Log($"Found Object: {obj.name}");
        }

        return taggedObjects;
    }

    /// <summary>
    /// Returns a random string based on various parameters.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="allCaps"></param>
    /// <param name="allLowercase"></param>
    /// <param name="includeNums"></param>
    /// <param name="includeSpecial"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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
    /// <summary>
    /// Returns the players current position.
    /// </summary>
    /// <param name="playerObj"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Checks the distance between the player and an object.
    /// Returns true if the object is within the maxDist and fieldOfView of the camera passed in.
    /// </summary>
    /// <param name="playerCam"></param>
    /// <param name="item"></param>
    /// <param name="maxDist"></param>
    /// <param name="fieldOfView"></param>
    /// <returns></returns>
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

