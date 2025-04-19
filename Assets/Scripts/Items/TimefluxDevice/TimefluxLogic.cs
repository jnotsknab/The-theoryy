using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimefluxLogic : MonoBehaviour
{
    public TimeBody playerTimeBody;
    private List<TimeBody> objectTimeBodies = new List<TimeBody>();
    public GameObject vfxScanner;
    public AudioClip[] clips;
    private ParticleSystem scannerSphere;
    public GameObject objParticlePrefab;
    private AudioSource audioSource;

    private GameObject antenna;
    public Material antennaMat;
    private Light antennaLight;

    //Only the timeflux device should manipulate time, but in the future if anything else does i have this that we just set in the inspector.
    public bool hasBattery;
    public float batteryCharge;
    public float maxCharge;
    public float drainRate;

    private bool visualDisableInvoked = false;
    private List<TimeBody> timeBodies = new List<TimeBody>();

    //Scan Collision Detection
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public enum Mode
    {
        PlayerRewind,
        ObjectRewind,
        SlowMode
    }

    private Mode mode;


    private void Update()
    {   
        //bool ensures we only disable the visuals once not every frame
        if (!IsEquipped() && !visualDisableInvoked)
        {
            DisableVisuals();
        }
    }
    private void Awake()
    {

        antenna = GameObject.FindWithTag("Antenna");
        antennaLight = antenna.GetComponentInChildren<Light>();
        scannerSphere = vfxScanner.GetComponentInChildren<ParticleSystem>();
        mode = Mode.PlayerRewind;
        VisualizeModes();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        maxCharge = 10000f;
        //Set battery to max on awake
        batteryCharge = maxCharge;
        drainRate = 10.0f;
        timeBodies.Clear();
        
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
        else if (IsEquipped() && timeBodies.Count > 0 && mode == Mode.ObjectRewind && context.performed)
        {
            foreach (var body in timeBodies)
            {
                body.StartRewind();
            }
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
            //Scan is played here

            scannerSphere.Play();
            StartCoroutine(ExpandSphereCollider(1f, 120f, 3f));
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
            audioSource.volume = 0.75f;
            audioSource.Play();
            mode = (Mode)(((int)mode + 1) % System.Enum.GetValues(typeof(Mode)).Length);
            VisualizeModes();

            Debug.Log("Current Mode: " + mode.ToString());
        }
    }

    private void VisualizeModes()
    {
        visualDisableInvoked = false;
        if (mode == Mode.ObjectRewind)
        {
            SetMaterialProperties(true, new Color32(90, 210, 50, 255), 0);
            SetLightProperties(true, 0.1f, new Color32(90, 210, 50, 255));
        }
        else if (mode == Mode.PlayerRewind)
        {
            SetMaterialProperties(true, new Color32(25, 50, 210, 255), 0);
            SetLightProperties(true, 0.1f, new Color32(25, 50, 210, 255));
        }
        else
        {
            SetMaterialProperties(true, new Color32(200, 10, 0, 255), 0);
            SetLightProperties(true, 0.1f, new Color32(200, 10, 0, 255));
        }
        
    }

    private void DisableVisuals()
    {
        visualDisableInvoked = true;
        SetMaterialProperties(false, Color.black, 0);
        SetLightProperties(false, 0f, Color.black);
    }

    private void SetMaterialProperties(bool emissionEnabled, Color emissionColor, int emissionIntensity)
    {
        if (emissionEnabled)
        {
            antennaMat.EnableKeyword("_EMISSION");
            antennaMat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }
        else
        {
            antennaMat.DisableKeyword("_EMISSION");
            antennaMat.SetColor("_EmissionColor", Color.black);
        }

    }

    private void SetLightProperties(bool lightEnabled, float lightIntensity, Color lightColor)
    {
        if (lightEnabled)
        {
            antennaLight.enabled = true;
            antennaLight.color = lightColor;
            antennaLight.intensity = lightIntensity;
        }
        else
        {
            antennaLight.enabled = false;
            antennaLight.color = Color.black;
            //Ensures intensity is always 0
            antennaLight.intensity = lightIntensity * 0f;
        }
    }

    private void ObjectRewindCheck()
    {
        //This method will use the scanner sphere to check if it collides with any objects using a layermask for items, grounded items, and any other layer we need
        //if we collide, we will attempt to get a timebody component, if we find it we enable it (timebody will have clause where if it has a set rewind pos to go to we wont record and when rewind is called we just rewind to that set pos), once enabled it will remain enabled for a set amount of time where the player can rewind the object, visualized with fresnel effect.
    }

    /// <summary>
    /// Expands a sphere collider outwards, after lifetime has elapsed the collider radius will be 0 and the collider will be disabled.
    /// </summary>
    /// <param name="startRad"></param>
    /// <param name="endRad"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    private IEnumerator ExpandSphereCollider(float startRad, float endRad, float lifetime)
    {   
        
        float elapsedTime = 0f;
        SphereCollider collider = this.gameObject.GetComponent<SphereCollider>();
        if (collider == null) yield break;

        collider.enabled = true;
        collider.radius = startRad;

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lifetime;
            collider.radius = Mathf.Lerp(startRad, endRad, t);
            yield return null;
        }

        collider.radius = endRad;
        collider.radius = 0f;
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        TimeBody timeBody = other.gameObject.AddComponent<TimeBody>();
        timeBody.timefluxLogic = this;
        timeBodies.Add(timeBody);
        StartCoroutine(DoObjParticle(other.gameObject, timeBody));
    }

    private IEnumerator DoObjParticle(GameObject obj, TimeBody timeBody)
    {
        if (obj == null)
        {
            Debug.LogError("Parameter obj is null.");
            yield break;
        }

        GameObject particleObj = Instantiate(objParticlePrefab, obj.transform);
        particleObj.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(10);

        Destroy(timeBody);
        Destroy(particleObj);
        timeBodies.Clear();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null ) rb.isKinematic = false;

    }




}
