using UnityEngine;

public class ShotgunInteraction : MonoBehaviour
{
    [Header("GameObect Should be the grounded Shotgun")]
    public GameObject sawedOffShotgun;
    public AudioSource interactSource;
    public AudioClip clip;
    private AudioHandler audioHandler = new AudioHandler();


    public Animator sawedOffAnimator;
    private string itemDropAnimName = "SawedOffDrop";
    private string armDropAnimName = "FPSawedOffDrop";

    private void Start()
    {
        NewItemPickupHandler.Instance.SetItemAnimator(sawedOffAnimator);
    }

    public void PickupSawedOff()
    {
        interactSource.clip = clip;
        audioHandler.PlaySource(interactSource, true, true, 0.7f, 1.3f, 0.75f, 1f);
        if (!NewItemPickupHandler.Instance.pickedUp)
            NewItemPickupHandler.Instance.PickupItem(sawedOffShotgun, sawedOffAnimator);
        
    }

    public void DropSawedOff()
    {
        interactSource.clip = clip;
        audioHandler.PlaySource(interactSource, true, true, 0.7f, 1.3f, 0.75f, 1f);
        NewItemPickupHandler.Instance.DropItem(armDropAnimName, itemDropAnimName);
        Debug.Log("SawedOffDropped");
        
    }
}
