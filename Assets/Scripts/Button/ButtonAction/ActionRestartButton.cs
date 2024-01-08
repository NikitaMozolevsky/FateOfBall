using System;
using UnityEngine;

public class ActionRestartButton : MonoBehaviour
{
    
    public static Action onRestartGame;
    
    public void Pause()
    {
        onRestartGame?.Invoke();
    }
    
}