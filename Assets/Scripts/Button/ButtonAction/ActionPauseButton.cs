using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPauseButton : MonoBehaviour
{
    public static Action onPauseGame;
    
    public GameObject pausePanel;

    private void OnEnable()
    {
        onPauseGame += OnPausePanel;
    }
    
    private void OnDisable()
    {
        onPauseGame -= OnPausePanel;
    }

    public void Pause()
    {
        onPauseGame?.Invoke();
    }

    private void OnPausePanel()
    {
        pausePanel.SetActive(true);
    }
}
