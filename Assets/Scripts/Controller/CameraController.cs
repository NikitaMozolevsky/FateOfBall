using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }
    public static bool colorChangerActive = false;
    
    public CinemachineFreeLook sphereCamera; // Привязанная к шару
    public CinemachineFreeLook startPositionCamera; // Привязанная к точке возрождения
    //public CinemachineFreeLook loseCamera; // Привязанная к месту проигрыша
    public Color[] colors;
    public Transform losePoint;
    
    // Время за которое которое меняется цвет.
    public const float COLOR_TRANSITION_DURATION = 5f;
    public const float DELAY_BEFORE_STARTING_COLOR_CHANGE = 1f;
    public const float DURATION_CAMERA_CHANGE = 2f;
    public const int HIGH_PRIORITY = 11;
    public const int LOW_PRIORITY = 9;

    // Первое ли это касание
    private bool firstTouch = true;

    private CameraController()
    {
    }

    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    
    private void Start()
    {
        CreateSingleton();
        StartColorChanger();
    }

    private void OnEnable()
    {
        SphereController.onBallCollision += SetPriorityToSphereCamera;
        //ActionPlayButton.onPlay += ColorChanger;
        ActionRestartButton.onRestartGame += SetPriorityToStartPointCamera;
        ActionPlayButton.onPlay += ContinueColorChanger;
        ActionPauseButton.onPauseGame += PauseColorChanger;
        ActionContinueButton.onContinueGame += ContinueColorChanger;
        GameController.onLose += PauseColorChanger;
        GameController.onLose += CreateLosePoint;
        GameController.onLose += FreezeCamera;
        //SphereController.onBallCollision += UnfreezeCamera;
    }

    private void OnDisable()
    {
        SphereController.onBallCollision -= SetPriorityToSphereCamera;
        //ActionPlayButton.onPlay -= ColorChanger; // (StartColorChanger)
        ActionRestartButton.onRestartGame -= SetPriorityToStartPointCamera;
        ActionPlayButton.onPlay -= ContinueColorChanger;
        ActionPauseButton.onPauseGame -= PauseColorChanger;
        ActionContinueButton.onContinueGame -= ContinueColorChanger;
        GameController.onLose -= PauseColorChanger;
        GameController.onLose -= CreateLosePoint;
        GameController.onLose -= FreezeCamera;
        //SphereController.onBallCollision -= UnfreezeCamera;
        
    }

    private void SetPriorityToSphereCamera()
    { // Смена еамеры на привязанную к шару если это первое касание
        if (firstTouch)
        {
            sphereCamera.LookAt = SphereController.instance.sphere.transform;
            sphereCamera.Follow = SphereController.instance.sphere.transform;
            sphereCamera.m_Priority = HIGH_PRIORITY;
            startPositionCamera.m_Priority = LOW_PRIORITY;
            firstTouch = false;
        }
    }

    private void SetPriorityToStartPointCamera()
    { // Смена еамеры на привязанную к точке начала
        sphereCamera.m_Priority = LOW_PRIORITY;
        startPositionCamera.m_Priority = HIGH_PRIORITY;
    }

    private void StartColorChanger()
    {
        StartCoroutine(CameraService.instance.ColorChanger());
    }

    private void PauseColorChanger()
    {
        colorChangerActive = false;
    }

    private void ContinueColorChanger()
    {
        colorChangerActive = true;
    }

    private void FreezeCamera()
    { // Замораживает камеру во время поражения на losePoint
        sphereCamera.LookAt = losePoint;
        sphereCamera.Follow = losePoint;
    }

    /*private void UnfreezeCamera()
    { // 
        sphereCamera.LookAt = SphereController.sphere.transform;
        sphereCamera.Follow = SphereController.sphere.transform;
    }*/

    private void CreateLosePoint()
    {
        losePoint.position = SphereController.instance.sphere.transform.position;
    }
}
