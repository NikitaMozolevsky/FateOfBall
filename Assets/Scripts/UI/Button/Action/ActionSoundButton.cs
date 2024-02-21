

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionSoundButton : MonoBehaviour
{
    public Toggle toggle;
    public AudioSource audioSource;

    public static String MUSIC_PREF = "music";

    private void Start()
    {
        toggle.onValueChanged.AddListener(SwithSound);
        toggle.isOn = false;
    }

    public void SwithSound(bool isOn)
    {
        if (!toggle.isOn)
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }
    }
}