
using UnityEngine;

public class DestroyOnInvisible : MonoBehaviour
{
    
    /*public static DestroyOnInvisible instance { get; private set; }
    
    private DestroyOnInvisible()
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
    }*/
    
    private void Start()
    {
        /*CreateSingleton();*/
    }
    private void OnBecameInvisible()
    {
        // Если объект стал невидимым, удаляем его
        if (PlatformController.instance.smoothlyPlatformsRaised)
        {
            Destroy(gameObject);
        }
    }
}