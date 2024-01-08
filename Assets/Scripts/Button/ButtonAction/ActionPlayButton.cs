using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlayButton : MonoBehaviour
{
    public static Action onPlay; // Вызывается при нажатии кнопки Play
    
    public void Play()
    {
        onPlay?.Invoke(); // Вызов события
    }
}
