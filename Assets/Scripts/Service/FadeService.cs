

using UnityEngine;

public class FadeService
{
    private static FadeService _instance;

    private Camera nextCamera;
    
    // Приоритеты камер.
    public const int HIGH_PRIORITY = 11;
    public const int LOW_PRIORITY = 9;
    // Имя триггена.
    public static string FADE_TRIGGER = "FadeTrigger";
    public static string UNFADE_TRIGGER = "UnfadeTrigger";
    
    private FadeService()
    {
    }

    public static FadeService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FadeService();
            }
            return _instance;
        }
    }
    
    // Меняет приоритеты камер а не сцены.
    public void ShowRecordsCamera()
    {
        CameraController.instance.mainCamera.depth = LOW_PRIORITY;
        CameraController.instance.recordsCamera.depth = HIGH_PRIORITY;
    }

    // Меняет приоритеты камер а не сцены.
    public void ShowMainCamera()
    {
        CameraController.instance.recordsCamera.depth = LOW_PRIORITY;
        CameraController.instance.mainCamera.depth = HIGH_PRIORITY;
    }

    public void FadeToCamera(Camera camera, Animator animator)
    {
        nextCamera = camera;
        animator.SetTrigger(FADE_TRIGGER);
    }

    public void OnFadeComplete(Canvas fadeCanvas, Animator animator)
    {
        if (nextCamera == CameraController.instance.mainCamera)
        {
            ShowMainCamera();
        }

        if (nextCamera == CameraController.instance.recordsCamera)
        {
            ShowRecordsCamera();
        }

        fadeCanvas.worldCamera = nextCamera;
        animator.SetTrigger(UNFADE_TRIGGER);
    }
}