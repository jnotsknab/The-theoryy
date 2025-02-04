using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string sceneToLoad;  // Name of the scene to load
    public GameObject loadingScreen; // Optional: Assign a loading screen GameObject in the inspector
    public UnityEngine.UI.Slider progressBar; // Optional: Assign a progress bar slider in the inspector

    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Start the asynchronous scene loading process
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        // Optionally display a loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Prevent the new scene from activating immediately
        operation.allowSceneActivation = false;

        // Monitor the loading progress
        while (!operation.isDone)
        {
            // Update the progress bar (optional)
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f); // Normalized to 0-1 range
            }

            // Check if the scene is fully loaded
            if (operation.progress >= 0.9f)
            {
                // Activate the new scene
                operation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }
    }
}
