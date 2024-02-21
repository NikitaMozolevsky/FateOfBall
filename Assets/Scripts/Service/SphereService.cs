using UnityEngine;

public class SphereService
{
    private static SphereService _instance;

    // Отвечает за движение шара.
    public static bool sphereMovement = false;
    // Отвечает за поворот.
    public static bool isLeft = true;
    // Отвечает за первое столкновение.
    public static bool isFirstCollision = true;
    // Было ли боковое столкновение луча сферы с платформой. 
    public static bool sphereRayTouchedPlatfrom = false;
    // Скорость сферы.
    private const float SPHERE_SPEED = 0.2f;
    // Точка по Y ниже ниже которой для шара фиксируется поражение.
    private const float Y_LOSE_POSITION = -3;
    public static SphereService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SphereService();
            }
            return _instance;
        }
    }

    // Движет сферу при sphereMovement = true.
    public void MoveSphere(GameObject sphere)
    {
        if (sphereMovement)
        {
            Vector3 targetDirection;
            // Проверка наличия препятствия перед шаром
            CheckTouchObstacle(sphere, sphere.transform.forward);
            CheckTouchObstacle(sphere, sphere.transform.right);

            if (!sphereRayTouchedPlatfrom) // Если препятствия нет - продолжается работа скрипта поворота.
            {
                if (isLeft)
                {
                    // Движение по Z
                    targetDirection = sphere.transform.forward;
                    sphere.transform.position += targetDirection * SPHERE_SPEED;
                }
                else
                {
                    // Движение по X
                    targetDirection = sphere.transform.right;
                    sphere.transform.position += targetDirection * SPHERE_SPEED;
                }
            }
        }
    }
    
    public void ToggleBoolean()
    { // Изменение направления
        isLeft = !isLeft;
    }

    // Возвращает true если шар оказался ниже определенного уровня.
    public static bool SphereOutOfPlatform()
    {
        Vector3 currentSpherePosition = 
            SphereController.instance.sphere.transform.localPosition;
        return currentSpherePosition.y < Y_LOSE_POSITION;
    }
    
    public void CheckTouchObstacle(GameObject sphere, Vector3 direction)
    {
        // Определение точки начала луча (от центра шара)
        Vector3 rayStart = sphere.transform.position;

        // Пуск луча в указанном направлении
        Ray ray = new Ray(rayStart, direction);

        // Проверка столкновения луча с объектами
        if (Physics.Raycast(ray, sphere.transform.localScale.x / 2))
        {
            // Обнаружено препятствие
            sphereRayTouchedPlatfrom = true;
        }
    }
    
    public void SetSphereStartPosition(GameObject sphere, Transform sphereStartPosition)
    {
        // Для остановки шарика
        sphere.GetComponent<Rigidbody>().isKinematic = true;
        sphere.GetComponent<Rigidbody>().isKinematic = false;
        
        sphere.transform.position = sphereStartPosition.position;
    }

    // Столкновение шара с платформой.
    public void OnCollisionEnter(Collision collision)
    {
        // Если с платформой происходит в первый раз.
        if (isFirstCollision)
        {
            SphereController.onFirstBallCollision?.Invoke();
            isFirstCollision = false;
        }
        else
        {
            SphereController.onBallCollision?.Invoke(collision);
        }
    }

    // Отслеживает касания по экрану для управления шаром.
    public void ScreenTouchManager()
    {
        if (GameService.playCondition)
        {
            // Проверяем, есть ли касания на сенсорном экране
            if (Input.touchCount > 0)
            {
                // Перебираем все касания
                for (int i = 0; i < Input.touchCount; i++)
                {
                    // Получаем текущее касание
                    Touch touch = Input.GetTouch(i);

                    // Проверяем, было ли начало касания
                    if (touch.phase == TouchPhase.Began)
                    {
                        // Меняем направление шара на противоположное
                        ToggleBoolean();
                        // Звук смены направления.
                        ChangeDirectionSound();
                    }
                }
            }
        }
    }

    private void ChangeDirectionSound()
    {
        GameController.instance.audioSource.PlayOneShot(SphereController.instance.clickSound);
    }
}
