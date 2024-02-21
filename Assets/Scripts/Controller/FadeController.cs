

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    
    public static FadeController instance { get; private set; }
    private FadeService fadeService = FadeService.instance;
    
    public Animator animator;
    public Canvas fadeCanvas;

    public static string MAIN_SCENE = "MainScene";
    public static string RECORD_SCENE = "RecordScene";

    private FadeController() {}
    
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
        ActionMainSceneButton.onBackToMainScene += FadeToCamera;
        ActionRecordButton.onShowRecords += FadeToCamera;
    }

    private void UnsubscribeEvents()
    {
        ActionMainSceneButton.onBackToMainScene -= FadeToCamera;
        ActionRecordButton.onShowRecords -= FadeToCamera;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    public void FadeToCamera(Camera camera)
    {
        fadeService.FadeToCamera(camera, animator);
    }   

    public void OnFadeComplete()
    {
        fadeService.OnFadeComplete(fadeCanvas, animator);
    }
}