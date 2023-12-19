using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereService
{
    private static SphereService _thisInstance;
    // Статический метод для получения экземпляра синглтона
    public static SphereService _instance
    {
        get
        {
            if (_thisInstance == null)
            {
                _thisInstance = new SphereService();
            }
            return _thisInstance;
        }
    }

    public void MoveSphereWithTransform(GameObject sphere, GameObject platform)
    {
        Vector3 targetDirection;
    
        if (SphereController._instance.IsLeft)
        {
            targetDirection = platform.transform.forward;
            sphere.transform.position += targetDirection * SphereController._instance.SphereSpeed;
        }
        else
        {
            targetDirection = platform.transform.right;
            sphere.transform.position += targetDirection * SphereController._instance.SphereSpeed;
        }
    }
    
    public void ToggleBoolean()
    { // Изменение направления
        SphereController._instance.IsLeft = !SphereController._instance.IsLeft;
        Debug.Log("isTouched: " + SphereController._instance.IsLeft);
    }
}
