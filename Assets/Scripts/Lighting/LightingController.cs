using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


// Start is called before the first frame update
//I want this class to control lighting based events i may need in the future.
//For now i just need a function that toggles the spawn hallway lights on and off based on the players distance to them
//We can track the range on the player by referencing the position of the playerobj and then extending a range in all directions.
public class LightingController
{
    MiscUtils miscUtils = new MiscUtils();
    AudioHandler audioHandler = new AudioHandler();
    public GameObject[] lightObjs;
    public Vector3[] lightPositions;
    public float playerDetectionRadius = 10f;
    private HashSet<Vector3> activeLights = new HashSet<Vector3>();
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<Light, float> flickerTimers = new Dictionary<Light, float>();
    private Dictionary<Light, float> flickerIntervals = new Dictionary<Light, float>();


    public class CoroutineHelper : MonoBehaviour
    {
        private CoroutineHelper coroutineRunnerWorker;
        private CoroutineHelper coroutineRunner
        {
            get
            {
                Debug.Log("GET RUN");
                if (coroutineRunnerWorker != null)
                {
                    return coroutineRunnerWorker;
                }
                return InitCoroutineRunner();
            }
            set { }
        }

        private CoroutineHelper InitCoroutineRunner()
        {
            GameObject instance = new GameObject();
            instance.isStatic = true;
            coroutineRunnerWorker = instance.AddComponent<CoroutineHelper>();
            return coroutineRunnerWorker;
        }
    }

    public AudioClip GetAudioClip(string clipName)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }

        Debug.LogWarning($"Audio clip '{clipName}' not found.");
        return null;
    }

    public GameObject[] GetLightObjects(string tag)
    {
        lightObjs = miscUtils.GetTaggedObjects(tag);

        

        return lightObjs;
    }


    public Vector3[] GetLightPositions(GameObject[] lightObjects)
    {
        if (lightObjects == null || lightObjects.Length == 0)
            return Array.Empty<Vector3>();

        lightPositions = lightObjects.Select(obj => obj.transform.position).ToArray();
        return lightPositions;
    }

    /// <summary>
    /// Enables the range around the player object to toggle the hallway lights in the spawn hallway.
    /// </summary>
    public void EnableSpawnLightRange(Vector3[] lightPositions, GameObject playerObj)
    {
        Vector3 playerPos = miscUtils.GetPlayerPosition(playerObj);
        Debug.Log($"Player pos: {playerPos}");
        

        // Iterate over all light positions
        foreach (Vector3 lightPos in lightPositions)
        {
            float distance = Vector3.Distance(playerPos, lightPos);

            // If the light is within range and isn't already activated, turn it on
            if (distance <= playerDetectionRadius && !activeLights.Contains(lightPos))
            {
                ToggleLight(lightPos, true);  // Turn on light
                activeLights.Add(lightPos);  // Mark the light as active

            }
            // If the light is out of range and is active, turn it off
            else if (distance > playerDetectionRadius && activeLights.Contains(lightPos))
            {
                ToggleLight(lightPos, false); // Turn off light
                activeLights.Remove(lightPos); // Mark the light as inactive
                
            }
            else if (distance <= playerDetectionRadius && activeLights.Contains(lightPos))
            {
                LightFlicker(lightPos, 0.5f, 2f, 0f, 1.1f, Time.deltaTime);
            }
            
        }
    }

    public void ToggleLight(Vector3 lightPos, bool turnOn)
    {
        // Find the light GameObject at the given position
        GameObject light = lightObjs.FirstOrDefault(obj => Vector3.Distance(obj.transform.position, lightPos) < 0.1f);

        if (light != null)
        {
            // Toggle the light's active state based on 'turnOn' parameter
            light.SetActive(turnOn);


            //// Get the AudioSource component attached to the light
            //AudioSource audioSource = light.GetComponent<AudioSource>();
            //AudioClip clip1 = GetAudioClip("SpawnLightSFXNew");
            //AudioClip clip2 = GetAudioClip("SpawnLampFlicker");
            //audioSource.clip = player.spawnLightStartup;
            //CoroutineRunner.Instance.RunCoroutine(PlayDelayedClip(audioSource, player.spawnLightHum, 0.25f));

            audioHandler.PlaySources(light, true, true, 0.85f, 1.2f, 0.5f, 1f);
            

            if (turnOn)
            {
                Debug.Log("Light was turned on");
            }
            else
            {
                Debug.Log("Light was turned off");
                audioHandler.FadeSources(light, false, 1f);
            }
        }
        else
        {
            Debug.LogWarning("Light at position " + lightPos + " not found.");
        }
    }

    //public void LightFlicker(Vector3 lightPos, float minIntensity, float maxIntensity, float flickerSpeed)
    //{
    //    GameObject lightObj = lightObjs.FirstOrDefault(obj => Vector3.Distance(obj.transform.position, lightPos) < 0.1f);
    //    if (lightObj != null)
    //    {
    //        Light lightComponent = lightObj.GetComponent<Light>();
    //        if (lightComponent != null && !flickerTimers.ContainsKey(lightComponent))
    //        {
    //            flickerTimers[lightComponent] = 0f; // Initialize flicker timer
    //        }
    //    }
    //}

    public void LightFlicker(Vector3 lightPos, float minInterval, float maxInterval, float minIntensity, float maxIntensity, float deltaTime)
    {
        GameObject lightObj = lightObjs.FirstOrDefault(obj => Vector3.SqrMagnitude(obj.transform.position - lightPos) < 0.05f);
        if (lightObj != null)
        {
            Light lightComponent = lightObj.GetComponent<Light>();
            if (lightComponent != null && !flickerTimers.ContainsKey(lightComponent))
            {
                flickerTimers[lightComponent] = 0f;
                flickerIntervals[lightComponent] = UnityEngine.Random.Range(minInterval, maxInterval);
            }
        }

        // Copy keys to avoid modifying while iterating
        var lights = flickerTimers.Keys.ToList();
        foreach (var light in lights)
        {
            flickerTimers[light] += deltaTime;

            if (flickerTimers[light] >= flickerIntervals[light]) // Only update when timer exceeds interval
            {
                light.intensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
                Debug.Log($"Light intensity updated to {light.intensity}");

                flickerTimers[light] = 0f; // Reset timer
                flickerIntervals[light] = UnityEngine.Random.Range(minInterval, maxInterval); // Assign new interval
            }
        }
    }





}
