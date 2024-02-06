

using System;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public static GameController instance { get; private set; }
    private GameService gameService = GameService.instance;

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
    
    private void OnEnable()
    {
        ActionPauseButton.onPauseGame += gameService.StopTime;
        ActionContinueButton.onContinueGame += gameService.ContinueTime;
        ActionPlayButton.onPlay += gameService.ResetOnPlayVariables;
        ActionRestartButton.onRestartGame += gameService.ResetOnRestartVariables;
        ActionRestartButton.onRestartGame += InvokeAfterRestartInvention;
        GameService.onLose += gameService.ResetOnLoseVariables;
        GameService.afterRestartGame += gameService.ResetAfterRestartVariablesCoroutine;
    }

    private void OnDisable()
    {
        ActionPauseButton.onPauseGame -= gameService.StopTime;
        ActionContinueButton.onContinueGame -= gameService.ContinueTime;
        ActionPlayButton.onPlay -= gameService.ResetOnPlayVariables;
        ActionRestartButton.onRestartGame -= gameService.ResetOnRestartVariables;
        ActionRestartButton.onRestartGame -= InvokeAfterRestartInvention;
        GameService.onLose -= gameService.ResetOnLoseVariables;
        GameService.afterRestartGame -= gameService.ResetAfterRestartVariablesCoroutine;
    }

    private void Awake()
    {
        CreateSingleton();
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        gameService.CheckLose();
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