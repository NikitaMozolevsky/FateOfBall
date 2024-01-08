using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    // Единственный экземпляр синглтона
    public static SphereController instance { get; private set; }
    public static GameObject sphere;
    public static Action onBallCollision;

    private SphereService sphereService = SphereService.instance;

    public bool isLeft = true;
    public float sphereSpeed = 5f;
    public GameObject platform;
        
    private Rigidbody sphereRigitbody;

    public float SphereSpeed
    {
        get { return sphereSpeed; }
        set { sphereSpeed = value; }
    }

    public bool IsLeft
    {
        get { return isLeft; }
        set { isLeft = value; }
    }

    private void Awake()
    {
        CreateSingleton();
        sphere = gameObject;
        sphereRigitbody = sphere.GetComponent<Rigidbody>();
    }

    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void FixedUpdate() // Для плавности движения
    {
        if (GameController.playCondition)
        {
            MoveSphere(); 
        }
    }

    private void MoveSphere()
    {
        if (sphereRigitbody != null && platform != null)
        {
            sphereService.MoveSphereWithTransform(sphere, platform);
        }
        else
        {
            Debug.LogWarning("Rigidbody or platform is null");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    { // Столкновение шара с платформой
        Debug.Log("Collision!");
        onBallCollision?.Invoke();
    }

    public bool SphereOutOfPlatform()
    {
        return sphereService.SphereOutOfPlatform(sphere, platform);
    }
}
