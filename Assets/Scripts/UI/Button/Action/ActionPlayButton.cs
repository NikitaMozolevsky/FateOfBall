using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPlayButton : MonoBehaviour
{
    public static UnityAction onPlay; // Вызывается при нажатии кнопки Play

    public void Play()
    {
        onPlay?.Invoke(); // Вызов события
    }
}
