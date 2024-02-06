using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SphereController : MonoBehaviour
{
    // Единственный экземпляр синглтона
    public static SphereController instance { get; private set; }
    
    private SphereService sphereService = SphereService.instance;
    private GameService gameService = GameService.instance;
    private PlatformRemoveService prs = PlatformRemoveService.instance;
    private PlatformGenerationService pgs = PlatformGenerationService.instance;
    
    // Вызывается при каждом столкновении с шаром.
    public static UnityAction<Collision> onBallCollision;
    // Вызывается при первом столкновении с шаром.
    public static UnityAction onFirstBallCollision;
    // Вызывается при каждой коллизии сферы.
    /*public static UnityAction onDropPlatform;*/
    
    public GameObject sphere;
    public Transform sphereStartPosition;

    private SphereController()
    {
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
    
    private void OnEnable()
    {
        ActionRestartButton.afterRestartGame += SetSphereStartPosition;
        // Остановка и запуск времени.
        /*onFirstBallCollision += gameService.StopTime;
        ActionPlayButton.onPlay += gameService.ContinueTime;*/
    }

    private void OnDisable()
    {
        ActionRestartButton.afterRestartGame -= SetSphereStartPosition;
        /*onFirstBallCollision -= gameService.StopTime;
        ActionPlayButton.onPlay -= gameService.ContinueTime;*/
    }

    private void Awake()
    {
        CreateSingleton();
    }

    private void FixedUpdate() // Для плавности движения
    {
        MoveSphere();
    }
    
    // Столкновение шара с платформой.
    private void OnCollisionEnter(Collision collision)
    {
        sphereService.OnCollisionEnter(collision);
    }
    
    private void MoveSphere()
    {
        sphereService.MoveSphere(sphere);
    }
    
    // Возвращает шар на стартовую позицию с которой он падает.
    private void SetSphereStartPosition()
    {
        sphereService.SetSphereStartPosition(sphere, sphereStartPosition);
    }
}
