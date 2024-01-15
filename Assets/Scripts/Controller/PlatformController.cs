using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static PlatformController instance { get; private set; }
    private PlatformService platformService = PlatformService.instance;
    public List<GameObject> platformList = new(); // Очередь из платформ
    // Вызывается при запуске игры и рестарте (по задумке)
    public static Action onNewGame;

    public GameObject viewPanel;
    [NonSerialized] public GameObject firstPlatform;
    public Transform platformsTransform;
    public AnimationCurve raisingCurve;

    // Точка спавна новой платформы, постоянно перемещается.
    // Будет -2 по z т.к. должна переместится на новую точку.
    public Transform currentGenerationPoint;
    public GameObject originalPlatform; // Префаб платформы.
    [NonSerialized] public bool platformGeneration = true;
    [NonSerialized] public bool smoothlyPlatformsRaised = false;
    
    // Позиции пеервых 2 платформ, ктороые всегда на одном месте
    private Vector3 firstPlatformVector;
    private Vector3 secondPlatformVector;

    private PlatformController()
    {
    } // Для Singleton

    private void Start()
    {
        CreateSingleton();
        onNewGame?.Invoke();
    }

    private void OnEnable()
    {
        onNewGame += PreparePlatformGenerator;
        ActionPlayButton.onPlay += RemoveFirstPlatform;
        //GameController.onLose += DestroyInvisiblePlatforms;
    }

    private void OnDisable()
    {
        onNewGame -= PreparePlatformGenerator;
        ActionPlayButton.onPlay -= RemoveFirstPlatform;
        //GameController.onLose -= DestroyInvisiblePlatforms;
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
        // Постоянное управление поднятием о опусканием платформы.
        PlatformListManager();
    }

    // Срабатывает при рестарте и запуске игры.
    public void PreparePlatformGenerator()
    {
        platformService.DeleteAllPlatforms(platformList); // Очищение перед запуском
    }

    // После того как первые 100 платформ сгенерировались моментально опускает первые платформы
    // что бы потом подняить.
    private void RaisingFirstPlatforms()
    {
        PlatformService.instance.PutDownFirstPlatforms(platformList);
        StartCoroutine(PlatformService.instance.RaiseFirstPlatforms(platformList));
    }
    
    // Генерирует в Update платформы
    public void PlatformListManager()
    {
        if (platformGeneration)
        {
            platformService.PlatformGenerator(platformList);
        }

        if (!smoothlyPlatformsRaised)
        { // Срабатывает когда уже созданы SMOOTHLY_RAISED_PLATFORMS платформ
            // И эти платформы еще не были подняты, далее bool меняется что бы
            // Платформы не были подняты повторно
            if (platformList.Count > PlatformService.SMOOTHLY_RAISED_PLATFORMS)
            {
                RaisingFirstPlatforms();
                smoothlyPlatformsRaised = true;
            }
        }
    }

    public GameObject CreatePlatformObject(Vector3 spawnPosition)
    {
        GameObject newPlatform = Instantiate(originalPlatform, spawnPosition, Quaternion.identity);
        // PUT объекта в platforms empty object.
        newPlatform.transform.parent = platformsTransform;
        return newPlatform;
    }

    private void RemoveFirstPlatform()
    {
        platformService.RemovePlatformManually(firstPlatform);
    }

    private void DestroyInvisiblePlatforms()
    {
        foreach (var platform in platformList)
        {
            platformService.DestroyPlatformIfInvisible(platform, viewPanel);
        }
    }

    public void GenerateByTransformPointCollision()
    { // Генерация алтформы когда в дочерний sphereCollider попадает точка transform платформы 
        
    }
} 