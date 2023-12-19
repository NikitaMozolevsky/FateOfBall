using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController _instance { get; private set; }
    
    private CinemachineFreeLook _cinemachineFreeLook;
    
    public Rigidbody _sphereRigitbody;
    
    private void Awake()
    {
        CreateSingleton();
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    
    
    /*private void InitializeSingletonInOtherClasses() // Инициализация
    {
        SphereService._instance.InitializeSingleton();
    }*/
    
}
