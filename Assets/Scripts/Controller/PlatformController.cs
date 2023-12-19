using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static PlatformController _instance { get; private set; }
    private PlatformController() {} // Для Singleton
    
    public GameObject _platform;
    public GameObject _firstPlatform;

    private PlatformService _platformService = PlatformService.Instance;

    private void Awake()
    {
        CreateSingleton();
        /*InitializeSingletonInOtherClasses();*/
    }
    
    /*private void InitializeSingletonInOtherClasses() // Инициализация
    {
        SphereService._instance.InitializeSingleton();
    }*/
    
    private void CreateSingleton()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (PlayButton.isPlay)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _platformService.FallPlatform(_platform);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                /*_platformService.UpPlatform(_platform);*/
            }
        }
    }
}
