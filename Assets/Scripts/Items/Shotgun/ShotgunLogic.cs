using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.InputSystem;

public class ShotgunLogic : MonoBehaviour
{
    //public ItemPickupHandler itemPickupHandler;
    public VisualEffect muzzleFlash;
    public AudioSource shotgunShotSFX;
    public ScreenShake screenShake;

    [Header("Animation")]
    public Animator sawedOffAnimator;
    public Animator playerAnimator;
    public Animator shellBoreLeftAnimator;
    public Animator shellBoreRightAnimator;
    public Animator shellHandLeftAnimator;
    public Animator shellHandRightAnimator;
    private GameObject shellBoreLeft;
    private GameObject shellBoreRight;
    private GameObject shellHandLeft;
    private GameObject shellHandRight;
    public SkinnedMeshRenderer shellBoreLeftRenderer;
    public SkinnedMeshRenderer shellBoreRightRenderer;
    public SkinnedMeshRenderer shellHandLeftRenderer;
    public SkinnedMeshRenderer shellHandRightRenderer;


    [Header("Item Attributes")]
    public float damage = 50f;
    public float range = 11f;


    public int maxAmmo = 2;
    private int currentAmmo;
    public float maxReloadTime;
    public float minReloadTime = 1f;
    public bool isReloading = false;
    private bool knockBackRequested = false;

    private AnimationUtils animationUtils;
    private AudioHandler audioHandler = new AudioHandler();
    private AudioSource[] audioSources;
    private AudioSource fireSource;
    private AudioSource ejectSource;
    private AudioSource shellLoadSource;
    private AudioSource rackSource;

    public GameObject sawedOffObj;


    public Camera fpcam;
    public Rigidbody playerRigidbody;
    public float recoilForce = 10f;


    private void Awake()
    {
        animationUtils = gameObject.AddComponent<AnimationUtils>();
        shotgunShotSFX = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //Fetch all audio sources on the gun.
        audioSources = audioHandler.GetAudioSources(sawedOffObj);
        fireSource = audioSources[0];
        ejectSource = audioSources[1];
        shellLoadSource = audioSources[2];
        rackSource = audioSources[3];

        shellBoreLeft = GetShellRef("ShellBoreLeft");
        shellBoreRight = GetShellRef("ShellBoreRight");
        shellHandLeft = GetShellRef("ShellHandLeft");
        shellHandRight = GetShellRef("ShellHandRight");

        //Set all shells to inactive until reload animation.
        ResetShells();


        currentAmmo = maxAmmo;

        // If the Rigidbody is not assigned in the inspector, try to find it automatically
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponentInParent<Rigidbody>();
        }

