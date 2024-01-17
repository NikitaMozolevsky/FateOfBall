using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService
{

    private static PlatformService _instance;
    
    private float elapsedTime;
    

    // Ткущая позиция точки спавнаа
    /*private Vector3 spawnPoint = PlatformController.instance.currentGenerationPoint.position;/*#1#*/
    private bool platformGeneration = true;
    
    // Точка спавна платформы, перемещается в цикле.
    private Vector3 startSpawnPoint;
    
    
    public const float PLATFORM_DISTANCE = 2f;
    
    // Платформы которые поднимаются без коллизии
    public const int OFF_COLLISION_PLATFORM_COUNT = 15;

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
}
