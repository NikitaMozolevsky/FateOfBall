

using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraService
{
    private static CameraService _instance;
    
    // Переменная отвечающая за то разрешается ли менять цвет или нет.
    private bool colorChangerActive = false;
    private float currentTime;
    private int colorIndex;
    // Время за кототое main камера перемещается к другой freeLook камере.
    public const float TIME_TO_SET_CAMERA_POSITION = 2f;
    // Приоритеты камер.
    public const int HIGH_PRIORITY = 11;
    public const int LOW_PRIORITY = 9;
    // Скорость изменения цвета. (совсем не точно)
    public const float COLOR_CHANGE_SPEED = 4;
    // Как долго будет один цвет на экране. (совсем не точно)
    public float ONE_COLOR_DURATION_TIME = 4;

    private CameraService()
    {
    }

    public static CameraService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CameraService();
            }
            return _instance;
        }
    }
    
    // Устанавливает повышенный приоритет для камеры следящей за сферой.
    // Срабатывает при первом касании.
    public void UpPriorityToSphereCamera
        (CinemachineFreeLook sphereCamera, CinemachineFreeLook startMundanePositionCamera)
    {
        // Смена камеры на привязанную к шару если это первое касание.
        // Попробую сделать при помощи события.
        if (SphereService.isFirstCollision)
        {
            sphereCamera.m_Priority = HIGH_PRIORITY;
            startMundanePositionCamera.m_Priority = LOW_PRIORITY;
        }
    }
    
    // Устанавливает повышенный приоритет для камеры следящей за точкой приземления шара.
    // Работает изначально, срабатывает при рестарте.
    public void UpPriorityToMundanePointCamera
        (CinemachineFreeLook sphereCamera, CinemachineFreeLook startMundanePositionCamera)
    {
        // Поднятие приоритета камеры следящей за точкой на которую падает шар.
        sphereCamera.m_Priority = LOW_PRIORITY;
        startMundanePositionCamera.m_Priority = HIGH_PRIORITY;
    }

    // Останавливает слежку за сферой.
    // Срабатывает во время поражения.
    public void OffSphereCamera(CinemachineFreeLook sphereCamera)
    {
        sphereCamera.LookAt = null;
        sphereCamera.Follow = null;
    }

    // Начинает слежку за сферой.
    // Срабатывает при рестарте.
    public void FollowSphereCamera()
    {
        CameraController.instance.sphereCamera.LookAt = SphereController.instance.sphere.transform;
        CameraController.instance.sphereCamera.Follow = SphereController.instance.sphere.transform;
    }

    // Останавливает смену цвета.
    public void PauseColorChanger()
    {
        colorChangerActive = false;
    }

    // Продолжнает смену цвета.
    public void ContinueColorChanger()
    {
        colorChangerActive = true;
    }
    
    // Вызывается после рестарта.
    // Камера следящая за сферой возвращается в изначальное положение.
    public void SetSphereCameraStartPosition
        (CinemachineFreeLook sphereCamera, Transform sphereCameraStartTransform)
    {
        sphereCamera.transform.position = sphereCameraStartTransform.position;
    }
    
    public void ColorChange(Camera mainCamera, Color[] colors)
    {
        if (GameService.playCondition)
        {
            // Плавно меняет цвет по индексу.
            mainCamera.backgroundColor = Color.Lerp
                (mainCamera.backgroundColor, colors[colorIndex], COLOR_CHANGE_SPEED * Time.deltaTime);
        }
    }
    
    // Сбрасывает время для изменения цвета.
    public void ColorChangeTime()
    {
        if (GameService.playCondition)
        {
            if (currentTime <= 0)
            {
                colorIndex++;
                CheckColorIndex();
                currentTime = ONE_COLOR_DURATION_TIME;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }
    
    // Сбрасывает индекс цвета.
    private void CheckColorIndex()
    {
        if (colorIndex >= CameraController.instance.colors.Length)
        {
            colorIndex = 0;
        }
    }
}