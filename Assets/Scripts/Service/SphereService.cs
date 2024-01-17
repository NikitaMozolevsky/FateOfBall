using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereService
{
    private static SphereService thisInstance;
    
    private bool isLeft = true;
    private bool obtained = false;
    
    private const float SPHERE_SPEED = 0.2f;
    // Длинна луча который нужне для проверки что есть припятствие для шара
    // Сам радиус шара = 0.5.
    private const float RAY_LENGTH_TO_DETECT_COLLISION = 0.5f;
    public static SphereService instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = new SphereService();
            }
            return thisInstance;
        }
    }

    public void MoveSphereWithTransform(GameObject sphere, GameObject platform)
    {
        Vector3 targetDirection;
        // Проверка наличия препятствия перед шаром
        CheckObtain(sphere, sphere.transform.forward);
        CheckObtain(sphere, sphere.transform.right);

        if (!obtained) // Если препятствия нет - продолжается работа скрипта поворота.
        {
            if (isLeft)
            { // Движение по Z
                targetDirection = sphere.transform.forward;
                sphere.transform.position += targetDirection * SPHERE_SPEED;
                /*sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;*/
            }
            else
            { // Движение по X
                targetDirection = sphere.transform.right;
                sphere.transform.position += targetDirection * SPHERE_SPEED;
                /*sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;*/
            }
        }
    }
    
    public void ToggleBoolean()
    { // Изменение направления
        isLeft = !isLeft;
        Debug.Log("isTouched: " + isLeft);
    }

    public bool SphereOutOfPlatform(GameObject sphere, GameObject platform)
    {
        return sphere.transform.position.y < platform.transform.position.y/* - bottomBorderDistance*/;
    }
    
    public void CheckObtain(GameObject sphere, Vector3 direction)
    {
        // Определение точки начала луча (от центра шара)
        Vector3 rayStart = sphere.transform.position;

        // Пуск луча в указанном направлении
        Ray ray = new Ray(rayStart, direction);

        // Проверка столкновения луча с объектами
        if (Physics.Raycast(ray, sphere.transform.localScale.x / 2))
        {
            // Обнаружено препятствие
            obtained = true;
        }
    }
}
