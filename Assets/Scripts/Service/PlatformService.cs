using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService
{
    private static PlatformService _instance;
    
    public const float DESIRED_DURATION = 3f;
    public const float Y_DIFFERENCE = 10;
    private float elapsedTime;
    private AnimationCurve platformCurve;
    
    private static Util utilInstance = Util.instance;
    public List<GameObject> platformList = new(); // Очередь из платформ
    // Подняты ли первые 20 платформ (осальные сразу на нужном месте)
    public static bool firstPlatformsRaised = false;

    private bool platformGeneration = true;
    private bool smoothlyPlatformsRaised = true;
    
    public const float PLATFORM_DISTANCE = 2f;
    public const int PLATFORM_COUNT = 100;
    public const int SMOOTHLY_RAISED_PLATFORMS = 20;

    // Статический метод для получения экземпляра синглтона
    public static PlatformService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new PlatformService();
            }
            return _instance;
        }
    }

    // Плавно перемещает платформу
    public IEnumerator MoveTowardsTarget(GameObject platform, bool raising)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = platform.transform.position;
        Vector3 tartgetPosition;
        
        tartgetPosition = GetTargetPositionForPlatform(platform, raising);
        platformCurve = PlatformController.instance.raisingCurve;

        while (elapsedTime < DESIRED_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / DESIRED_DURATION;
            platform.transform.position = Vector3.Lerp
                (startPosition, tartgetPosition, platformCurve.Evaluate(percentageComplete));
            yield return null;
        }
    }

    // Перемещает платформу моментально
    public void RelocatePlatformTowardTarget(GameObject platform, bool raising)
    {
        platform.transform.position = GetTargetPositionForPlatform(platform, raising);
    }

    // Возвращает верхнюю либо нижнюю позицию для каждой платформы в зависимости от bool
    private Vector3 GetTargetPositionForPlatform(GameObject platform, bool topPosition)
    { // Если целевая позиция верхняя позиция платформы, то Y_DIFFERENCE положиетелен, и наоборот.
        float yTargetPosition = Y_DIFFERENCE;
        yTargetPosition *= topPosition ? 1 : -1;
        
        Vector3 targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y + yTargetPosition, 
                platform.transform.position.z);
        
        return targetPosition;
    }
    
    // Генерирует в Update платформы
    public void PlatformListManager()
    {
        if (platformGeneration)
        {
            PlatformGenerator();
            PlatformRaiser();
        }
    }
    
    // Срабатывает при рестарте и запуске игры.
    public void PreparePlatformGenerator()
    {
        PlatformController.instance.DeleteAllPlatforms(); // Очищение перед запуском
        PutFirstTwoPlatformsInQueue();
    }
    
    public void PlatformGenerator()
    { // Если очередь из платформ меньше 100 создается новая платформа
        while (platformList.Count < PLATFORM_COUNT)
        {
            bool randomSide = utilInstance.GetRandomBool();
            GameObject newPlatform = PlatformController.instance.
                CreatePlatformObject(GetSpawnPoint(randomSide));
            platformList.Add(newPlatform);
        }
    }

    public void PlatformRaiser()
    { // Поднимает плавно первые 20 платформ, а остальные моментально
        if (!firstPlatformsRaised)
        {
            RaiseFirstPlatforms();
            firstPlatformsRaised = true;
        }
        else
        {
            
        }
    }
    
    public void PutFirstTwoPlatformsInQueue()
    { // Создание первых 2 платформ, put в очередь

        GameObject firstPlatform = PlatformController.instance.
            CreatePlatformObject(GetSpawnPoint(false));
        GameObject secondPlatform = PlatformController.instance.
            CreatePlatformObject(GetSpawnPoint(false));
        platformList.Add(firstPlatform);
        platformList.Add(secondPlatform);
    }
    
    public Vector3 GetSpawnPoint(bool rightTurn)
    {
        
        // Генерация случайного значения между true и false

        if (rightTurn) // true - в право, else - в лево.
        {
            // Увеличение x координаты на 2
            PlatformController.instance.currentGenerationPoint.position += new Vector3
                (PLATFORM_DISTANCE, 0f, 0f);
        }
        else
        {
            // Увеличение z координаты на 2
            PlatformController.instance.currentGenerationPoint.position += new Vector3
                (0f, 0f, PLATFORM_DISTANCE);
        }
        // Перемещение точки спавна новой платформы
        Vector3 spawnPoint = PlatformController.instance.currentGenerationPoint.position;

        return spawnPoint;
    }
    
    public void RaiseFirstPlatforms()
    { // Поднимает первые 20 платформ, остальные по идее уже должны быть в верху
        // Создать 2 листа в котром первые 20 создаются и поднимаются, затем
        // после перемещения точки спавна создаются на нужной высоте остальные платформы
        
    }
}
