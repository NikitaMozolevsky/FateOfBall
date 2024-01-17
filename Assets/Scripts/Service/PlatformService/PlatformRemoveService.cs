

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRemoveService
{
    private static PlatformRemoveService _instance;
    private PlatformMovementService pms = PlatformMovementService.instance;
    
    public const float TIME_TO_DESTROY_PLATFORM = 2f;
    public const float TIME_TO_DROP_PLATFORM = 0.2f;
    // На платформе 2 дочерние точки, их можно получить по этим именам.
    public const string TOP_VISIBLE_POINT = "TopVisiblePoint";
    public const string BOT_VISIBLE_POINT = "BotVisiblePoint";
    
    private PlatformRemoveService()
    {
    }

    // Статический метод для получения экземпляра синглтона.
    public static PlatformRemoveService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new PlatformRemoveService();
            }
            return _instance;
        }
    }
    
    private void DeleteFromList(GameObject platform, List<GameObject> platformList)
    {
        platformList.Remove(platform);
    }
    
    public IEnumerator DestroyPlatformThroughTime(GameObject platform)
    { // Удаляет объект через время.
    
        yield return new WaitForSeconds(TIME_TO_DESTROY_PLATFORM);
        GameController.Destroy(platform);
    }
    
    public void CheckMissedPlatformCollision(Collision collision, List<GameObject> platformList)
    { // Удалить объект если по какой-то причине шар не сделал для него коллизии
        // и он остался на месте, обычно такой объект тольок 1.
        
        GameObject platform = collision.gameObject; // 
        int currentPlatformIndex = platformList.IndexOf(platform);
        // Предыдущая платформа.
        GameObject previousPlatform = platformList[currentPlatformIndex - 1];
        Rigidbody previousPlatformRigitbody = previousPlatform.GetComponent<Rigidbody>();
        // Если предыдущая платформа кинематична - она не упала.
        if (previousPlatformRigitbody.isKinematic)
        { // Уронить и удалить платформу.
            RemovePlatformManually(previousPlatform);
        }
    }
    
    public void CheckMissedPlatformCollisionGPT(Collision collision, List<GameObject> platformList)
    {
        if (GameController.playCondition)
        {
            GameObject platform = collision.gameObject;
            int currentPlatformIndex = platformList.IndexOf(platform);

            // Проверяем текущую и предыдущие платформы до тех пор, пока не найдем первую некинематичную платформу.
            for (int i = currentPlatformIndex; i >= 0; i--)
            {
                GameObject currentOrPreviousPlatform = platformList[i];
                Rigidbody platformRigidbody = currentOrPreviousPlatform.GetComponent<Rigidbody>();

                // Если платформа не кинематична - она не упала.
                if (!platformRigidbody.isKinematic)
                {
                    break; // Прерываем цикл, так как нашли первую некинематичную платформу.
                }

                // Уронить и удалить кинематичную платформу с рандомной задержкой.
                GameController.instance.StartCoroutine
                (RemovePlatformManuallyWithRandomDelay
                    (currentOrPreviousPlatform, TIME_TO_DROP_PLATFORM));
            }
        }
    }
    
    public void RemovePlatformManually(GameObject platform)
    { 
        // Для того что бы уронить и удалить платформу.
        GameController.instance.StartCoroutine(pms.DropPlatform(platform));
        GameController.instance.StartCoroutine(DestroyPlatformThroughTime(platform));
    }
    
    public IEnumerator RemovePlatformManuallyWithRandomDelay(GameObject platform, float delay)
    { 
        // Удалить из листа.
        DeleteFromList(platform, PlatformController.instance.platformList);
        // Удаление с рандомной задержкой.
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
}