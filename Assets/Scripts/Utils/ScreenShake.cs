using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    /// <summary>
    /// Shakes the screen (camera) based on a certain duration and magnitude.
    /// </summary>
    /// <param name="duration">How long the screen shake lasts</param>
    /// <param name="magnitude">How intense the shake is</param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 origPos = transform.localPosition; // Store the original position
        float elapsed = 0.0f;


        while (elapsed < duration)
        {
            float currentMagnitude = magnitude * Mathf.Pow(1 - (elapsed / duration), 2); // Easier to make more "rough" or "smooth"

            float frequencyMultiplier = Mathf.Lerp(10f, 1f, elapsed / duration);  // Starts rough, smooths over time

            float x = Mathf.PerlinNoise(elapsed * frequencyMultiplier, 0f) * 2f - 1f; // Noise for horizontal shake
            float y = Mathf.PerlinNoise(0f, elapsed * frequencyMultiplier) * 2f - 1f; // Noise for vertical shake

            transform.localPosition = origPos + new Vector3(x, y, origPos.z) * currentMagnitude;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origPos;
    }
}
