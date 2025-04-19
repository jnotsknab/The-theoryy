
using UnityEngine;

public class InhalerInteraction : MonoBehaviour
{
    [Header("Gameobject should be the grounded item")]
    public GameObject inhaler;
    public AudioSource interactSource;
    public AudioClip clip;
    private AudioHandler audioHandler = new AudioHandler();

    public Animator inhalerAnimator;
    private string itemDropAnimName = "InhalerDrop";
    private string armDropAnimName = "FPInhalerDrop";

    private void Start()
    {
        NewItemPickupHandler.Instance.SetItemAnimator(inhalerAnimator);
    }

    public void PickupInhaler()
    {
        //interactSource.clip = clip;
        //audioHandler.PlaySource(interactSource, true, true, 1.25f, 1.5f, 0.75f, 1f);
        NewItemPickupHandler.Instance.PickupItem(inhaler, inhalerAnimator);
    }

    public void DropInhaler()
    {
        //interactSource.clip = clip;
        //audioHandler.PlaySource(interactSource, true, true, 1.25f, 1.5f, 0.75f, 1f);
        NewItemPickupHandler.Instance.DropItem(armDropAnimName, itemDropAnimName);
        Debug.Log("Inhaler Dropped");
    }
}
