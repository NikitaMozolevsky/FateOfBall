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
    public AudioClip moveButtonSound;
    
    private float elapsedTime;
    private float targetShowYPosition = 300f; // Позиция видимости кнопки Play.
    private float targetHideYPosition = -100f; // Позиция сокрытости кнопки Play.

    private void SubscribeEvents()
    {
        SphereController.onFirstBallCollision += ShowPlayButton;
        SphereController.onFirstBallCollision += ShowPlayButtonSound;
        ActionPlayButton.onPlay += HidePlayButton;
    }
    
    private void UnsubscribeEvents()
    {
        SphereController.onFirstBallCollision -= ShowPlayButton;
        SphereController.onFirstBallCollision -= ShowPlayButtonSound;
        ActionPlayButton.onPlay -= HidePlayButton;
    }
    
    private void Start()
    {
        targetShowPlayPosition = new Vector2(0, targetShowYPosition);
        targetHidePlayPosition = new Vector2(0, targetHideYPosition);
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    // Показ кноки Play при ударе о платформу (первой коллизии).
    private void ShowPlayButton()
    {
        StartCoroutine(RepresentationButton.MoveElementPosition
                (gameObject, targetShowPlayPosition, showCurve));
    }

    // Сокрытие кнопки Play при нажатии.
    // Временное отключение.
    private void HidePlayButton()
    {
        // Сокрытие кнопки Play.
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, targetHidePlayPosition, hideCurve));
        // Отключение кнопки на 1 секунду, что бы не мешала игре.
        StartCoroutine(RepresentationButton.instance.TemporarilyDeactivateButton
            (gameObject));
    }

    // Звучание при нажатии кнопки.
    private void ShowPlayButtonSound()
    {
        StartCoroutine(RepresentationButton.ShowButtonSound(moveButtonSound));
    }

    private void HidePlayButtonSound()
    {
        RepresentationButton.instance.HideButtonSound(moveButtonSound);
    }
}
 