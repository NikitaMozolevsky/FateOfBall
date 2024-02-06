using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepresentationPlayButton : MonoBehaviour
{
    private SphereService sphereService = SphereService.instance;
    
    private RectTransform buttonRectTransform;
    private Vector2 targetShowPlayPosition;
    private Vector2 targetHidePlayPosition;
      
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    
    private float elapsedTime;
    private float targetShowYPosition = 300f; // Позиция видимости кнопки Play.
    private float targetHideYPosition = -100f; // Позиция сокрытости кнопки Play.
    

    private void Start()
    {
        targetShowPlayPosition = new Vector2(0, targetShowYPosition);
        targetHidePlayPosition = new Vector2(0, targetHideYPosition);
    }

    private void OnEnable()
    {
        SphereController.onFirstBallCollision += ShowPlayButton;
        ActionPlayButton.onPlay += HidePlayButton;
        
    }
    
    private void OnDisable()
    {
        SphereController.onFirstBallCollision -= ShowPlayButton;
        ActionPlayButton.onPlay -= HidePlayButton;
    }

    // Показ кноки Play при ударе о платформу (первой коллизии).
    private void ShowPlayButton()
    {
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
                (gameObject, targetShowPlayPosition, showCurve));
    }

    // Сокрытие кнопки Play при нажатии.
    // Временное отключение.
    private void HidePlayButton()
    {
        // Сокрытие кнопки Play.
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetHidePlayPosition, hideCurve));
        // Отключение кнопки на 1 секунду, что бы не мешала игре.
        StartCoroutine(RepresentationButtonAction.instance.TemporarilyDeactivateButton
            (gameObject));
    }
    
}
 