

using System.Collections;
using UnityEngine;

public class SoundService
{
    private static SoundService _instance;
    private AudioSource audioSource => 
        SoundController.instance.GetComponent<AudioSource>();
    
    private SoundService()
    {
    }
    
    public static SoundService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundService();
            }
            return _instance;
        }
    }

    // Звук осуществляется заблаговременно (много мороки)
    public static IEnumerator ShowButtonSound(AudioClip audioClip)
    {
        float soundDuration = audioClip.length;
        // Звучание звука не дольше чем движется кнопка.
        bool soundIsSuitable = soundDuration <= 
                               RepresentationButton.DURATION_OF_BUTTON_MOVEMENT;
        // Если длительность звука больше чем время появления кнопки - плохо.
        if (soundIsSuitable)
        {
            float timeToPlaySound = 
                RepresentationButton.DURATION_OF_BUTTON_MOVEMENT - soundDuration;
            yield return new WaitForSeconds(timeToPlaySound);
            SoundController.instance.audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("Soud too long!");
        }
    }
    public static void PlaySound(AudioClip audioClip)
    {
        SoundController.instance.audioSource.PlayOneShot(audioClip);
    }

    // Играет рандомный звук из переданных.
    public static void PlayRandomSound(params AudioClip[] audioClips)
    {
        if (audioClips.Length == 0)
        {
            Debug.Log("No parameters provided.");
            return;
        }
        
        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip randomSound = audioClips[randomIndex];

        PlaySound(randomSound);
    }
}