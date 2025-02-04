using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{   
    /// <summary>
    /// Depricated, use the monobehavior class within 
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        return new GameObject("CoroutineHelper").AddComponent<CoroutineHelper>().StartCoroutine(coroutine);
    }
}
