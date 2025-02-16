using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimefluxInteraction : MonoBehaviour
{
    [Header("GameObect Should be the grounded Timeflux Device")]
    public GameObject timefluxDevice;
    public AudioSource interactSource;
    public AudioClip clip;
    private AudioHandler audioHandler = new AudioHandler();

    public Animator timefluxAnimator;
    private string itemDropAnimName = "TimefluxDrop";
    private string armDropAnimName = "FPTimefluxDrop";

    private void Start()
    {
        NewItemPickupHandler.Instance.SetItemAnimator(timefluxAnimator);
    }

    public void PickupTimeflux()
    {
        interactSource.clip = clip;
        audioHandler.PlaySource(interactSource, true, true, 0.7f, 1.3f, 0.75f, 1f);
        if (!NewItemPickupHandler.Instance.pickedUp)
            NewItemPickupHandler.Instance.PickupItem(timefluxDevice, timefluxAnimator);

    }

    public void DropTimeflux()
    {
        interactSource.clip = clip;
        audioHandler.PlaySource(interactSource, true, true, 0.7f, 1.3f, 0.75f, 1f);
        NewItemPickupHandler.Instance.DropItem(armDropAnimName, itemDropAnimName);
        Debug.Log("Timeflux Dropped");

    }
}
