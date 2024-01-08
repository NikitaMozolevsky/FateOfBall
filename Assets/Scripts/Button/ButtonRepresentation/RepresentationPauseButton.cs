

using System;
using System.Collections;
using UnityEngine;

public class RepresentationPauseButton : MonoBehaviour
{
    private RectTransform buttonRectTransform;
    private Vector2 targetShowPausePosition;
    private Vector2 targetHidePausePosition;

    public GameObject pauseButton; // но весь скрипт не может быть на другой кнопке
    public AnimationCurve hideCurve;
    public AnimationCurve showCurve;
    
    private float elapsedTime;
    private bool showPlayButton = false;
    private float targetShowXPosition = 330f; // Позиция видимости кнопки Play
    private float targetHideXPosition = 500f; // Позиция сокрытости кнопки Play
    private void OnEnable()
    {
        ActionPlayButton.onPlay += ShowPauseButton;
        GameController.onLose += RemovePauseButton;
        ActionPauseButton.onPauseGame += DeletePauseButton;
        ActionContinueButton.onContinueGame += RefreshPauseButton;
        
    }
    private void OnDisable()
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
    }

    private void RemovePauseButton()
    { // Скрывает кнопку паузы за канвас
        Debug.Log("RemovePauseButton()");
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetHidePausePosition, showCurve));
    }

    private void ShowPauseButton()
    { // Коказывает кнопку паузы из-за канваса
        Debug.Log("RemovePauseButton()");
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetShowPausePosition, showCurve));
    }

    private void DeletePauseButton()
    { // Выключает отображение кнопки
        RepresentationButtonAction.instance.ButtonRendering(gameObject, false);
    }

    private void RefreshPauseButton()
    { // Включает отображение кнопки
        RepresentationButtonAction.instance.ButtonRendering(gameObject, true);
    }
}