using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    // Единственный экземпляр синглтона
    public static SphereController _instance { get; private set; }

    private SphereService _sphereService = SphereService._instance;

    [SerializeField] private bool _isLeft = true;
    [SerializeField] private float _sphereSpeed = 5f;

    public GameObject _sphere;
    public GameObject _platform;
    public CinemachineFreeLook _cinemachineFreeLook;

    private Rigidbody _sphereRigitbody;

    public float SphereSpeed
    {
        get { return _sphereSpeed; }
        set { _sphereSpeed = value; }
    }

    public bool IsLeft
    {
        get { return _isLeft; }
        set { _isLeft = value; }
    }

    private void Awake()
    {
        CreateSingleton();
        /*InitializeSingletonInOtherClasses();*/
        _sphereRigitbody = _sphere.GetComponent<Rigidbody>();
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
    }/*

    private void InitializeSingletonInOtherClasses() // Инициализация
    {
        SphereService._instance.InitializeSingleton();
    }*/

    private void FixedUpdate()
    {
        if (PlayButton.isPlay)
        {
            MoveSphere();
        }
    }private void Update()
    {
        if (PlayButton.isPlay)
        {
            TouchDetector();
        }
    }

    private void TouchDetector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _sphereService.ToggleBoolean();
        }
    }

    private void MoveSphere()
    {
        if (_sphereRigitbody != null && _platform != null)
        {
            //_sphereService.MoveSphereWithVelocity(_sphereRigitbody, _platform);
            //_sphereService.MoveSphereWithForce(_sphereRigitbody, _platform);
            _sphereService.MoveSphereWithTransform(_sphere, _platform);
        }
        else
        {
            Debug.LogWarning("Rigidbody or platform is null");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        _cinemachineFreeLook.enabled = true;
    }
}
