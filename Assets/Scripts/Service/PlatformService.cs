using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService
{
    
    public const float PLATFORM_IS_KINEMATIC = 0;
    public const float PLATFORM_IS_FALIING = 1;
    public const float PLATFORM_IS_UPPING = 2;
    
    public float _platformState = 0;
    public float desiredDuration = 3f;
    public AnimationCurve curve;
    
    private static PlatformService _instance;
    private float fallToY = -10;
    private float elapsedTime;

    // Статический метод для получения экземпляра синглтона
    public static PlatformService Instance
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

    public void FallPlatform(GameObject platform)
    {
        platform.GetComponent<Rigidbody>().isKinematic = false;
        if (platform.transform.position.y < fallToY)
        {
            platform.GetComponent<Rigidbody>().isKinematic = true;
            platform.SetActive(false);
        }
        Debug.Log("A Pressed");
    }
}
