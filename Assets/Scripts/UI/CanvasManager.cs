

using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance { get; private set; }
    
    public AnimationCurve showElementCurve;
    public AnimationCurve hideElementCurve;

    public const float TIME_TO_MOVE_ELEMENT = 1f;
    
    private CanvasManager()
    {
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        CreateSingleton();
    }
}