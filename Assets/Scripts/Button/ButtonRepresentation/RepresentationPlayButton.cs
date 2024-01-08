using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepresentationPlayButton : MonoBehaviour
{

    private RectTransform buttonRectTransform;
    private Vector2 targetShowPlayPosition;
    private Vector2 targetHidePlayPosition;
    
    public float desiredDuration;   
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    
    private float elapsedTime;
    private bool showPlayButton = false;
    private float targetShowYPosition = -125f; // Позиция видимости кнопки Play
    private float targetHideYPosition = -230f; // Позиция сокрытости кнопки Play
    private bool isFirstCollision = true;
    

    private void Start()
    {
        buttonRectTransform = gameObject.GetComponent<RectTransform>();
        targetShowPlayPosition = new Vector2(0, targetShowYPosition);
        targetHidePlayPosition = new Vector2(0, targetHideYPosition);
    }

    private void OnEnable() // Подписка на удар о платформу
    {
        SphereController.onBallCollision += OnFirstBallCollision;
        ActionPlayButton.onPlay += OnPlay;
        
    }
    
    private void OnDisable() // Отписка
    {
        SphereController.onBallCollision -= OnFirstBallCollision;
        ActionPlayButton.onPlay -= OnPlay;
    }

    private void OnFirstBallCollision()
    { // Показ кноки Play при ударе о платформу (первой коллизии)
        if (isFirstCollision)
        {
            Debug.Log("Button show started!");
            // Показ кнопки Play
            StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
                (gameObject, targetShowPlayPosition, showCurve));
            isFirstCollision = false; // Вернуть в новом запуске игры
        }
    }

    private void OnPlay()
    { // Сокрытие кнопки Play
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetHidePlayPosition, hideCurve));
    }

    
}
 