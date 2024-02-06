﻿

    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class GameService
    {
        
        private static GameService _instance;
        private SphereService sphereService = SphereService.instance;
        
        // Выход за пределы.
        public static UnityAction onLose;
        public static UnityAction onStartGame;
        public static UnityAction afterRestartGame;
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
        if (sphereService.SphereOutOfPlatform() && !loseCondition)
        {
            Debug.Log("Lose!");
            onLose?.Invoke();
        }
    }

    public void StartGame()
    {
        onStartGame?.Invoke();
    }
    
    // Устанавливает bool переменным соответствующие значения.
    public void ResetOnPlayVariables()
    { 
        Debug.Log("Play");
        
        playCondition = true;
        sphereService.sphereMovement = true;
    }

    // Когда камера в начальном положении - вызывается событие.
    public IEnumerator InvokeAfterRestartInvention()
    {
        // Время до перехода на новую камеру.
        yield return new WaitForSeconds(CameraService.TIME_TO_SET_CAMERA_POSITION);
        afterRestartGame?.Invoke();
    }

    // Устанавливает переменным соответствующие значения.
    public void ResetOnRestartVariables()
    {
        // Для появления кнопки Play устанавливаем что еще не было касания патформы.
        sphereService.isFirstCollision = true;
        // Установить поворот при начале игры на лево.
        sphereService.isLeft = true;
    }

    // Изменяет переменные когда камера в стартовом положении
    public void ResetAfterRestartVariablesCoroutine()
    {
        loseCondition = false;
        // Устанавливает bool переменную отвечающую за ренерацию плтформа.
        platformGeneration = true;
        sphereService.sphereMovement = false;
        sphereService.sphereRayTouchedPlatfrom = false;
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