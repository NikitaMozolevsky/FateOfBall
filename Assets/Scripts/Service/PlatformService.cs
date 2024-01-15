using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService
{

    private static PlatformService _instance;
    
    public const float DESIRED_DURATION = 1f;
    // 
    public const float Y_DIFFERENCE = 40;
    private float elapsedTime;
    private AnimationCurve platformCurve;
    
    private static Util utilInstance = Util.instance;
    // Подняты ли первые 20 платформ (осальные сразу на нужном месте)
    public static bool firstTwoPlatformsCreated = false;
    
    // Ткущая позиция точки спавнаа
    /*private Vector3 spawnPoint = PlatformController.instance.currentGenerationPoint.position;/*#1#*/
    private bool platformGeneration = true;
    // Хранит значение предыдущего поворота
    private bool previousTurnValue = true;
    // Считает сколько раз было одинаковое значение поворота
    private int sameTurnCounter = 0;
    // Точка спавна платформы, перемещается в цикле.
    private Vector3 startSpawnPoint;
    
    public const float PLATFORM_DISTANCE = 2f;
    public const int PLATFORM_COUNT = 100;
    // Платформы которые поднимаются без коллизии
    public const int OFF_COLLISION_PLATFORM_COUNT = 15;
    public const int SMOOTHLY_RAISED_PLATFORMS = 30;
    public const int TOP_Y_PLATFORM_POSITION = 0;
    public const int BOTTOM_Y_PLATFORM_POSITION = -45;
    public const float TIME_TO_DROP_PLATFORM = 0.2f;
    public const float TIME_TO_DESTROY_PLATFORM = 2f;
    public const string TOP_VISIBLE_POINT = "TopVisiblePoint";
    public const string BOT_VISIBLE_POINT = "BotVisiblePoint";
    
    private PlatformService()
    {
    }

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

    // Плавно перемещает платформу вверх или в низ, в зависимости от bool
    public IEnumerator MovePlatformTowardTarget(GameObject platform, bool raising)
    {
        // Рандомная задержка в пределах 1 секунды
        /*yield return new WaitForSeconds(Random.Range(0f, DESIRED_DURATION));*/
        
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
    public void TeleportPlatformTowardTarget(GameObject platform, bool raising)
    {
        platform.transform.position = GetTargetPositionForPlatform(platform, raising);
    }

    // Возвращает верхнюю либо нижнюю позицию для каждой платформы в зависимости от bool
    private Vector3 GetTargetPositionForPlatform(GameObject platform, bool topPosition)
    { // Если целевая позиция верхняя позиция платформы, то Y_DIFFERENCE положиетелен, и наоборот.
        float yTargetPosition = Y_DIFFERENCE;
        yTargetPosition *= topPosition ? 1f : -1f;
        
        Vector3 targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y + yTargetPosition, 
                platform.transform.position.z);
        
        return targetPosition;
    }

    public void PlatformGenerator(List<GameObject> platformList)
    { // Если очередь из платформ меньше 100 создается новая платформа
        while (platformList.Count < PLATFORM_COUNT)
        {
            bool randomSide;
            if (!firstTwoPlatformsCreated)
            { // Первые 2 платформы направлены в лево
                randomSide = false;
                if (platformList.Count > 1)
                    firstTwoPlatformsCreated = true;
            }
            else
            {
                randomSide = utilInstance.GetRandomBool();
            }

            GameObject newPlatform = PlatformController.instance.
                CreatePlatformObject(GetPlatformSpawnPoint(randomSide, false));
            
            // Инициализация первой платформы
            if (PlatformController.instance.firstPlatform == null)
            {
                PlatformController.instance.firstPlatform = newPlatform;
            }
            platformList.Add(newPlatform);
        }
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
        if (randomSide == previousTurnValue)
        {
            sameTurnCounter++;
            if (sameTurnCounter == 5)
            {
                randomSide = !randomSide;
            }
        }
        else
        {
            sameTurnCounter = 0;
        }
        previousTurnValue = randomSide;

        return randomSide;
    }
    
    public Vector3 GetPlatformSpawnPoint(bool rightTurn, bool topPosition)
    {
        // Позиция точки спавна по вертикали зависит от bool переменной.
        float verticalValue = topPosition ? TOP_Y_PLATFORM_POSITION : BOTTOM_Y_PLATFORM_POSITION;
        // Смещение точки создания новой платформы зависит от текущего масштаба платформы.
        float spawnPointOffset = PlatformController.instance.originalPlatform.transform.localScale.x;

        Vector3 currentPosition = PlatformController.instance.currentGenerationPoint.position;

        if (rightTurn) // true - вправо, else - влево.
        {
            // Увеличение x координаты на 2
            currentPosition += new Vector3(spawnPointOffset, 0f, 0f);
        }
        else
        {
            // Увеличение z координаты на 2
            currentPosition += new Vector3(0f, 0f, spawnPointOffset);
        }

        // Установка Y на уровне verticalValue
        currentPosition.y = verticalValue;

        // Перемещение точки спавна новой платформы
        PlatformController.instance.currentGenerationPoint.position = currentPosition;

        return currentPosition;
    }
    
    public IEnumerator RaiseFirstPlatforms(List<GameObject> platformList)
    { // Поднимает первые 20 платформ, остальные по идее уже должны быть в верху.
        // Теперь все платформы создаются в верху, а затем первые сразу резко
        // опускаются в низ.
        for (int i = 0; i < SMOOTHLY_RAISED_PLATFORMS; i++)
        { // Плавно поднимает платформу
            yield return new WaitForSeconds(0.1f);
            GameObject platform = platformList[i];
            PlatformController.instance.StartCoroutine
                (MovePlatformTowardTarget(platform, true));
        }
    }

    public void PutDownFirstPlatforms(List<GameObject> platformList)
    { // Опускает в низ первые платформы что бы поднять
        for (int i = 0; i < SMOOTHLY_RAISED_PLATFORMS; i++)
        { // Резко опускает платформу
            GameObject platform = platformList[i];
            TeleportPlatformTowardTarget(platform, false);
        }
    }
    
    public void OffPlatformKinematic(GameObject platform)
    {
        // Подение за счет отключения кинематичности
        platform.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void PassedPlatformManager(Collision collision)
    {
        GameObject platform = collision.gameObject;
        GameController.instance.StartCoroutine(DropPlatform(platform));
        GameController.instance.StartCoroutine(DestroyPlatform(platform));
    }
    
    public IEnumerator DropPlatformIfPlay(GameObject platform)
    { // Роняет платформу если состояние игры
        if (GameController.playCondition)
        {
            yield return new WaitForSeconds(TIME_TO_DROP_PLATFORM);
            // Сила притяжения
            OffPlatformKinematic(platform);
            // Слишком быстро падает, мешает игре если поднимать
            /*StartCoroutine(platformService.MoveTowardsTarget(droppingPlatform, true));*/
        }
    }
    
    public IEnumerator DestroyPlatformIfPlay(GameObject platform)
    { // Если игры запущена и прошло определенное врямя падающая платформа удаляется
        // Удаляет платформу если состояние игры
        if (GameController.playCondition)
        {
            yield return new WaitForSeconds(TIME_TO_DESTROY_PLATFORM);
            GameController.Destroy(platform);
        }
    }
    
    public IEnumerator DropPlatform(GameObject platform)
    { // Роняет платформу
        
        yield return new WaitForSeconds(TIME_TO_DROP_PLATFORM);
        // Сила притяжения
        OffPlatformKinematic(platform);
        // Слишком быстро падает, мешает игре если поднимать
        /*StartCoroutine(platformService.MoveTowardsTarget(droppingPlatform, true));*/
        
    }
    
    public IEnumerator DestroyPlatform(GameObject platform)
    { // Удаляет объект через время
    
        yield return new WaitForSeconds(TIME_TO_DESTROY_PLATFORM);
        GameController.Destroy(platform);
    }

    public void CheckMissedPlatformCollision(Collision collision, List<GameObject> platformList)
    { // Удалить объект если по какой-то причине шар не сделал для него коллизии
        // и он остался на месте, обычно такой объект тольок 1.
        
        GameObject platform = collision.gameObject; // 
        int currentPlatformIndex = platformList.IndexOf(platform);
        // Предыдущая платформа
        GameObject previousPlatform = platformList[currentPlatformIndex - 1];
        Rigidbody previousPlatformRigitbody = previousPlatform.GetComponent<Rigidbody>();
        // Если предыдущая платформа кинематична - она не упала
        if (previousPlatformRigitbody.isKinematic)
        { // Уронить и удалить платформу
            RemovePlatformManually(previousPlatform);
        }
    }
    
    public void CheckMissedPlatformCollisionGPT(Collision collision, List<GameObject> platformList)
    {
        if (GameController.playCondition)
        {
            GameObject platform = collision.gameObject;
            int currentPlatformIndex = platformList.IndexOf(platform);

            // Проверяем текущую и предыдущие платформы до тех пор, пока не найдем первую некинематичную платформу
            for (int i = currentPlatformIndex; i >= 0; i--)
            {
                GameObject currentOrPreviousPlatform = platformList[i];
                Rigidbody platformRigidbody = currentOrPreviousPlatform.GetComponent<Rigidbody>();

                // Если платформа не кинематична - она не упала
                if (!platformRigidbody.isKinematic)
                {
                    break; // Прерываем цикл, так как нашли первую некинематичную платформу
                }

                // Уронить и удалить кинематичную платформу с рандомной задержкой
                GameController.instance.StartCoroutine
                    (RemovePlatformManuallyWithRandomDelay
                        (currentOrPreviousPlatform, TIME_TO_DROP_PLATFORM));
            }
        }
    }

    public void RemovePlatformManually(GameObject platform)
    { // Для того что бы уронить и удалить первую платформу
      // (можно было бы поставть время на 0)
        // (как по другому не придумал), запукается при нажатии PlayButton
        GameController.instance.StartCoroutine(DropPlatform(platform));
        GameController.instance.StartCoroutine(DestroyPlatform(platform));
    }

    public IEnumerator RemovePlatformManuallyWithRandomDelay(GameObject platform, float delay)
    { // Удаление с рандомной задержкой
        yield return new WaitForSeconds(Random.Range(0f, delay));
        RemovePlatformManually(platform);
    }
    
    public void DeleteAllPlatforms(List<GameObject> platformList)
    {
        foreach (var platform in platformList)
        {
            GameController.Destroy(platform);
        }
    }

    public void DestroyPlatformIfInvisible(GameObject platform, GameObject viewPanel)
    {
        // Получаем точки видимости на платформе
        Transform topVisiblePoint = platform.transform.Find(TOP_VISIBLE_POINT);
        Transform botVisiblePoint = platform.transform.Find(BOT_VISIBLE_POINT);

        // Получаем основную камеру
        Camera mainCamera = Camera.main;

        // Создаем луч от основной камеры к верхней точке
        Ray topRay = mainCamera.ScreenPointToRay(topVisiblePoint.position);

        // Создаем луч от основной камеры к нижней точке
        Ray botRay = mainCamera.ScreenPointToRay(botVisiblePoint.position);

        // Переменные для хранения результата лучевого тестирования
        RaycastHit topHit, botHit;

        // Проверяем, проходит ли луч от верхней точки через панель
        if (Physics.Raycast(topRay, out topHit))
        {
            if (IsPointInsidePanel(viewPanel, topHit.point))
            {
                // Верхняя точка проходит через панель, не удаляем платформу
                return;
            }
        }

        // Проверяем, проходит ли луч от нижней точки через панель
        if (Physics.Raycast(botRay, out botHit))
        {
            if (IsPointInsidePanel(viewPanel, botHit.point))
            {
                // Нижняя точка проходит через панель, не удаляем платформу
                return;
            }
        }

        // Если оба луча не проходят через панель, удаляем платформу
        GameController.Destroy(platform);
    }

// Метод для проверки, находится ли точка внутри панели
    private bool IsPointInsidePanel(GameObject viewPanel, Vector3 point)
    {
        RectTransform viewPanelRect = viewPanel.GetComponent<RectTransform>();
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, point);
        return RectTransformUtility.RectangleContainsScreenPoint(viewPanelRect, screenPoint);
    }
}
