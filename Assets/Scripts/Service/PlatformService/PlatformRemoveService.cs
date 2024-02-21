

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformRemoveService
{
    private static PlatformRemoveService _instance;
    private PlatformMovementService pms = PlatformMovementService.instance;
    private Util util = Util.instance;

    // Время через которое уничтожится платформа после падения.
    public const float TIME_TO_DESTROY_PLATFORM = 1f;
    // Время для пропущенных платформ через которое они падают.
    public const float TIME_TO_DROP_PLATFORM = 0.2f;
    // Диапазон времени в котором все видимые и поднятые платформы роняются.
    public const float TIME_TO_DROP_ALL_PLATFORMS = 1F;
    
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
    
    public IEnumerator DestroyPlatformThroughTime(GameObject platform)
    { // Удаляет объект через время.
    
        yield return new WaitForSeconds(TIME_TO_DESTROY_PLATFORM);
        GameController.Destroy(platform);
    }

    public void RemovePlatformManually(GameObject platform)
    {
        // GameController.instance.StartCoroutine(pms.DropPlatform(platform));
        GameController.instance.StartCoroutine(DestroyPlatformThroughTime(platform));
    }
    
    public IEnumerator RemovePlatformManuallyWithRandomDelay(GameObject platform, float delay)
    {
        // Удаление с рандомной задержкой.
        yield return new WaitForSeconds(Random.Range(0f, delay));
        RemovePlatformManually(platform);
    }

    public void ClearDroppedList(List<GameObject> droppedList)
    {
        Debug.Log("lsit cleared");
        droppedList.Clear();
        Debug.Log("lsit cleared");
    }

    // Возвращает значение (Видна ли платформа?)
    public bool IsPlatformVisible(GameObject platform)
    {

        if (platform)
        {
            Camera camera = Camera.main;
            Plane[] cameraFrustrum;
            Collider platformCollider = platform.GetComponent<Collider>();
         
            var bounds = platformCollider.bounds;
            cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(cameraFrustrum, bounds);
        }
        // Вызывает ошибку обращения к уже уничтоженному объекту,
        // поэтому такая заглушка.
        return false;
    }

    // Уничтожает сброшенные и вышедшие за камеру платформы.
    public void InvisibleDroppedPlatformDestroyer(List<GameObject> droppedPlatformList)
    {
        foreach (var platform in droppedPlatformList)
        {
            DesroyPlatformIfInvisible(platform);
        }
    }
    
    // Работает в цикле.
    // Уничтожает одну платформу если та за пределами камеры.
    public void DesroyPlatformIfInvisible(GameObject platform)
    {
        if (!IsPlatformVisible(platform))
        {
            GameController.Destroy(platform);
        }
    }

    // Вызывается при поражении. Роняет все платформы которые видны.
    // Принимает в качестве параметра список raised.
    // Роняет с рандомной задержкой используя IEnumerator.
    public void DropAllRaisedPlatfromsWithRandomDelay
        (List<GameObject> raisedPlatformList)
    {
        foreach (var platform in raisedPlatformList)
        {
            // Роняет платформу с рандомной задержкой.
            GameController.instance.StartCoroutine(DropPlatformWithRandomDelay(platform));
        }
    }

    // Производит действия при соприкосновении шара с платформой.
    public void CollisionWithPlatform
        (Collision collision, List<GameObject> raisedPlatformList, 
            List<GameObject> droppedPlatformList)
    {
        if (GameService.playCondition)
        {
            GameObject platform = collision.gameObject;
            // Роняем платформу.
            // DropPlatform(platform);
            // Получает список всех пройденных платформ (в том числе текущую).
            List<GameObject> passedPlatforms = GetPassedPlatforms
                (platform, raisedPlatformList);
            // Платформы переходят в список droppedPlatforms которые удаляются при 
            // исчезновении с экрана.
            MarkPassedPlatformsAsDropped
                (passedPlatforms, raisedPlatformList, droppedPlatformList);
            // Роняет платформы.
            DropPlatformsList(passedPlatforms);    
        }
    }

    // Возвращает списое из пройденных платформ (чаще всего одну).
    private List<GameObject> GetPassedPlatforms
        (GameObject currentPlatform, List<GameObject> raisedPlatformList)
    {
        // Индекс текущей платформы в списке.
        int currentIndex = raisedPlatformList.IndexOf(currentPlatform);
        // Вернуть подсписок, начиная с первой платформы и до текущей платформы включительно.
        return raisedPlatformList.GetRange(0, currentIndex + 1);
    }

    // Записывает пройденные платформы из raised в dropped список.
    private void MarkPassedPlatformsAsDropped
        (List<GameObject> passed, List<GameObject> raised, List<GameObject> dropped)
    {
        // Переместить все платформы из списка passed в список dropped.
        dropped.AddRange(passed);

        // Очистить список raised от платформ, которые были перемещены в dropped.
        foreach (var platform in passed)
        {
            raised.Remove(platform);
        }
    }

    // Вызывается при поражении, после фактического сброса всех платформ.
    // Перемещает все платформы в список сброшенных для возможности их удаления.
    public void AssignAllRaisedPlatfornsAsDroppedV1
        (List<GameObject> raised, List<GameObject> dropped)
    {
        dropped.AddRange(raised);
        foreach (var platform in raised)
        {
            raised.Remove(platform);
        }
    }
    
    // Вызывается при поражении, после фактического сброса всех платформ.
    // Перемещает все платформы в список сброшенных для возможности их удаления.
    public void AssignAllRaisedPlatfornsAsDropped
        (List<GameObject> raised, List<GameObject> dropped)
    {
        // Итерируем в обратном порядке, чтобы избежать ошибки при удалении
        for (int i = raised.Count - 1; i >= 0; i--)
        {
            GameObject platform = raised[i];
            dropped.Add(platform);
            raised.RemoveAt(i);
        }
    }
    
    // Роняет все платформы в списке.
    // Если в списке больше чем 1 платформа - все кроме первой платформы
    // роняются с рандомной задержкой перед падением.
    private void DropPlatformsList(List<GameObject> passedPlatforms)
    {
        foreach (var platform in passedPlatforms)
        {
            // Первый элемент роняется с постоянной задержкой что-бы не мешать движению шара.
            if (platform == passedPlatforms[0])
            {
                GameController.instance.StartCoroutine
                    (DropPlatformWithDelay(platform));
            }
            else
            {
                GameController.instance.StartCoroutine
                    (DropPlatformWithRandomDelay(platform));
            }
        }
    }

    // Метод который запускает куратину которая опускает платформу в низ.
    public void DropPlatform(GameObject platform)
    {
        GameController.instance.StartCoroutine
            (pms.MovePlatformTowardTarget(platform, false));
        // Вызывается при падении платформы.
        // Нужени для егнерации и счетчика очков.
        PlatformController.onDropPlatform?.Invoke();
    }
    
    // Метод который запускает куратину которая опускает платформу в низ.
    // Срабатывает с задержкой.
    public IEnumerator DropPlatformWithDelay(GameObject platform)
    {
        yield return new WaitForSeconds(TIME_TO_DROP_PLATFORM);
        DropPlatform(platform);
    }

    public IEnumerator DropPlatformWithRandomDelay(GameObject platform)
    {
        float randomTimeToDropPlatform = util.GetRandomFloatInRange(TIME_TO_DROP_PLATFORM);
        yield return new WaitForSeconds(randomTimeToDropPlatform);
        DropPlatform(platform);
    }
}