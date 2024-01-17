

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementService
{
    private static PlatformMovementService _instance;
    
    // Как долго будет перемещатся платформа.
    public const float DESIRED_DURATION = 1f;
    // Расстояние на которое переместится платформа по оси Y.
    public const float Y_DIFFERENCE = 45;
    // Колическтво платформ которые поднимаутся плавно.
    public const int SMOOTHLY_RAISED_PLATFORMS = 30;
    public const float DELAY_BETWEEN_RAISING_PLATFORMS = 0.1f;
    
    
    private PlatformMovementService()
    {
    }

    // Статический метод для получения экземпляра синглтона
    public static PlatformMovementService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new PlatformMovementService();
            }
            return _instance;
        }
    }
    
    // Плавно перемещает платформу к targetPosition.
    // targetPosition зависит от getTopPosition.
    public IEnumerator MovePlatformTowardTarget(GameObject platform, bool getTopPosition)
    {
        // Рандомная задержка в пределах 1 секунды
        /*yield return new WaitForSeconds(Random.Range(0f, DESIRED_DURATION));*/

        float elapsedTime = 0f;
        Vector3 startPosition = platform.transform.position;
        Vector3 tartgetPosition = GetTargetPositionForPlatform(platform, getTopPosition);
        AnimationCurve platformCurve = getTopPosition ? 
            PlatformController.instance.raiseCurve : PlatformController.instance.dropCurve;

        while (elapsedTime < DESIRED_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / DESIRED_DURATION;
            platform.transform.position = Vector3.Lerp
                (startPosition, tartgetPosition, platformCurve.Evaluate(percentageComplete));
            yield return null;
        }
    }
    
    // Перемещает платформу моментально (на Y_DIFFERENCE вверх).
    public void TeleportPlatformTowardTarget(GameObject platform, bool getTopPosition)
    {
        platform.transform.position = GetTargetPositionForPlatform(platform, getTopPosition);
    }
    
    // Возвращает верхнюю либо нижнюю позицию для каждой платформы в зависимости от bool
    private Vector3 GetTargetPositionForPlatformV1(GameObject platform, bool topPosition)
    { // Если целевая позиция верхняя позиция платформы, то Y_DIFFERENCE положиетелен, и наоборот.
        float yTargetPosition = Y_DIFFERENCE;
        yTargetPosition *= topPosition ? 1f : -1f;
        
        Vector3 targetPosition = new Vector3
        (platform.transform.position.x, 
            platform.transform.position.y + yTargetPosition, 
            platform.transform.position.z);
        
        return targetPosition;
    }
    
    private Vector3 GetTargetPositionForPlatform(GameObject platform, bool getTopPosition)
    {
        Vector3 targetPosition;
        if (getTopPosition)
        { // Получение позиции которая выше текущей позиции по Y на Y_DIFFERENCE.
            targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y + Y_DIFFERENCE, 
                platform.transform.position.z);
        }
        else
        { // Получение позиции которая ниже текущей позиции по Y на Y_DIFFERENCE.
            targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y - Y_DIFFERENCE, 
                platform.transform.position.z);
        }
        return targetPosition;
    }
    
    public IEnumerator RaiseFirstPlatforms(List<GameObject> platformList)
    { // Поднимает первые 20 платформ, остальные по идее уже должны быть в верху.
        // Теперь все платформы создаются в верху, а затем первые сразу резко
        // опускаются в низ.
        for (int i = 0; i < SMOOTHLY_RAISED_PLATFORMS; i++)
        { // Плавно поднимает платформу
            yield return new WaitForSeconds(DELAY_BETWEEN_RAISING_PLATFORMS);
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
    
    public IEnumerator DropPlatform(GameObject platform)
    { // Роняет платформу
        
        yield return new WaitForSeconds(PlatformRemoveService.TIME_TO_DROP_PLATFORM);
        // Сила притяжения
        /*OffPlatformKinematic(platform);*/
        // Слишком быстро падает, мешает игре если поднимать
        GameController.instance.StartCoroutine
            (MovePlatformTowardTarget(platform, false));
        // Cj,snbt
        PlatformController.onDropPlatform?.Invoke();
    }
}