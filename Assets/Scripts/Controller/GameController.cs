

using System;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public static GameController instance { get; private set; }
    public static Action onLose; // Выход за пределы.
    public static bool playCondition = false;
    public static bool loseCondition = false;

    private int stopTime = 0;
    private int continueTime = 1;

    private void Awake()
    {
        CreateSingleton();
    }
    
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        ActionPauseButton.onPauseGame += PauseOn;
        ActionContinueButton.onContinueGame += PauseOff;
        ActionPlayButton.onPlay += Play;
        ActionRestartButton.onRestartGame += ResetBoolVariables;
    }

    private void OnDisable()
    {
        ActionPauseButton.onPauseGame -= PauseOn;
        ActionContinueButton.onContinueGame -= PauseOff;
        ActionPlayButton.onPlay -= Play;
        onLose -= ResetBoolVariables;
    }
    
    private void Update()
    {
        OnLose();
    }

    private void PauseOn()
    {
        Time.timeScale = stopTime;
    }

    private void PauseOff()
    {
        Time.timeScale = continueTime;
    }

    private void Play()
    {
        playCondition = true;
    }
    
    private void OnLose()
    {
        if (SphereController.instance.SphereOutOfPlatform() && !loseCondition)
        {
            onLose?.Invoke();
            loseCondition = true;
            Debug.Log("Lose!");
        }
    }

    private void ResetBoolVariables()
    { // Устанавливает переменным значения которые были во время запуска игры
        // Для того что бы начать игру заново
        PlatformService.firstPlatformsRaised = false;
    }
}