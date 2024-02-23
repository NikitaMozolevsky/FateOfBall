

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementService
{
    private static PlatformMovementService _instance;

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
        if (platform) 
        {
            float elapsedTime = 0f;
            Vector3 startPosition = platform.transform.position;
            Vector3 tartgetPosition = GetTargetPositionForPlatform(platform, getTopPosition);
            AnimationCurve platformCurve = getTopPosition ? 
                PlatformController.instance.raiseCurve : PlatformController.instance.dropCurve;

            while (elapsedTime < GameController.instance.DESIRED_DURATION)
            {
                if (!platform)
                {
                    break;
                }
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / GameController.instance.DESIRED_DURATION;
                platform.transform.position = Vector3.Lerp
                    (startPosition, tartgetPosition, platformCurve.Evaluate(percentageComplete));
                yield return null;
            }
            // Платформа поднята, вызов события, добавляющего ее в другой список.
            PlatformGenerationService.onRaisedPlatform?.Invoke(platform);
        }
    }
    
    private Vector3 GetTargetPositionForPlatform(GameObject platform, bool getTopPosition)
    {
        Vector3 targetPosition;
        if (getTopPosition)
        { // Получение позиции которая выше текущей позиции по Y на Y_DIFFERENCE.
            targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y + GameController.instance.Y_DIFFERENCE, 
                platform.transform.position.z);
        }
        else
        { // Получение позиции которая ниже текущей позиции по Y на Y_DIFFERENCE.
            targetPosition = new Vector3
            (platform.transform.position.x, 
                platform.transform.position.y - GameController.instance.Y_DIFFERENCE, 
                platform.transform.position.z);
        }
        return targetPosition;
    }
}