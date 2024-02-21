using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPauseButton : MonoBehaviour
{
    public static UnityAction onPauseGame;
    
    public GameObject pausePanel;

    private void SubscribeEvents()
    {
        onPauseGame += OnPausePanel;
    }
    
    private void UnsubscribeEvents()
    {
        onPauseGame -= OnPausePanel;
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
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
