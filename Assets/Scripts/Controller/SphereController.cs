using System;
using UnityEngine;
using UnityEngine.Events;

public class SphereController : MonoBehaviour
{
    // Единственный экземпляр синглтона
        public static SphereController instance { get; private set; }
    
    private SphereService sphereService = SphereService.instance;
    
    // Вызывается при каждом столкновении с шаром.
    public static UnityAction<Collision> onBallCollision;
    // Вызывается при первом столкновении с шаром.
    public static UnityAction onFirstBallCollision;
    
    public GameObject sphere;
    public Transform sphereStartPosition;
    // Звук касания.
    public AudioClip clickSound;

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
    
    private void SubscribeEvents()
    {
        ActionRestartButton.afterRestartGame += SetSphereStartPosition;
    }

    private void UnsubscribeEvents()
    {
        ActionRestartButton.afterRestartGame -= SetSphereStartPosition;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void Update()
    {
        sphereService.ScreenTouchManager();
    }

    private void FixedUpdate() // Для плавности движения
    {
        MoveSphere();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
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
