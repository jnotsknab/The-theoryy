using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class serves as a script component to add onto each instantiated door, and handles the door logic.
/// </summary>
public class DoorInteraction : MonoBehaviour
{
    private Animator doorAnimator;
    private AudioHandler audioHandler = new AudioHandler();
    public GameObject doorUIRef;
    public AudioSource doorAudioSource;
    public AudioClip[] audioClips;
    //Reference to all our doors
    [Header("Door Obj")]
    //public GameObject[] doors;
    public GameObject door;

    //Assign this in inspector as each door will have two colliders one for raycasts and one for contact.
    public Collider doorContactCollider;
    private bool opened = false;
    private TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        if (opened)
        {
            textMeshProUGUI.text = "Close : [E]";
        }
        else
        {
            textMeshProUGUI.text = "Open : [E]";
        }
    }
    private void Awake()
    {
        doorAnimator = door.GetComponent<Animator>();
        textMeshProUGUI = doorUIRef.GetComponent<TextMeshProUGUI>();
    }
    public void ToggleDoor()
    {   
        if (opened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        
    }
    private void CloseDoor()
    {
        doorAudioSource.clip = audioClips[1];
        audioHandler.PlaySource(doorAudioSource, true, true, 0.8f, 1.2f, 0.7f, 1f);
        doorContactCollider.enabled = true;
        opened = false;
        StartCoroutine(SmoothLayerTransition(1, 2, 0.5f));

    }

    private void OpenDoor()
    {
        doorAudioSource.clip = audioClips[0];
        audioHandler.PlaySource(doorAudioSource, true, true, 0.8f, 1.2f, 0.7f, 1f);
        doorContactCollider.enabled = false;
        opened = true;
        StartCoroutine(SmoothLayerTransition(2, 1, 0.5f));
    }


    private IEnumerator SmoothLayerTransition(int fromLayer, int toLayer, float duration)
    {
        float timeElapsed = 0f;
        float startWeightFrom = doorAnimator.GetLayerWeight(fromLayer);
        float startWeightTo = doorAnimator.GetLayerWeight(toLayer);

        while (timeElapsed < duration)
        {
            // Lerp between the current weight and target weight
            float newWeightFrom = Mathf.Lerp(startWeightFrom, 0f, timeElapsed / duration);
            float newWeightTo = Mathf.Lerp(startWeightTo, 1f, timeElapsed / duration);

            // Set the weights on the layers
            doorAnimator.SetLayerWeight(fromLayer, newWeightFrom);
            doorAnimator.SetLayerWeight(toLayer, newWeightTo);

            // Increment time
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure we set the exact weights when the transition ends
        doorAnimator.SetLayerWeight(fromLayer, 0f);
        doorAnimator.SetLayerWeight(toLayer, 1f);
    }


}
