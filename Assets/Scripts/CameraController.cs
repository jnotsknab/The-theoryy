using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera orthoCamera; // Reference to your orthographic camera
    public RenderTexture renderTexture; // Your render texture (in-game screen)
    public float desiredHeight; // Desired height for the camera's view

    void Start()
    {
        // Set the render texture as the camera's target
        orthoCamera.targetTexture = renderTexture;

        // Adjust the camera's aspect ratio and size
        UpdateCameraAspectAndSize();
    }

    void UpdateCameraAspectAndSize()
    {
        // Calculate the aspect ratio based on the render texture's width and height
        float aspectRatio = (float)renderTexture.width / renderTexture.height;

        // Set the camera's aspect ratio
        orthoCamera.aspect = aspectRatio;

        // Adjust the camera's orthographic size based on the desired height
        orthoCamera.orthographicSize = desiredHeight / 2f; // Divide by 2 since orthographicSize controls the half vertical size
    }
}
