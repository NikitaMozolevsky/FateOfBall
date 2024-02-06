

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformGenerationService
{
    private static PlatformGenerationService _instance;
    private PlatformMovementService pms = PlatformMovementService.instance;
    private static Util utilInstance = Util.instance;
    
    // Срабатывает когда платформа достигает верха.
    public static UnityAction<GameObject> onRaisedPlatform;
        
    // Подняты ли первые 20 платформ (осальные сразу на нужном месте).
    public static bool firstTwoPlatformsCreated = false;
    // Хранит значение предыдущего поворота.
    private bool previousTurn = true;
    // Считает сколько раз было одинаковое значение поворота.
    private int sameTurnCounter = 0;
    // Считает количество платформ, для именования.
    private int platformCounter = 0;
    // Максимальное кол-во одинаковых поворотов.
    public const int SAME_TURNS_COUNT = 4;
    // Минимальное количество платформ.
    public const int PLATFORM_COUNT = 11;
    // Время между поднятиями платформ.
    public const float TIME_BETWEEN_RAISING_PLATFORM = 0.2f;
    
    
    private PlatformGenerationService()
    {
    }

    // Статический метод для получения экземпляра синглтона.
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
    
    public GameObject GeneratePlatform(List<GameObject> platformList)
    {
        bool randomSide = GetPlatformTurn(platformList);
        
        GameObject newPlatform = PlatformController.instance.
            CreatePlatformObject(GetPlatformSpawnPoint(randomSide));
        
        // Инициализация первой платформы.
        if (PlatformController.instance.firstPlatform == null)
        {
            PlatformController.instance.firstPlatform = newPlatform;
        }
        platformList.Add(newPlatform);
        return newPlatform;
    }

    // В Update генерирует платформы, если все удовлетворяет условиям.
    public IEnumerator PlatformGenerator(List<GameObject> platformList)
    {
        // Еали количество
        if (GameService.instance.platformGeneration && !RaisedPlatformNumberIsSufficent())
        {
            // Задержка перед поднятием (можно убрать)
            yield return new WaitForSeconds(TIME_BETWEEN_RAISING_PLATFORM);
            GenerateAndRaisePlatform(platformList);
        }
    }

    // Генерирует и поднимает платформу.
    // Не работает при lose = true.
    public void GenerateAndRaisePlatform(List<GameObject> platformList)
    {
        if (!GameService.loseCondition)
        {
            GameObject platform = GeneratePlatform(platformList);
            GameController.instance.StartCoroutine
                (pms.MovePlatformTowardTarget(platform, true));
        }
    }
    
    // Генерирует сторону в которую будет направлена следующая платформа.
    // Учитывет что первые 2 должны быть направлены в лево.
    // Если переменная повторяется несколько раз - изменить вручную.
    private bool GetPlatformTurn(List<GameObject> platformList)
    {
        bool randomSide;
        if (!firstTwoPlatformsCreated)
        { // Первые 2 платформы направлены в лево.
            randomSide = false;
            if (platformList.Count > 1)
                firstTwoPlatformsCreated = true;
        }
        else
        { // После первых 2 в лево остальные рандомно.
            randomSide = utilInstance.GetRandomBool();
        }
        // Ограничивает количество одинакового направления до 5.
        // Если больше - меняет направление.
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
        // Смещение точки создания новой платформы зависит от текущего масштаба платформы.
        float spawnPointOffset = PlatformController.instance.originalPlatform.transform.localScale.x;

        Transform currentPosition = PlatformController.instance.currentPlatformGenerationPoint;

        if (rightTurn) // true - вправо, else - влево.
        {
            // Увеличение x координаты на 2.
            currentPosition.position += new Vector3(spawnPointOffset, 0f, 0f);
        }
        else
        {
            // Увеличение z координаты на 2.
            currentPosition.position += new Vector3(0f, 0f, spawnPointOffset);
        }

        // Установка Y на уровне отрицательного Y_DIFFERENCE.
        currentPosition.localPosition = new Vector3
        (currentPosition.localPosition.x, 
            -PlatformMovementService.Y_DIFFERENCE, 
            currentPosition.localPosition.z);

        // Перемещение точки спавна новой платформы.
        PlatformController.instance.currentPlatformGenerationPoint.position = currentPosition.position;

        return currentPosition.position;
    }

    // Cоздает новую платформу, именует и засовывет к остальным в parentTransform.
    public GameObject CreatePlatformObject
        (Vector3 spawnPosition, GameObject originalPlatform, Transform parentTransform)
    {
        // Для нумерации платформ.
        platformCounter++;
        string platformName = "Platform_" + platformCounter.ToString();
        GameObject newPlatform = GameController.Instantiate
            (originalPlatform, spawnPosition, Quaternion.identity);
        newPlatform.name = platformName;
        // PUT объекта в platforms empty object.
        newPlatform.transform.parent = parentTransform;
        return newPlatform;
    }

    // Инициализирует текущую точку генерации платформы,
    // присваивает ей значение стартовой точки.
    public void CreateCurrentGenerationPoint()
    {
        GameObject newGenerationPoint = new GameObject("CurrentGenerationPoint");
        newGenerationPoint.transform.parent = PlatformController.instance.parentTransform;
        newGenerationPoint.transform.position = 
            GetStartPoint().position;
        PlatformController.instance.currentPlatformGenerationPoint = newGenerationPoint.transform;
    }

    // Устанавливает точку генерации платформы на начальное положение.
    public void SetGenerationPointToStartPosition()
    {
        
        Debug.Log("SetGenerationPointToStartPosition");
        PlatformController.instance.currentPlatformGenerationPoint.position = 
            GetStartPoint().position;
    }

    public Transform GetStartPoint()
    {
        return PlatformController.instance.startPlatformGenerationPoint;
    }

    // Возвращает достаточно ли платформ подняты.
    public bool RaisedPlatformNumberIsSufficent()
    {
        return PLATFORM_COUNT == PlatformController.instance.raisedPlatformList.Count;
    }
    public IEnumerator GenerateFirstPlatforms(List<GameObject> raisedPlatformList)
    {
        for (int i = 0; i < PLATFORM_COUNT; i++)
        {
            yield return new WaitForSeconds(TIME_BETWEEN_RAISING_PLATFORM);
            GenerateAndRaisePlatform(raisedPlatformList);
        }
    }
}