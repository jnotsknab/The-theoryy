using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimefluxLogic : MonoBehaviour
{
    public TimeBody playerTimeBody;
    public GameObject vfxScanner;
    public AudioClip[] clips;
    private ParticleSystem scannerSphere;
    private AudioSource audioSource;

    //Only the timeflux device should manipulate time, but in the future if anything else does i have this that we just set in the inspector.
    public bool hasBattery;
    public float batteryCharge;
    public float maxCharge;
    public float drainRate;

    public enum Mode
    {
        PlayerRewind,
        ObjectRewind,
        SlowMode
    }

    private Mode mode;


    private void Update()
    {
        
    }
    private void Awake()
    {   
        scannerSphere = vfxScanner.GetComponentInChildren<ParticleSystem>();
        mode = Mode.PlayerRewind;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        maxCharge = 100f;
        //Set battery to max on awake
        batteryCharge = maxCharge;
        drainRate = 10.0f;
        
    }

    /// <summary>
    /// Timeflux device input Event bound to left click
    /// </summary>
    /// <param name="context"></param>
    public void PrimaryInputEvent(InputAction.CallbackContext context)
    {
        if (IsEquipped() && mode == Mode.PlayerRewind && context.performed)
        {
            playerTimeBody.StartRewind();
        }
        

    }

    /// <summary>
    /// Timeflux device input event bound to right click.
    /// </summary>
    /// <param name="context"></param>
    public void SecondaryInputEvent(InputAction.CallbackContext context)
    {
        if (IsEquipped() &&  mode == Mode.PlayerRewind && context.performed)
        {
            playerTimeBody.StopRewind();
        }
        else if (IsEquipped() && mode == Mode.ObjectRewind && context.performed)
        {   
            //Set clip to scan clip
            audioSource.clip = clips[1];
            scannerSphere.Play();
            audioSource.Play();
            
        }
    }

    private bool IsEquipped()
    {
        if (NewItemPickupHandler.Instance.currentItemIDGlobal != 1)
        {
            return false;
        }
        return true;
    }

    public void ToggleMode(InputAction.CallbackContext context)
    {   
        if (IsEquipped() && context.performed)
        {
            //Set clip to toggle clip
            audioSource.clip = clips[0];
            audioSource.Play();
            mode = (Mode)(((int)mode + 1) % System.Enum.GetValues(typeof(Mode)).Length);
            Debug.Log("Current Mode: " + mode.ToString());
        }
    }

    private void TriggerScan()
    {

    }
}
