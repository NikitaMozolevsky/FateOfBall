

using System;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController instance { get; private set; }
    
    private ScoreService scoreService = ScoreService.instance;
    // Правый верхний счетчик.
    public TextMeshProUGUI currentSchoreCountTMP;
    // Центральная надпись, трансформ.
    public Transform yourSchoreTransform;
    // Центральный счетчик.
    public TextMeshProUGUI yourScoreCountTMP;
    // Топ результаты.
    public TextMeshProUGUI firstPlaceText;
    public TextMeshProUGUI secondPlaceText;
    public TextMeshProUGUI thirdPlaceText;
    public TextMeshProUGUI fourthPlaceText;
    public TextMeshProUGUI fifthPlaceText;

    private ScoreController() {}
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
    
    private void SubscribeEvents()
    {
        PlatformController.onDropPlatform += scoreService.AddSchore;
        ActionRestartButton.onRestartGame += scoreService.ResetCurrentCounter;
        ActionPlayButton.onPlay += ShowCurrentSchore;
        ActionPauseButton.onPauseGame += scoreService.SetNewScore;
        GameController.onLose += HideCurrentSchore;
        GameController.onLose += SetYourSchoreCount;
        GameController.onLose += ShowYourSchore;
        GameController.onLose += scoreService.SetNewScore;
        GameController.onLose += scoreService.SetScorePrefs;
        GameController.onLose += SetTextToRecords;
        ActionRestartButton.onRestartGame += HideYourSchore;
    }

    private void UnsubscribeEvents()
    {
        PlatformController.onDropPlatform -= scoreService.AddSchore;
        ActionRestartButton.onRestartGame -= scoreService.ResetCurrentCounter;
        ActionPlayButton.onPlay -= ShowCurrentSchore;
        ActionPauseButton.onPauseGame -= scoreService.SetNewScore;
        GameController.onLose -= HideCurrentSchore;
        GameController.onLose -= SetYourSchoreCount;
        GameController.onLose -= ShowYourSchore;
        GameController.onLose -= scoreService.SetNewScore;
        GameController.onLose -= scoreService.SetScorePrefs;
        GameController.onLose -= SetTextToRecords;
        ActionRestartButton.onRestartGame -= HideYourSchore;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
        scoreService.GetScorePrefs();
    }

    private void Update()
    {
        UpdateCurrentSchoreCount();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
        scoreService.SetScorePrefs();
    }

    private void UpdateCurrentSchoreCount()
    {
        scoreService.UpdateCurrentSchoreCount(currentSchoreCountTMP);   
    }

    private void SetYourSchoreCount()
    {
        scoreService.SetYourSchoreCount(yourScoreCountTMP);
    }
    
    // Перемещает число очков в правый верхний угол экрана.
    private void ShowCurrentSchore()
    {
        scoreService.ShowCurrentSchore(currentSchoreCountTMP);
    }

    // Прячет число очков.
    private void HideCurrentSchore()
    {
        scoreService.HideCurrentSchore(currentSchoreCountTMP);
    }

    private void ShowYourSchore()
    {
        scoreService.ShowYourSchore(yourSchoreTransform);  
    }

    private void HideYourSchore()
    {
        scoreService.HideYourSchore(yourSchoreTransform);
    }

    private void SetTextToRecords()
    {
        scoreService.SetTextToRecords
            (firstPlaceText, secondPlaceText, thirdPlaceText, 
                fourthPlaceText, fifthPlaceText);
    }
}