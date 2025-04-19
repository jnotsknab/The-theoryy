using UnityEngine;

public class ShotgunInteraction : MonoBehaviour
{
    [Header("GameObect Should be the grounded Shotgun")]
    public GameObject sawedOffShotgun;
    public AudioSource interactSource;
    public AudioClip clip;
    public ShotgunLogic shotgunLogic;
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
        audioHandler.PlaySource(interactSource, true, true, 0.4f, 0.6f, 0.75f, 1f);
        //if (!NewItemPickupHandler.Instance.pickedUp)
        //{

        //}
        //else
        //{
        //    NewItemPickupHandler.Instance.SwitchItem(0, sawedOffAnimator);
        //}
        NewItemPickupHandler.Instance.PickupItem(sawedOffShotgun, sawedOffAnimator);


    }

    public void DropSawedOff()
    {   
        if (!shotgunLogic.isReloading)
        {
            interactSource.clip = clip;
            audioHandler.PlaySource(interactSource, true, true, 0.4f, 0.6f, 0.75f, 1f);
            NewItemPickupHandler.Instance.DropItem(armDropAnimName, itemDropAnimName);
            Debug.Log("SawedOffDropped");
        }
    }
}