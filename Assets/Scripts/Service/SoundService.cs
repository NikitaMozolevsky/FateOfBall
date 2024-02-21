

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
    
    public void PlaySound(AudioClip audioClip,
        bool destroy, float volume = 1f, float p1 = 0.85f, float p2 = 1.2f)
    {
        audioSource.pitch = UnityEngine.Random.Range(p1, p2);
        audioSource.PlayOneShot(audioClip, volume);
    }
}