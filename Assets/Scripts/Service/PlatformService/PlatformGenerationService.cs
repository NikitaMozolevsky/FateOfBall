

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerationService
{
    private static PlatformGenerationService _instance;
    private PlatformMovementService pms = PlatformMovementService.instance;
    private static Util utilInstance = Util.instance;
    
    // Подняты ли первые 20 платформ (осальные сразу на нужном месте)
    private bool firstTwoPlatformsCreated = false;
    // Хранит значение предыдущего поворота
    private bool previousTurn = true;
    // Считает сколько раз было одинаковое значение поворота
    private int sameTurnCounter = 0;
    
    public const int PLATFORM_COUNT = 1000;
    public const int SAME_TURNS_COUNT = 5;
    public const int MIN_PLATFORM_COUNT = 6;
    
    
    private PlatformGenerationService()
    {
    }

    // Статический метод для получения экземпляра синглтона
    public static PlatformGenerationService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new PlatformGenerationService();
            }
            return _instance;
        }
    }
    
    public void PlatformGeneratorV1(List<GameObject> platformList)
    { // Если очередь из платформ меньше 100 создается новая платформа
        while (platformList.Count < PLATFORM_COUNT)
        {
            GeneratePlatform(platformList);
        }
    }
    
    public GameObject GeneratePlatform(List<GameObject> platformList)
    {
        bool randomSide = GetPlatformTurn(platformList);
        
        GameObject newPlatform = PlatformController.instance.
            CreatePlatformObject(GetPlatformSpawnPoint(randomSide));
        
        // Инициализация первой платформы
        if (PlatformController.instance.firstPlatform == null)
        {
            PlatformController.instance.firstPlatform = newPlatform;
        }
        platformList.Add(newPlatform);
        return newPlatform;
    }

    public void PlatformGenerator(List<GameObject> platformList)
    {
        if (platformList.Count < MIN_PLATFORM_COUNT)
        {
            GenerateAndRaisePlatform(platformList);
        }
    }

    public void GenerateAndRaisePlatform(List<GameObject> platformList)
    {
        GameObject platform = GeneratePlatform(platformList);
        GameController.instance.StartCoroutine
            (pms.MovePlatformTowardTarget(platform, true));
    }
    
    // Генерирует сторону в которую будет направлена следующая платформа
    // Учитывет что первые 2 должны быть направлены в лево
    // Если переменная повторяется несколько раз - изменить вручную.
    private bool GetPlatformTurn(List<GameObject> platformList)
    {
        bool randomSide;
        if (!firstTwoPlatformsCreated)
        { // Первые 2 платформы направлены в лево
            randomSide = false;
            if (platformList.Count > 1)
                firstTwoPlatformsCreated = true;
        }
        else
        { // После первых 2 в лево остальные рандомно.
            randomSide = utilInstance.GetRandomBool();
        }
        // Ограничивает количество одинакового направления до 5
        // если больше - меняет направление
        if (randomSide == previousTurn)
        {
            sameTurnCounter++;
            if (sameTurnCounter == SAME_TURNS_COUNT)
            {
                randomSide = !randomSide;
            }
        }
        else
        {
            sameTurnCounter = 0;
        }
        previousTurn = randomSide;

        return randomSide;
    }
    
    public Vector3 GetPlatformSpawnPoint(bool rightTurn)
    {
        // Позиция точки спавна по вертикали зависит от bool переменной.
        /*float verticalValue = topPosition ? TOP_Y_PLATFORM_POSITION : BOTTOM_Y_PLATFORM_POSITION;*/
        // Смещение точки создания новой платформы зависит от текущего масштаба платформы.
        float spawnPointOffset = PlatformController.instance.originalPlatform.transform.localScale.x;

        Transform currentPosition = PlatformController.instance.currentGenerationPoint;

        if (rightTurn) // true - вправо, else - влево.
        {
            // Увеличение x координаты на 2
            currentPosition.position += new Vector3(spawnPointOffset, 0f, 0f);
        }
        else
        {
            // Увеличение z координаты на 2
            currentPosition.position += new Vector3(0f, 0f, spawnPointOffset);
        }

        // Установка Y на уровне отрицательного Y_DIFFERENCE
        currentPosition.localPosition = new Vector3
        (currentPosition.localPosition.x, 
            -PlatformMovementService.Y_DIFFERENCE, 
            currentPosition.localPosition.z);

        // Перемещение точки спавна новой платформы
        PlatformController.instance.currentGenerationPoint.position = currentPosition.position;

        return currentPosition.position;
    }
}