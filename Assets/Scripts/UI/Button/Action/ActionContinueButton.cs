

using System;
using UnityEngine;

public class ActionContinueButton : MonoBehaviour
{
    public static Action onContinueGame;
    
    public GameObject pausePanel;

    private void SubscribeEvents()
    {
        onContinueGame += OffPausePanel;
    }

    private void UnsubscribeEvents()
    {
        onContinueGame -= OffPausePanel;
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    public void OnClick()
    {
        onContinueGame?.Invoke();
    }

    private void OffPausePanel()
    {
        pausePanel.SetActive(false);
    }
}