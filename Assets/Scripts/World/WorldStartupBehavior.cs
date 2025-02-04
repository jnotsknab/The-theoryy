using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStartupBehavior : MonoBehaviour
{
    public GameObject globalLighting;
    [SerializeField]
    private int framesToWait = 1000;
    private int frameCount = 0;

    void Start()
    {
        globalLighting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        while (frameCount < framesToWait)
        {
            globalLighting.SetActive(false);
            frameCount++;
        }
        globalLighting.SetActive(true);

    }
}
