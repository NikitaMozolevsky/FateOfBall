

using System.Collections;
using UnityEngine;

public class GameService
{
    
    private static GameService _instance;
    
    // Становится true при нажатии play.
    public static bool playCondition = false;
    public static bool loseCondition = false;
    // Маркер генерации платформ.
    public bool platformGeneration = true;
    // Переменные для управлением временем.
    private int stopTime = 0;
    private int continueTime = 1;
    private GameService()
    {
    }

    public static GameService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameService();
            }
            return _instance;
        }
    }
    
    public void StopTime()
    {
        Time.timeScale = stopTime;
    }

    public void ContinueTime()
    {
        Time.timeScale = continueTime;
    }
    
    public void CheckLose()
    {
        // Поражение.
        if (SphereService.SphereOutOfPlatform() && !loseCondition)
        {
            Debug.Log("Lose!");
            GameController.onLose?.Invoke();
        }
    }

    public void StartGame()
    {
        GameController.onStartGame?.Invoke();
    }
    
    // Устанавливает bool переменным соответствующие значения.
    public void ResetOnPlayVariables()
    { 
        Debug.Log("Play");
        
        playCondition = true;
        SphereService.sphereMovement = true;    
    }

    // Когда камера в начальном положении - вызывается событие.
    public IEnumerator InvokeAfterRestartInvention()
    {
        // Время до перехода на новую камеру.
        yield return new WaitForSeconds(CameraService.TIME_TO_SET_CAMERA_POSITION);
        GameController.afterRestartGame?.Invoke();
    }

    // Устанавливает переменным соответствующие значения.
    public void ResetOnRestartVariables()
    {
        // Для появления кнопки Play устанавливаем что еще не было касания патформы.
        SphereService.isFirstCollision = true;
        // Установить поворот при начале игры на лево.
        SphereService.isLeft = true;
    }

    // Изменяет переменные когда камера в стартовом положении
    public void ResetAfterRestartVariablesCoroutine()
    {
        loseCondition = false;
        // Устанавливает bool переменную отвечающую за ренерацию плтформа.
        platformGeneration = true;
        SphereService.sphereMovement = false;
        SphereService.sphereRayTouchedPlatfrom = false;
        PlatformGenerationService.firstTwoPlatformsCreated = false;
    }
    
    // Устанавливает переменным соответствующие значения.
    public void ResetOnLoseVariables()
    { 
        // Устанавливает bool переменную отвечающую за ренерацию плтформа.
        platformGeneration = false;
        playCondition = false;
        loseCondition = true;
    }
}