        if (playerRigidbody == null)
        {
            Debug.LogError("No Rigidbody found on the player. Please assign one.");
        }
        if (sawedOffAnimator != null)
        {
            // Get the reload animation state info
            AnimatorStateInfo stateInfo = sawedOffAnimator.GetCurrentAnimatorStateInfo(sawedOffAnimator.GetLayerIndex("ReloadLayer"));

            // Set maxReloadTime to the length of the reload animation
            maxReloadTime = stateInfo.length;
            Debug.Log($"Reload time for sawed off is {maxReloadTime}");
        }

    }

    void Update()
    {

        // Return if were reloading so multiple coroutines arent started.
        if (isReloading)
        {
            return;
        }
        //Convert Reload to new input system
        if (currentAmmo <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());

            return;
        }

    }

    private void FixedUpdate()
    {
        if (knockBackRequested)
        {
            knockBackRequested = false;
            ApplyRecoil();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && currentAmmo > 0)
            Shoot();
    }


    //Fix this later ts inefficient
    private GameObject GetShellRef(string shellName)
    {
        // Attempt to find the GameObject with the specified name
        GameObject shell = GameObject.Find(shellName);

        if (shell != null)
        {
            Debug.Log($"Found shell: {shellName}");
            return shell;
        }
        else
        {
            Debug.LogWarning($"Shell with name '{shellName}' not found!");
            return null;
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        isReloading = true;

        StartCoroutine(PlayReloadAnimations());

        yield return new WaitForSeconds(maxReloadTime);
        //ResetShells();

        currentAmmo = maxAmmo;
        isReloading = false;
    }
    void Shoot()
    {
        RaycastHit hit;
        int numRays = 30;
        float spreadAngle = 0.15f;
        int numHits = 0;

        currentAmmo--;

        StartCoroutine(PlayFireAnimations());
        audioHandler.PlaySource(fireSource, true, false, 0.95f, 1.05f, 1f, 1f);
        muzzleFlash.Play();
        StartCoroutine(screenShake.Shake(.3f, 4f));

        knockBackRequested = true;

        for (int i = 0; i < numRays; i++)
        {
            Vector3 spreadDirection = fpcam.transform.forward
                                      + new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0f);

            if (Physics.Raycast(fpcam.transform.position, spreadDirection.normalized, out hit, range))
            {
                numHits++;
                Debug.DrawLine(fpcam.transform.position, hit.point, Color.red, 0.1f); // Red line to the hit point
                Debug.Log(hit.transform.name);

                TestTarget target = hit.transform.GetComponent<TestTarget>();
                if (target != null)
                {
                    float dmg = CalculateDamage(numHits, numRays);
                    target.TakeDamage(dmg);
                    Debug.Log("Damage: " + dmg);
                }

                //Debug.DrawRay(fpcam.transform.position, spreadDirection.normalized * range, Color.green, 0.1f); // Green ray to visualize spread
            }
            else
            {
                Debug.DrawRay(fpcam.transform.position, spreadDirection.normalized * range, Color.yellow, 0.1f); // Yellow ray
            }
        }
    }

    void ApplyRecoil()
    {
        if (playerRigidbody != null)
        {
            // Apply a backward force based on the camera's forward direction
            Vector3 recoilDirection = -fpcam.transform.forward;
            playerRigidbody.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);
        }
    }

    float CalculateDamage(int numHits, int numRays)
    {
        if (numHits == 0) return 0f;
        return damage / numHits;
    }

    private IEnumerator PlayFireAnimations()
    {
        // Start both animation coroutines
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(playerAnimator, "ShootLayer", "FPSawedOffShoot"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(sawedOffAnimator, "ShootLayer", "SawedOffShoot"));
        yield return new WaitForSeconds(3);

    }

    private IEnumerator PlayReloadAnimations()
    {

        ActivateShells();

        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(shellBoreLeftAnimator, "ShellBoreLeftLayer", "ShellBoreLeftReload"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(shellBoreRightAnimator, "ShellBoreRightLayer", "ShellBoreRightReload"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(shellHandLeftAnimator, "ShellHandLeftLayer", "ShellHandLeftReload"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(shellHandRightAnimator, "ShellHandRightLayer", "ShellHandRightReload"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(playerAnimator, "ReloadLayer", "FPSawedOffReload"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(sawedOffAnimator, "ReloadLayer", "SawedOffReload"));
        yield return null;
    }

    public void ResetToMovementState()
    {

        //Fix transition between end of fire animation to movement animation as its hella jerky right now.
        StartCoroutine(animationUtils.ActivateLayer(playerAnimator, "ItemMovementLayer"));
        StartCoroutine(animationUtils.ActivateLayer(sawedOffAnimator, "ItemMovementLayer"));

    }

    public void ResetShells()
    {

        shellBoreLeftRenderer.enabled = false;
        shellBoreRightRenderer.enabled = false;
        shellHandLeftRenderer.enabled = false;
        shellHandRightRenderer.enabled = false;
    }

    public void ActivateShells()
    {
        shellBoreLeftRenderer.enabled = true;
        shellBoreRightRenderer.enabled = true;
        shellHandLeftRenderer.enabled = true;
        shellHandRightRenderer.enabled = true;
    }

    public void EjectSFXEvent()
    {
        audioHandler.PlaySource(ejectSource, true, true, 0.95f, 1.05f, 0.5f, 0.75f);
    }

    public void ShellLoadSFXEvent()
    {
        audioHandler.PlaySource(shellLoadSource, true, true, 0.95f, 1.05f, 0.5f, 0.75f);
    }

    public void RackSFXEvent()
    {
        audioHandler.PlaySource(rackSource, true, true, 0.95f, 1.05f, 0.5f, 0.75f);
    }
}