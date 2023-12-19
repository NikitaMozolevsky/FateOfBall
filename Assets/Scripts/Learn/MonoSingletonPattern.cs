

using System;
using Unity.VisualScripting;
using UnityEngine;

public class MonoSingletonPattern : MonoBehaviour
{

    public static MonoSingletonPattern _instance { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Объект не уничтожается при переходе сцен
            return;
        }
        
        Destroy(gameObject); // Уничтожение лишнего
    }
}