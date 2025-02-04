using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils : MonoBehaviour
{   
    /// <summary>
    /// Swaps to a layer and flips weights for all layers
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerName"></param>
    /// <param name="animationName"></param>
    
    public IEnumerator SwapToLayerAndPlayEntryAnimation(Animator animator, string layerName, string animationName)
    {
        int layerIndex = animator.GetLayerIndex(layerName);

        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{layerName}' not found in Animator!");
            yield break;
        }

        // Set layer weight to 1 if its 0, 0 if 1
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, i == layerIndex ? 1f : 0f);
        }

        animator.Play(animationName, layerIndex, 0f);

        while (animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 0.99f)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Blends the weight between from one layer transition to another
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerName"></param>
    /// <param name="targetWeight"></param>
    /// <param name="duration"></param>
    public IEnumerator BlendLayerWeight(Animator animator, string layerName, float targetWeight, float duration)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0f);
        }

        int layerIndex = animator.GetLayerIndex(layerName);
        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{layerName}' not found in Animator!");
            yield break;
        }

        float startWeight = animator.GetLayerWeight(layerIndex);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
            animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        animator.SetLayerWeight(layerIndex, targetWeight);
    }

    /// <summary>
    /// Resets all layers of the animator to a weight of 0.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerName"></param>
    public IEnumerator ResetLayers(Animator animator)
    {

        // Reset all layer weights to 0 first
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0f);
        }
        yield return null;

        
    }

    /// <summary>
    /// Sets target layer weight to 1, and all other layers to 0.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerName"></param>
    public IEnumerator ActivateLayer(Animator animator, string layerName)
    {
        int layerIndex = animator.GetLayerIndex(layerName);

        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{layerName}' not found in Animator!");
            yield return null;
        }

        // Reset all layers to weight 0
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0f);
        }

        // Set the target layer weight to 1 to activate it
        animator.SetLayerWeight(layerIndex, 1f);
    }

    public IEnumerator ActivateLayerLerp(Animator animator, string layerName, float duration)
    {
        int layerIndex = animator.GetLayerIndex(layerName);

        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{layerName}' not found in Animator!");
            yield return null;
        }

        // Reset all layers to weight 0
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0f);
        }

        // Gradually lerp the layer's weight from 0 to 1
        float elapsedTime = 0f;
        float startWeight = animator.GetLayerWeight(layerIndex);
        float targetWeight = 1f;

        while (elapsedTime < duration)
        {
            // Lerp the weight over time
            float newWeight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
            animator.SetLayerWeight(layerIndex, newWeight);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the weight is exactly 1 after the transition
        animator.SetLayerWeight(layerIndex, targetWeight);
    }

    public void ResetAnimator(Animator animator)
    {
        if (animator != null)
        {
            animator.Rebind();  // Reset internal animation state
            animator.Update(0f);  // Apply changes immediately
        }
    }







}
