/*using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlatformGeneratorService
{
    private static PlatformGeneratorService _instance;
    private static PlatformService platformService = PlatformService.instance;
    private static Util utilInstance = Util.instance;
    public List<GameObject> platformList = new(); // Очередь из платформ
    // Подняты ли первые 20 платформ (осальные сразу на нужном месте)
    public static bool first20PlatformsRaised = false;

    private bool platformGeneration = true;
    private bool platformRaiser = true;
    
    public const float PLATFORM_DISTANCE = 2f;
    public const int PLATFORM_COUNT = 100;
    public const int RAISED_PLATFORMS = 20;
    
    public static PlatformGeneratorService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new PlatformGeneratorService();
            }
            return _instance;
        }
    }

    public void PlatformQueueManager()
    {
        if (platformGeneration)
        {
            PlatformGenerator();
        }
        if (!first20PlatformsRaised)
        {
            RaiseFirstPlatforms();
            first20PlatformsRaised = true;
        }
    }
    
    // Срабатывает при рестарте и запуске игры.
    public void PreparePlatformGenerator()
    {
        PlatformGeneratorController.instance.DeleteAllPlatforms(); // Очищение перед запуском
        PutFirstTwoPlatformsInQueue();
    }
    
    public void PlatformGenerator()
    { // Если очередь из платформ меньше 100 создается новая платформа
        while (platformList.Count < PLATFORM_COUNT)
        {
            bool randomSide = utilInstance.GetRandomBool();
            GameObject newPlatform = PlatformGeneratorController.instance.
                CreatePlatformObject(GetSpawnPoint(randomSide));
            platformList.Add(newPlatform);
        }
    }

    public void PlatformRaiser()
    {
        
    }
    
    public void EnablePlatformGeneration()
    {
        platformGeneration = true;
    }
    public void DisablePlatformGeneration()
    {
        platformGeneration = false;
    }
    
    public void PutFirstTwoPlatformsInQueue()
    { // Создание первых 2 платформ, put в очередь

        GameObject firstPlatform = PlatformGeneratorController.instance.
            CreatePlatformObject(GetSpawnPoint(false));
        GameObject secondPlatform = PlatformGeneratorController.instance.
            CreatePlatformObject(GetSpawnPoint(false));
        platformList.Add(firstPlatform);
        platformList.Add(secondPlatform);
    }
    
    public Vector3 GetSpawnPoint(bool right)
    {
        
        // Генерация случайного значения между true и false

        if (right) // true - в право, else - в лево.
        {
            // Увеличение x координаты на 2
            PlatformGeneratorController.instance.currentGenerationPoint.position += new Vector3
                (PLATFORM_DISTANCE, 0f, 0f);
        }
        else
        {
            // Увеличение z координаты на 2
            PlatformGeneratorController.instance.currentGenerationPoint.position += new Vector3
                (0f, 0f, PLATFORM_DISTANCE);
        }
        // Перемещение точки спавна новой платформы
        Vector3 spawnPoint = PlatformGeneratorController.instance.currentGenerationPoint.position;

        return spawnPoint;
    }
    

    public void RaiseFirstPlatforms()
    { // Поднимает первую в очереди платформу и затем удаляет
        for (int i = 0; i < RAISED_PLATFORMS; i++)
        {
            GameObject platform = platformList[i];
            platformService.MoveTowardsTarget(platform, true);
        }
    }
}*/