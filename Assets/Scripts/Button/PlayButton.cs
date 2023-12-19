using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public static bool isPlay = false;
    public GameObject _playButton;
    
    public void Play()
    {
        Debug.Log("Play!");
        isPlay = true;
        _playButton.SetActive(false);
    }
}
 