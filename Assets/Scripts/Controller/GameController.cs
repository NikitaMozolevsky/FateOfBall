

using System;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public static GameController instance { get; private set; }
    private GameService gameService = GameService.instance;
    private LevelService levelService = LevelService.instance;
    
    // Выход за пределы.
    public static UnityAction onLose;
    public static UnityAction onStartGame;
    public static UnityAction afterRestartGame;

    public GameObject desiredObject;
    
    // Максимальное кол-во одинаковых поворотов.
    public int SAME_TURNS_COUNT = 4;
    // Минимальное количество платформ.
    public int PLATFORM_COUNT = 11;
    // Время между поднятиями первых платформ.
    public float TIME_BETWEEN_RAISING_PLATFORM = 0.2f;
    // Как долго будет перемещатся платформа.
    public float DESIRED_DURATION = 1.5f;
    // Расстояние на которое переместится платформа по оси Y.
    public float Y_DIFFERENCE = 45;
    // Время для пропущенных платформ через которое они падают.
    public float TIME_TO_DROP_PLATFORM = 0.2f;
    // Скорость сферы.
    public float SPHERE_SPEED = 0.2f;

    private GameController()
    {
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
    
    private void SubscribeEvents()
    {
        ActionPauseButton.onPauseGame += gameService.StopTime;
        ActionContinueButton.onContinueGame += gameService.ContinueTime;
        ActionPlayButton.onPlay += gameService.ResetOnPlayVariables;
        ActionRestartButton.onRestartGame += gameService.ResetOnRestartVariables;
        ActionRestartButton.onRestartGame += InvokeAfterRestartInvention;
        onLose += gameService.ResetOnLoseVariables;
        afterRestartGame += gameService.ResetAfterRestartVariablesCoroutine;
    }

    private void UnsubscribeEvents()
    {
        ActionPauseButton.onPauseGame -= gameService.StopTime;
        ActionContinueButton.onContinueGame -= gameService.ContinueTime;
        ActionPlayButton.onPlay -= gameService.ResetOnPlayVariables;
        ActionRestartButton.onRestartGame -= gameService.ResetOnRestartVariables;
        ActionRestartButton.onRestartGame -= InvokeAfterRestartInvention;
        onLose -= gameService.ResetOnLoseVariables;
        afterRestartGame -= gameService.ResetAfterRestartVariablesCoroutine;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        gameService.CheckLose();
        levelService.LevelManager();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void StartGame()
    {
        gameService.StartGame();
    }

    private void InvokeAfterRestartInvention()
    {
        StartCoroutine(gameService.InvokeAfterRestartInvention());
    }
}