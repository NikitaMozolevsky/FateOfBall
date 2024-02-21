

using System;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public static GameController instance { get; private set; }
    private GameService gameService = GameService.instance;
    
    // Выход за пределы.
    public static UnityAction onLose;
    public static UnityAction onStartGame;
    public static UnityAction afterRestartGame;

    // Для воспроизведения звука.
    public AudioSource audioSource;

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
        Util.instance.FindObjectsWithComponent<SoundController>();
    }

    private void Update()
    {
        gameService.CheckLose();
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