

using System;
using UnityEngine;
using Random = System.Random;

public class SoundController : MonoBehaviour
{
    public static SoundController instance { get; private set; }

    private SoundController()
    {
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        CreateSingleton();
    }

    
}