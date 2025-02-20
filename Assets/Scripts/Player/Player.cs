using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Player class manages the player's movement, health, audio events, lighting, and interactions with the environment.
/// It handles footstep sounds based on the player's movement and location, spawn lighting, and the whether player movement is enabled.
/// </summary>
public class Player : MonoBehaviour
{

    private GameObject playerObject;
    private GameObject playerSelf;
    public GameObject rightHandEquipPos;

    public int playerHealth = 50;
    public GameObject[] spawnLightObjects;
    public Vector3[] spawnLightPositions;
    public LightingController lightingController = new LightingController();
    public AudioSource playerAudioSource;
    public AudioClip[] playerFootstepSFX;
    private AudioHandler audioHandler = new AudioHandler();

    private float stepSprintCooldown = 0.4f; // Time between steps
    private float stepCrouchCooldown = 0.75f;
    private float stepWalkCooldown = 0.6f;
    private float lastStepTime = 0f; // Time of the last step

    public PlayerMovement playerMovement;
    public string currentLocation;

    

    void Start()
    {
        PlayerInit();
        SpawnLightingInit();
        audioHandler.LoadAudioClips();
    }

    void Update()
    {   
        SpawnLightingEvents();
        PlayerAudioEvents();
    }
    private void PlayerInit()
    {
        playerObject = GetPlayerObject();
        playerSelf = GetPlayerSelf();
        
        
        
    }
    
    private void SpawnLightingInit()
    {
        spawnLightObjects = lightingController.GetLightObjects("SpawnLight");
        spawnLightPositions = lightingController.GetLightPositions(spawnLightObjects);


    }

    private void PlayerAudioEvents()
    {
        PlayerSpawnAudio();
    }

    private void PlayerSpawnAudio()
    {
        if (playerMovement.grounded && currentLocation == "SpawnRoom" && playerMovement.moveDirection != Vector3.zero)
        {
            if (playerMovement.sprinting && playerMovement.currentSprintTime >= 0)
            {
                if (Time.time - lastStepTime > stepSprintCooldown)
                {
                    PlayFootstepsSpawn();
                }
            }
            else if (playerMovement.crouching)
            {
                if (Time.time - lastStepTime > stepCrouchCooldown)
                {
                    PlayFootstepsSpawn();
                }
            }
            else
            {
                if (Time.time - lastStepTime > stepWalkCooldown)
                {
                    PlayFootstepsSpawn();
                }
            }
        }
    }

    public GameObject GetPlayerObject()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindWithTag("PlayerObject");
        }
        return playerObject;
    }

    public GameObject GetPlayerSelf()
    {
        if (playerSelf == null)
        {
            playerSelf = GameObject.FindWithTag("PlayerSelf");
        }
        return playerSelf;
    }

    private void PlayFootstepsSpawn()
    {
        AudioClip randomClip = playerFootstepSFX[Random.Range(0, playerFootstepSFX.Length)];
        //Debug.Log("Footstep SFX count: " + playerFootstepSFX.Length);

        playerAudioSource.clip = randomClip;
        audioHandler.PlaySource(playerAudioSource, true, true, 0.85f, 1.15f, 0.75f, 1f);

        lastStepTime = Time.time;
    }

    private void SpawnLightingEvents()
    {
        lightingController.EnableSpawnLightRange(spawnLightPositions, playerObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LocationZone"))
        {
            currentLocation = other.gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LocationZone"))
        {
            currentLocation = "";
        }
    }

    public void DisablePlayerMovement()
    {
        DisableAllColliders();
        if (playerMovement == null)
        {
            playerMovement = playerSelf.GetComponent<PlayerMovement>();
        }
        playerMovement.enabled = false;

    }

    public void EnablePlayerMovement()
    {   
        EnableAllColliders();
        if (playerMovement == null)
        {
            playerMovement = playerSelf.GetComponent<PlayerMovement>();
        }
        playerMovement.enabled = true;

    }

    private void DisableAllColliders()
    {
        if (rightHandEquipPos == null) return;

        Collider[] colliders = rightHandEquipPos.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void EnableAllColliders()
    {
        if (rightHandEquipPos == null) return;

        Collider[] colliders = rightHandEquipPos.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }


}
