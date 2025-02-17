using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler
{   
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    /// <summary>
    /// Plays the given audio source. Takes various audio modifiers as parameters.
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="pitchMin"></param>
    /// <param name="pitchMax"></param>
    public void PlaySource(AudioSource audioSource, bool randPitch, bool randVolume, float pitchMin, float pitchMax, float volMin, float volMax)
    {
        if (audioSource != null)
        {   
            if (randPitch)
            {
                audioSource.pitch = Random.Range(pitchMin, pitchMax);
                
            }
            if (randVolume)
            {
                audioSource.volume = Random.Range(volMin, volMax);
            }
            
            audioSource.Play();
        }
        else Debug.LogWarning("AudioSource is null cannot play audio.");
    }

    public AudioSource[] GetAudioSources(GameObject obj)
    {
        return obj.GetComponents<AudioSource>();
    }

    public void LoadAudioClips()
    {
        audioClips["SpawnFootstep1"] = Resources.Load<AudioClip>("Assets/SFX/Player/PlayerSpawnFootstep1");
        audioClips["SpawnFootstep2"] = Resources.Load<AudioClip>("Assets/SFX/Player/PlayerSpawnFootstep2");
        audioClips["SpawnFootstep3"] = Resources.Load<AudioClip>("Assets/SFX/Player/PlayerSpawnFootstep3");
        audioClips["SpawnFootstep4"] = Resources.Load<AudioClip>("Assets/SFX/Player/PlayerSpawnFootstep4");

        audioClips["SpawnLightOn"] = Resources.Load<AudioClip>("Assets/SFX/StaticSFX/SpawnLightSFXNew");
        audioClips["SpawnLightHum"] = Resources.Load<AudioClip>("Assets/SFX/StaticSFX/SpawnLampFlicker");
        audioClips["SawedOffFire"] = Resources.Load<AudioClip>("Assets/SFX/ItemSFX/SawedOffFire");
        audioClips["SawedOffLoad"] = Resources.Load<AudioClip>("Assets/SFX/ItemSFX/ShellLoad");
        audioClips["SawedOffCock"] = Resources.Load<AudioClip>("Assets/SFX/ItemSFX/SawedOffCock");
        audioClips["SawedOffEject"] = Resources.Load<AudioClip>("Assets/SFX/ItemSFX/ShellEject");

    }

    /// <summary>
    /// Plays all the audiosources of a given GameObject.
    /// </summary>
    /// <param name="sourceObj"></param>
    /// <param name="randPitch"></param>
    /// <param name="randVolume"></param>
    /// <param name="pitchMin"></param>
    /// <param name="pitchMax"></param>
    /// <param name="volMin"></param>
    /// <param name="volMax"></param>
    public void PlaySources(GameObject sourceObj, bool randPitch, bool randVolume, float pitchMin, float pitchMax, float volMin, float volMax)
    {
        AudioSource[] audioSources = GetAudioSources(sourceObj);

        if (audioSources.Length > 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (randPitch)
                {
                    audioSource.pitch = Random.Range(pitchMin, pitchMax);

                }
                if (randVolume)
                {
                    audioSource.volume = Random.Range(volMin, volMax);
                }

                audioSource.Play();
            }
        }


    }

    //Fades dont work its fucked fix it later.
    public IEnumerator FadeSource(AudioSource audioSource, bool fadeIn = false, float fadeDuration = 10f)
    {
        if (!fadeIn)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                Debug.Log($"Volume is Fading out {audioSource.volume}");
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
            //Debug.Log("Audio Source Stopped after fade out.");
        }
        else
        {
            float startVolume = 0f;
            audioSource.volume = startVolume;
            audioSource.Play();

            while (audioSource.volume < 1f)
            {
                audioSource.volume += Time.deltaTime / fadeDuration;
                yield return null;
            }

            audioSource.volume = 1f;
        }
    }

    public void StopSources(GameObject sourceObj)
    {
        AudioSource[] audioSources = GetAudioSources(sourceObj);

        if (audioSources.Length > 0)
        {
            foreach(AudioSource audioSource in audioSources)
            {
                audioSource.Stop();
            }
        }
    }

    public void FadeSources(GameObject targetObj, bool fadeIn = false, float fadeDuration = 1f)
    {
        AudioSource[] audioSources = targetObj.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {   
            //Fix This Eventually coroutine helper is fucked rn.
            //CoroutineHelper.StartStaticCoroutine(FadeSource(audioSource, fadeIn, fadeDuration));
        }
    }



}
