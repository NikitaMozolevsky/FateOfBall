

using System;
using System.Collections;
using UnityEngine;

public class RepresentationPauseButton : MonoBehaviour
{
    private GameService gameService = GameService.instance;
    
    private RectTransform buttonRectTransform;
    private Vector2 targetShowPausePosition;
    private Vector2 targetHidePausePosition;
    
    public AnimationCurve showCurve;
    
    private float elapsedTime;
    private bool showPlayButton = false;
    private float targetShowXPosition = -100f; // Позиция видимости кнопки Play.
    private float targetHideXPosition = 100f; // Позиция сокрытости кнопки Play.
    private void SubscribeEvents()
    {
        ActionPlayButton.onPlay += ShowPauseButton;
        GameController.onLose += RemovePauseButton;
        ActionPauseButton.onPauseGame += DeletePauseButton;
        ActionContinueButton.onContinueGame += RefreshPauseButton;
        
    }
    private void UnsubscribeEvents()
    {
        ActionPlayButton.onPlay -= ShowPauseButton;
        GameController.onLose -= RemovePauseButton;
        ActionPauseButton.onPauseGame -= DeletePauseButton;
        ActionContinueButton.onContinueGame -= RefreshPauseButton;
    }

    private void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        targetShowPausePosition = new Vector2
            (targetShowXPosition, buttonRectTransform.anchoredPosition.y);
        targetHidePausePosition = new Vector2
            (targetHideXPosition, buttonRectTransform.anchoredPosition.y);
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void RemovePauseButton()
    { // Скрывает кнопку паузы за канвас.
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, targetHidePausePosition, showCurve));
    }

    private void ShowPauseButton()
    { // Коказывает кнопку паузы из-за канваса.
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, targetShowPausePosition, showCurve));
    }

    private void DeletePauseButton()
    { // Выключает отображение кнопки.
        RepresentationButton.instance.ButtonRendering(gameObject, false);
    }

    private void RefreshPauseButton()
    { // Включает отображение кнопки.
        RepresentationButton.instance.ButtonRendering(gameObject, true);
    }
}