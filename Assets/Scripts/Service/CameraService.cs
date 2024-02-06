

using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraService
{
    private static CameraService _instance;
    private SphereService sphereService = SphereService.instance;
    
    // Переменная отвечающая за то разрешается ли менять цвет или нет.
    private bool colorChangerActive = false;
    
    // Время за которое которое меняется цвет.
    public const float COLOR_TRANSITION_DURATION = 5f;
    // Задержка перед началом изменения цвета на новый.
    public const float DELAY_BEFORE_STARTING_COLOR_CHANGE = 1f;
    // Время за кототое main камера перемещается к другой freeLook камере.
    public const float TIME_TO_SET_CAMERA_POSITION = 2f;
    // Приоритеты камер.
    public const int HIGH_PRIORITY = 11;
    public const int LOW_PRIORITY = 9;

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

    // Куратина смены фона.
    public IEnumerator ColorChanger()
    {
        while (GameService.playCondition)
        {
            foreach (Color targetColor in CameraController.instance.colors)
            {
                float t = 0f;
                Color startColor = Camera.main.backgroundColor;

                while (t < 1f && colorChangerActive)
                {
                    t += Time.deltaTime / COLOR_TRANSITION_DURATION;
                    Camera.main.backgroundColor = Color.Lerp(startColor, targetColor, t);
                    yield return null;
                }

                // Ждем перед следующим цветом
                yield return new WaitForSeconds(DELAY_BEFORE_STARTING_COLOR_CHANGE);
            }
        }
    }
    
    // Устанавливает повышенный приоритет для камеры следящей за сферой.
    // Срабатывает при первом касании.
    public void UpPriorityToSphereCamera
        (CinemachineFreeLook sphereCamera, CinemachineFreeLook startMundanePositionCamera)
    {
        // Смена камеры на привязанную к шару если это первое касание.
        // Попробую сделать при помощи события.
        if (sphereService.isFirstCollision)
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
}