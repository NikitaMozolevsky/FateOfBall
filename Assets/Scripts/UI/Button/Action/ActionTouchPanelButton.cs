using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TouchPanel - отключена.
public class ActionTouchPanelButton : MonoBehaviour
{
    
    private SphereService sphereService = SphereService.instance;
    
    private Button panelButton;

    private void SubscribeEvents()
    { // Подписка и отписка от события
        ActionPlayButton.onPlay += TurnOnTouchPanel;
        GameController.onLose += TurnOffTouchPanel;
    }

    private void UnsubscribeEvents()
    {   
        ActionPlayButton.onPlay -= TurnOnTouchPanel;
        GameController.onLose -= TurnOffTouchPanel;
    }

    private void Awake()
    {
        panelButton = GetComponent<Button>();
    }
    
    private void Start()
    {
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void TurnOnTouchPanel()
    { // Включение / Выключение кнопки событием
        panelButton.enabled = true;
    }

    private void TurnOffTouchPanel()
    {
        panelButton.enabled = false;
    }

    public void OnClick()
    {
        sphereService.ToggleBoolean();
    }
}
