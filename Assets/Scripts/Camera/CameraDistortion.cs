using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraDistortion : MonoBehaviour
{
    public Volume postProcessingVolume; // Assign your volume here in the Inspector
    private LensDistortion lensDistortion;
    private float originalDistortion;
    private float targetDistortion;
    private float transitionDuration;
    private float elapsedTime;

    void Start()
    {
        // Ensure the volume has a profile
        if (postProcessingVolume.profile != null)
        {
            // Get the LensDistortion effect
            postProcessingVolume.profile.TryGet<LensDistortion>(out lensDistortion);

            if (lensDistortion != null)
            {
                // Store the original distortion value
                originalDistortion = lensDistortion.intensity.value;
            }
        }
    }

    void Update()
    {
        // If LensDistortion exists and the rewind is happening
        if (lensDistortion != null && elapsedTime < transitionDuration)
        {
            // Calculate the lerp factor (0-1) for the transition
            float lerpFactor = Mathf.Clamp01(elapsedTime / (transitionDuration / 2));

            // Transition towards the target distortion for the first half of the duration
            if (elapsedTime < transitionDuration / 2)
            {
                lensDistortion.intensity.value = Mathf.Lerp(originalDistortion, targetDistortion, lerpFactor);
            }
            // Transition back to the original distortion for the second half
            else
            {
                lensDistortion.intensity.value = Mathf.Lerp(targetDistortion, originalDistortion, lerpFactor);
            }

            // Update elapsed time
            elapsedTime += Time.deltaTime;
        }
    }

    // Function to start the distortion transition
    public void StartDistortionTransition(float duration, float target)
    {
        originalDistortion = lensDistortion.intensity.value; // Save the current value
        targetDistortion = target; // Set the target distortion
        transitionDuration = duration; // Set the transition duration
        elapsedTime = 0f; // Reset the timer
    }
}
