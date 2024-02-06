

using System;
using UnityEngine;

public class ActionContinueButton : MonoBehaviour
{
    public static Action onContinueGame;
    
    public GameObject pausePanel;

    private void OnEnable()
    {
        onContinueGame += OffPausePanel;
    }

    private void OnDisable()
    {
        onContinueGame -= OffPausePanel;
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