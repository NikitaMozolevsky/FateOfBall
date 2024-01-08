using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTouchPanelButton : MonoBehaviour
{
    
    private SphereService sphereService = SphereService.instance;

    private Button panelButton;

    private void OnEnable()
    { // Подписка и отписка от события
        ActionPlayButton.onPlay += TurnOnTouchPanel;
        GameController.onLose += TurnOffTouchPanel;
    }

    private void OnDisable()
    {   
        ActionPlayButton.onPlay -= TurnOnTouchPanel;
        GameController.onLose -= TurnOffTouchPanel;
    }

    private void Start()
    {
        panelButton = GetComponent<Button>();
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
