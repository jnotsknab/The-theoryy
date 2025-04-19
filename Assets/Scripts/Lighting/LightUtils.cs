using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class to change behavior of lights
public class LightUtils : MonoBehaviour
{
    
    public void FlickerLight(Light lightComponent, float flickerDelay, float minIntensity, float maxIntensity)
    {
        StartCoroutine(FlickerRoutine(lightComponent, flickerDelay, minIntensity, maxIntensity));    
    }

    private IEnumerator FlickerRoutine(Light lightComponent, float flickerDelay, float minIntensity, float maxIntensity)
    {
        while (true)
        {
            lightComponent.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerDelay);
        }
    }

    public void PulseLight(Light lightComponent, float baseIntensity, float flashIntensity, float flashDuration, float fadeDuration, bool delayBeforeLoop = false)
    {
        StartCoroutine(PulseRoutine(lightComponent, baseIntensity, flashIntensity, flashDuration, fadeDuration, delayBeforeLoop));
    }

    private IEnumerator PulseRoutine(Light lightComponent, float baseIntensity, float flashIntensity, float flashDuration, float fadeDuration, bool delayBeforeLoop = false)
    {   
        //Add another parameter so we dont hardcode.
        if (delayBeforeLoop) yield return new WaitForSeconds(0.25f);

        while (true)
        {
            lightComponent.intensity = flashIntensity;
            yield return new WaitForSeconds(flashDuration);

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                lightComponent.intensity = Mathf.Lerp(flashIntensity, baseIntensity, elapsed / fadeDuration);
                yield return null;
            }

            lightComponent.intensity = baseIntensity;
        }
        
    }
}
