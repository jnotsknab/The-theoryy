using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InhalerLogic : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;

    public ParticleSystem inhalerParticles;
    private AnimationUtils animationUtils = new AnimationUtils();

    public Animator inhalerAnimator;
    public Animator armAnimator;

    public PlayerMovement playerMovement;

    public AudioSource inhalerSource;
    private AudioHandler audioHandler = new AudioHandler();

    private bool staminaInhalerBuff = false;
    private float buffMultiplier = 1.25f; 
    private bool buffApplied = false;
    private void Start()
    {
        volume.profile.TryGet(out lensDistortion);
    }
    public enum InhalerType
    {
        Health,
        Stamina,
        Speed

    }

    public InhalerType inhalerType;

    public int numCharges = 10;


    private void Update()
    {   
        float ogSprintSpeed = 15f;

        if (staminaInhalerBuff && !buffApplied)
        {   
            buffApplied = true;
            playerMovement.sprintSpeed *= buffMultiplier;
        }

        if (!staminaInhalerBuff && buffApplied)
        {
            buffApplied = false;
            playerMovement.sprintSpeed = ogSprintSpeed;
            
        }
    }
    private void UseInhaler()
    {
        //if (inhalerType == InhalerType.Health)
        //{
            
        //}

        DoHealthInhaler();
    }

    public void OnUseInhaler(InputAction.CallbackContext context)
    {
        if ( IsEquipped() && context.performed) UseInhaler();
    }

    private void DoHealthInhaler()
    {
        StartCoroutine(PlayInhaleAnimations());
        inhalerParticles.Play();
    }

    private IEnumerator PlayInhaleAnimations()
    {
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(armAnimator, "UseLayer", "FPInhalerUse"));
        StartCoroutine(animationUtils.SwapToLayerAndPlayAnimation(inhalerAnimator, "UseLayer", "InhalerUse"));
        yield return new WaitForSeconds(.25f);
        audioHandler.PlaySource(inhalerSource, true, true, 0.8f, 1.2f, 1.0f, 1.0f);
        //enable buffs
        staminaInhalerBuff = true;
        StartDistortionEffect(-1f, 2.75f);
        yield return new WaitForSeconds(2.75f);
        StartDistortionEffect(0.25f, 10f);
        yield return new WaitForSeconds(10);
        staminaInhalerBuff = false;

    }

    //Move this to a helper / utils class later.
    private bool IsEquipped()
    {
        if (NewItemPickupHandler.Instance.currentItemIDGlobal != 2)
        {
            return false;
        }
        return true;
    }


    //Invoked in an animation event for the inhaler.
    public void ResetToMovementState()
    {

        //Fix transition between end of fire animation to movement animation as its hella jerky right now.
        StartCoroutine(animationUtils.ActivateLayer(armAnimator, "ItemMovementLayer"));
        StartCoroutine(animationUtils.ActivateLayer(inhalerAnimator, "ItemMovementLayer"));

    }

    private void StartDistortionEffect(float targetDistortion, float duration)
    {
        StartCoroutine(ChangeDistortion(targetDistortion, duration));
    }


    private IEnumerator ChangeDistortion(float targetIntensity, float duration)
    {
        float time = 0f;
        float startIntensity = lensDistortion.intensity.value;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            lensDistortion.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        lensDistortion.intensity.value = targetIntensity;
        //Disable the buffs here
        
    }

}
