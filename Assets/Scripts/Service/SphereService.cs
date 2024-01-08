using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereService
{
    private static SphereService thisInstance;
    // Статический метод для получения экземпляра синглтона

    private float bottomBorderDistance;
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
    
        if (SphereController.instance.IsLeft)
        {
            targetDirection = platform.transform.forward;
            sphere.transform.position += targetDirection * SphereController.instance.SphereSpeed;
        }
        else
        {
            targetDirection = platform.transform.right;
            sphere.transform.position += targetDirection * SphereController.instance.SphereSpeed;
        }
    }
    
    public void ToggleBoolean()
    { // Изменение направления
        SphereController.instance.IsLeft = !SphereController.instance.IsLeft;
        Debug.Log("isTouched: " + SphereController.instance.IsLeft);
    }

    public bool SphereOutOfPlatform(GameObject sphere, GameObject platform)
    {
        return sphere.transform.position.y < platform.transform.position.y - bottomBorderDistance;
    }
}
