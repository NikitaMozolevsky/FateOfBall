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
    private SphereService sphereService = SphereService.instance;
    private PlatformRemoveService prs = PlatformRemoveService.instance;
    private PlatformGenerationService pgs = PlatformGenerationService.instance;
    
    public GameObject sphere;
    public static Action onBallCollision;

    public GameObject platform;
    public Transform followSphere;
        
    private Rigidbody sphereRigitbody;
    private bool isFirstCollision = true;
    private Collision firstCollision;

    private SphereController()
    {
    }

    private void Awake()
    {
        CreateSingleton();
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
    {
        // Столкновение шара с платформой
        Debug.Log("Collision!");
        onBallCollision?.Invoke();
        prs.CheckMissedPlatformCollisionGPT(collision, PlatformController.instance.platformList);
    }

    public bool SphereOutOfPlatform()
    {
        return sphereService.SphereOutOfPlatform(sphere, platform);
    }
}
