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
    private PlatformRemoveService prs = PlatformRemoveService.instance;
    private PlatformMovementService pms = PlatformMovementService.instance;
    private PlatformGenerationService pgs = PlatformGenerationService.instance;
    
    private PlatformService platformService = PlatformService.instance;
    public List<GameObject> platformList = new(); // Очередь из платформ
    // Вызывается при запуске игры и рестарте (по задумке)
    public static Action onNewGame;
    public static Action onDropPlatform;

    public GameObject viewPanel;
    [NonSerialized] public GameObject firstPlatform;
    public Transform platformsTransform;
    public AnimationCurve raiseCurve;
    public AnimationCurve dropCurve;

    // Точка спавна новой платформы, постоянно перемещается.
    // Будет -2 по z т.к. должна переместится на новую точку.
    public Transform currentGenerationPoint;
    public GameObject originalPlatform; // Префаб платформы.
    [NonSerialized] public bool platformGeneration = true;
    [NonSerialized] public bool smoothlyPlatformsRaised = false;
    
    // Позиции пеервых 2 платформ, ктороые всегда на одном месте
    private Vector3 firstPlatformVector;
    private Vector3 secondPlatformVector;
    private int platformCounter = 0;

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
        onDropPlatform += GenerateAndRaisePlatform;
    }

    private void OnDisable()
    {
        onNewGame -= PreparePlatformGenerator;
        ActionPlayButton.onPlay -= RemoveFirstPlatform;
        onDropPlatform -= GenerateAndRaisePlatform;
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
        PlatformsManager();
        
    }

    // Срабатывает при рестарте и запуске игры.
    public void PreparePlatformGenerator()
    {
        // Нужно переместить в другой метод, вызывающийся после уничтожения платформ.
        prs.DeleteAllPlatforms(platformList); // Очищение перед запуском
        
        // pgs.GenerateAndRaisePlatform(platformList); 
    }

    // После того как первые 100 платформ сгенерировались моментально опускает первые платформы
    // что бы потом подняить.
    private void RaisingFirstPlatforms()
    {
        pms.PutDownFirstPlatforms(platformList);
        StartCoroutine(pms.RaiseFirstPlatforms(platformList));
    }
    
    // Генерирует в Update платформы
    public void PlatformsManagerV1()
    {
        if (platformGeneration)
        {
            pgs.PlatformGeneratorV1(platformList);
        }

        if (!smoothlyPlatformsRaised)
        { // Срабатывает когда уже созданы SMOOTHLY_RAISED_PLATFORMS платформ
            // И эти платформы еще не были подняты, далее bool меняется что бы
            // Платформы не были подняты повторно
            if (platformList.Count > PlatformMovementService.SMOOTHLY_RAISED_PLATFORMS)
            {
                RaisingFirstPlatforms();
                smoothlyPlatformsRaised = true;
            }
        }
    }
    
    // Генерирует в Update платформы
    public void PlatformsManager()
    {
        if (platformGeneration)
        {
            pgs.PlatformGenerator(platformList);
        }
    }

    public GameObject CreatePlatformObject(Vector3 spawnPosition)
    {
        // Для нумерации платформ
        platformCounter++;
        string platformName = "Platform_" + platformCounter.ToString();
        GameObject newPlatform = Instantiate(originalPlatform, spawnPosition, Quaternion.identity);
        newPlatform.name = platformName;
        // PUT объекта в platforms empty object.
        newPlatform.transform.parent = platformsTransform;
        return newPlatform;
    }

    private void RemoveFirstPlatform()
    {
        prs.RemovePlatformManually(firstPlatform);
    }

    private void GenerateAndRaisePlatform()
    {
        pgs.GenerateAndRaisePlatform(platformList);
    }
} 