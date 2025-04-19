using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LeverLogic : MonoBehaviour
{
    private AnimationUtils animationUtils = new AnimationUtils();
    public Animator leverAnimator;

    public VisualEffect sparkEffect;
    public VisualEffect beamEffect;

    public LightUtils lightUtils;

    public bool leverEnabled;

    private GameObject beamImpactObj;
    private GameObject sparkObj;

    private AudioSource sparkSource;
    private AudioSource beamSource;
    public AudioSource playerSource;

    public GameObject playerObj;
    public bool jumperTimeTravelEnabled = true;

    public ScreenShake screenShake;

    public Material timeTravelScreenShaderMat;

    void Start()
    {
        leverEnabled = false;
        sparkObj = GameObject.FindGameObjectWithTag("InitLight");
        beamImpactObj = GameObject.FindGameObjectWithTag("ImpactLight");
        beamSource = beamImpactObj.GetComponent<AudioSource>();
        sparkSource = sparkObj.GetComponent<AudioSource>();
        
    }

    public void DoLeverStuff()
    {   
        if (leverEnabled)
        {
            PlayLeverAnim();
            StartCoroutine(TriggerJumperBeam());
            jumperTimeTravelEnabled = true;
            leverEnabled = false;
        }
        if (jumperTimeTravelEnabled)
        {
            
            StartCoroutine(TimeTravelPlayer());
        }
        
        
    }
    private void PlayLeverAnim()
    {
        leverAnimator.SetTrigger("Action");
    }

    private IEnumerator TriggerJumperBeam()
    {
        Light light = sparkObj.GetComponent<Light>();
        Light light2 = beamImpactObj.GetComponent<Light>();

        yield return new WaitForSeconds(1f);

        sparkEffect.Play();
        sparkSource.Play();
        lightUtils.PulseLight(light, 0.25f, 2f, 0.25f, 0.25f, true);
        beamEffect.Play();
        beamSource.Play();
        lightUtils.PulseLight(light2, 0.1f, 1f, 0.15f, 0.1f, true);
        StartCoroutine(LerpTimeTravelVignette(-0.25f));


    }


    //Work in progress, i think this lever inside of the jumper itself will control the actual teleporting / time traveling of the player within the jumper.
    private IEnumerator TimeTravelPlayer()
    {   
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(screenShake.Shake(10f, 0.65f));
        yield return new WaitForSeconds(3.75f);
        playerSource.Play();
        timeTravelScreenShaderMat.SetFloat("_VignetteIntensity", 0f);
        timeTravelScreenShaderMat.SetFloat("_VignettePower", 2f);
        playerObj.transform.position = new Vector3(-40f, 588f, 78f);
        
    }

    private IEnumerator LerpTimeTravelVignette(float targetIntensity, float lerpSpeed = 1f)
    {
        float currentValue = timeTravelScreenShaderMat.GetFloat("_VignettePower");
        timeTravelScreenShaderMat.SetFloat("_VignetteIntensity", 0.15f);

        while (currentValue > targetIntensity)
        {
            currentValue = Mathf.Lerp(currentValue, targetIntensity, Time.deltaTime * lerpSpeed);
            timeTravelScreenShaderMat.SetFloat("_VignettePower", currentValue);
            yield return null;
        }

        timeTravelScreenShaderMat.SetFloat("_VignettePower", targetIntensity);
        
        
    }

}
