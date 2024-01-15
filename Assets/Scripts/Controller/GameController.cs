

using System;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public static GameController instance { get; private set; }
    public static Action onLose; // Выход за пределы.
    // Становится true при нажатии play.
    public static bool playCondition = false;
    public static bool loseCondition = false;
    public static bool menuCondition = true;
    public static bool pauseCondition = false;

    private int stopTime = 0;
    private int continueTime = 1;

    private GameController()
    {
    }

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
        ActionPlayButton.onPlay += ResetOnPlayBoolVariables;
        ActionRestartButton.onRestartGame += ResetOnRestartBoolVariables;
        onLose -= ResetOnLoseBoolVariables;
        
    }

    private void OnDisable()
    {
        ActionPauseButton.onPauseGame -= PauseOn;
        ActionContinueButton.onContinueGame -= PauseOff;
        ActionPlayButton.onPlay += ResetOnPlayBoolVariables;
        ActionRestartButton.onRestartGame += ResetOnRestartBoolVariables;
        onLose -= ResetOnLoseBoolVariables;
        
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
    
    private void OnLose()
    {
        if (SphereController.instance.SphereOutOfPlatform() && !loseCondition)
        {
            onLose?.Invoke();
            loseCondition = true;
            Debug.Log("Lose!");
        }
    }

    private void ResetOnPlayBoolVariables()
    { // Устанавливает bool переменным соответствующие значения
        playCondition = true;
    }

    private void ResetOnRestartBoolVariables()
    { // Устанавливает bool переменным соответствующие значения
        
    }
    
    private void ResetOnLoseBoolVariables()
    { // Устанавливает bool переменным соответствующие значения
        
    }
}