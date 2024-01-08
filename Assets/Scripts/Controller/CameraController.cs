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
    
    public float transitionDuration = 5f;
    public int highPriority = 11;
    public int lowPriority = 9;
    
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
        SphereController.onBallCollision += UnfreezeCamera;
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
        SphereController.onBallCollision -= UnfreezeCamera;
        
    }

    private void SetPriorityToSphereCamera()
    { // Смена еамеры на привязанную к шару
        sphereCamera.m_Priority = highPriority;
        startPositionCamera.m_Priority = lowPriority;
        
    }

    private void SetPriorityToStartPointCamera()
    { // Смена еамеры на привязанную к точке начала
        sphereCamera.m_Priority = lowPriority;
        startPositionCamera.m_Priority = highPriority;
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
    {
        sphereCamera.LookAt = losePoint;
        sphereCamera.Follow = losePoint;
    }

    private void UnfreezeCamera()
    {
        sphereCamera.LookAt = SphereController.sphere.transform;
        sphereCamera.Follow = SphereController.sphere.transform;
    }

    private void CreateLosePoint()
    {
        losePoint.position = SphereController.sphere.transform.position;
    }
}
