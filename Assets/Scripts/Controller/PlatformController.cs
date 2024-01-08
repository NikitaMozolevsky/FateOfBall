using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static PlatformController instance { get; private set; }
    private PlatformService platformService = PlatformService.instance;

    public Transform platformsTransform;
    public GameObject firstPlatform;
    public AnimationCurve raisingCurve;
    public AnimationCurve lowerCurve;
    
    public static Action onPlatformsGenerated;
    
    
    // Точка спавна новой платформы, постоянно перемещается.
    // Будет -2 по z т.к. должна переместится на новую точку.
    public Transform currentGenerationPoint;
    public GameObject originalPlatform; // Префаб платформы.
    
    // Позиции пеервых 2 платформ, ктороые всегда на одном месте
    private Vector3 firstPlatformVector;
    private Vector3 secondPlatformVector;
    
    
    private PlatformController() {} // Для Singleton

    private void Awake()
    {
        CreateSingleton();
    }
    
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        // Для тестирования
        ControlPlatformByButtons();
        // Постоянное управление поднятием о опусканием платформы.
        PlatformService.instance.PlatformListManager();
    }

    private void ControlPlatformByButtons()
    {
        if (Input.GetKeyDown(KeyCode.A))
        { // Опустить платформу
            StartCoroutine(platformService.MoveTowardsTarget
                (firstPlatform, false));
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        { // Поднять платформу
            StartCoroutine(platformService.MoveTowardsTarget
                (firstPlatform, true));
        }
    }

    private void RaisePlatformSmoothly(GameObject platform, bool raising)
    {
        StartCoroutine(platformService.MoveTowardsTarget
            (platform, raising));
    }

    private void RaisePlatformSharply(GameObject platform, bool raising)
    {
        platformService.RelocatePlatformTowardTarget(platform, raising);
    }
    
    public GameObject CreatePlatformObject(Vector3 spawnPosition)
    {
        GameObject newPlatform = Instantiate(originalPlatform, spawnPosition, Quaternion.identity);
        // PUT объекта в platforms empty object.
        newPlatform.transform.parent = platformsTransform;
        return newPlatform;
    }
    
    public void DeleteAllPlatforms()
    {
        foreach (var platform in platformService.platformList)
        {
            Destroy(platform);
        }
    }
}
