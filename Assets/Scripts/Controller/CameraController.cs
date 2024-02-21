using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }
    private CameraService cameraService = CameraService.instance;

    // Для Fade.
    public Animator animator;
    // mainCamera
    public Camera mainCamera;
    // recordsCamera
    public Camera recordsCamera;
    // Камера привязана к шару.
    public CinemachineFreeLook sphereCamera;
    // Камера привязана к точке на которой приземляется шар при первом касании.
    public CinemachineFreeLook startMundanePositionCamera;
    // Начальная позиция камеры следящей за шаром.
    // Нужна для возвращения камеры в начальное положение.
    public Transform startSphereCameraTransform;
    // Массив цветов которые меняются.
    public Color[] colors;
    


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
    
    // Подписка на события.
    private void SubscribeEvents()
    {
        SphereController.onFirstBallCollision += UpPriorityToSphereCamera;
        ActionPlayButton.onPlay += ContinueColorChanger;
        ActionPauseButton.onPauseGame += PauseColorChanger;
        ActionContinueButton.onContinueGame += ContinueColorChanger;
        GameController.onLose += PauseColorChanger;
        GameController.onLose += FreezeSphereCamera;
        ActionRestartButton.onRestartGame += UpPriorityToMundanePointCamera;
        // GameService.afterRestartGame += cameraService.FollowSphereCamera;
        SphereController.onFirstBallCollision += cameraService.FollowSphereCamera;
        GameController.afterRestartGame += SetSphereCameraStartPosition;
    }

    // Отписка от событий.
    private void UnsubscribeEvents()
    {
        SphereController.onFirstBallCollision -= UpPriorityToSphereCamera;
        ActionPlayButton.onPlay -= ContinueColorChanger;
        ActionPauseButton.onPauseGame -= PauseColorChanger;
        ActionContinueButton.onContinueGame -= ContinueColorChanger;
        GameController.onLose -= PauseColorChanger;
        GameController.onLose -= FreezeSphereCamera;
        ActionRestartButton.onRestartGame -= UpPriorityToMundanePointCamera;
        SphereController.onFirstBallCollision -= cameraService.FollowSphereCamera;
        GameController.afterRestartGame -= SetSphereCameraStartPosition;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void Update()
    {
        ColorChange();
        cameraService.ColorChangeTime();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    // Устанавливает повышенный приоритет для камеры следящей за сферой.
    // Срабатывает при первом касании.
    private void UpPriorityToSphereCamera()
    {
        cameraService.UpPriorityToSphereCamera(sphereCamera, startMundanePositionCamera);
    }

    // Устанавливает повышенный приоритет для камеры следящей за точкой приземления шара.
    // Работает изначально, срабатывает при рестарте.
    private void UpPriorityToMundanePointCamera()
    { 
        cameraService.UpPriorityToMundanePointCamera(sphereCamera, startMundanePositionCamera);
    }

    // Останавливает смену цвета.
    private void PauseColorChanger()
    {
        cameraService.PauseColorChanger();
    }

    // Продолжнает смену цвета.
    private void ContinueColorChanger()
    {
        cameraService.ContinueColorChanger();
    }

    // Замораживает камеру сферы.
    // Срабатывает во время поражения.
    private void FreezeSphereCamera()
    {
        cameraService.OffSphereCamera(sphereCamera);
    }

    // Вызывается после рестарта.
    // Камера следящая за сферой возвращается в изначальное положение.
    private void SetSphereCameraStartPosition()
    {
        cameraService.SetSphereCameraStartPosition(sphereCamera, startSphereCameraTransform);
    }

    // Плавно меняет цвет по индексу.
    private void ColorChange()
    {
        cameraService.ColorChange(mainCamera, colors);
    }
}
